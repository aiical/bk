using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using CourseManager.CourseArrange.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;
using Abp.AutoMapper;
using CourseManager.Common;
using Abp.Events.Bus;
using Abp.Notifications;
using Abp.Net.Mail.Smtp;
using CourseManager.Users;

namespace CourseManager.CourseArrange
{
    public class TeacherCourseArrangeAppService : CourseManagerAppServiceBase, ITeacherCourseArrangeAppService
    {
        private readonly IRepository<TeacherCourseArrange, string> _teacherCourseArrangeRepository;
        private readonly IEventBus _eventBus;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IRepository<User, long> _userRepository;
        private readonly ISmtpEmailSender _smtpEmailSender;
        public TeacherCourseArrangeAppService(
            IRepository<TeacherCourseArrange, string> teacherCourseArrangeRepository,
              IRepository<User, long> userRepository,
            ISmtpEmailSender smtpEmailSender,
            INotificationPublisher notificationPublisher,
            IEventBus eventBus
            )
        {
            this._teacherCourseArrangeRepository = teacherCourseArrangeRepository;
            _userRepository = userRepository;
            _smtpEmailSender = smtpEmailSender;
            _notificationPublisher = notificationPublisher;
            _eventBus = eventBus;
        }
        private IQueryable<TeacherCourseArrange> GetArrangesByCondition(TeacherCourseArrangeInput input)
        {
            //WhereIf 是ABP针对IQueryable<T>的扩展方法 第一个参数为条件，第二个参数为一个Predicate 当条件为true执行后面的条件过滤
            var query = _teacherCourseArrangeRepository.GetAll()
                        .WhereIf(!input.Id.IsNullOrEmpty(), o => o.Id == input.Id)
                         .WhereIf(input.TeacherId > 0, o => o.TeacherId == input.TeacherId)
                        .WhereIf(!input.ClassType.IsNullOrEmpty(), t => t.ClassType == input.Filter)
                        .WhereIf(input.BeginTime != null, o => o.BeginTime.Value > input.BeginTime)
                        .WhereIf(input.BeginTime != null && input.EndTime != null, o => (input.BeginTime < o.BeginTime.Value && o.EndTime.Value < input.EndTime))
                        .Where(o => o.IsDeleted == false);
            query = string.IsNullOrEmpty(input.Sorting)
                        ? query.OrderBy(t => t.BeginTime)
                        : query.OrderBy(t => input.Sorting);
            return query;
        }
        public async Task<ListResultDto<TeacherCourseArrangeListDto>> GetArranagesAsync()
        {
            var stus = await _teacherCourseArrangeRepository.GetAllListAsync();

            return new ListResultDto<TeacherCourseArrangeListDto>(
                stus.MapTo<List<TeacherCourseArrangeListDto>>()
                );
        }

        public ListResultDto<TeacherCourseArrangeListDto> GetArranages(TeacherCourseArrangeInput input)
        {
            var res = GetArrangesByCondition(input);

            return new ListResultDto<TeacherCourseArrangeListDto>(
                res.MapTo<List<TeacherCourseArrangeListDto>>()
                );
        }
        public TeacherCourseArrange GetArranage(TeacherCourseArrangeInput input)
        {
            return _teacherCourseArrangeRepository.Get(input.Id);
        }
        public List<TeacherCourseArrange2SignInOutput> GetTeacherCourseArrange2SignIn(TeacherCourseArrangeInput input)
        {
            var res = GetArrangesByCondition(input).ToList();
            List<TeacherCourseArrange2SignInOutput> output = new List<TeacherCourseArrange2SignInOutput>();
            foreach (var item in res)
            {
                output.Add(new TeacherCourseArrange2SignInOutput()
                {
                    TimeDuration = string.Format("{0}--{1}", item.BeginTime.Value.ToString("HH:mm"), item.EndTime.Value.ToString("HH:mm")),
                    Id = item.Id
                });
            }
            return output;
        }
        public PagedResultDto<TeacherCourseArrangeListDto> GetPagedArranges(TeacherCourseArrangeInput input)
        {
            var query = GetArrangesByCondition(input);
            var count = query.Count();
            input.SkipCount = (input.PIndex ?? 1 - 1) * input.PSize ?? 10;
            input.MaxResultCount = input.PSize ?? 10;
            var list = query.PageBy(input).ToList();//ABP提供了扩展方法PageBy分页方式

            return new PagedResultDto<TeacherCourseArrangeListDto>(count, list.MapTo<List<TeacherCourseArrangeListDto>>());
        }

        /// <summary>
        /// 排课
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public TeacherCourseArrange TeacherArrangeCourse(CreateTeacherCourseArrangeInput input)
        {
            Logger.Info("AddTeacherCourseArrange: " + input);
            var arrange = input.MapTo<TeacherCourseArrange>();
            TeacherCourseArrange result;
            if (!string.IsNullOrEmpty(input.Id)) result = _teacherCourseArrangeRepository.Update(arrange);
            else
            {
                arrange.Id = IdentityCreator.NewGuid;
                arrange.ArrangeTime = DateTime.Now;
                arrange.CreatorUserId = AbpSession.UserId.Value;
                result = _teacherCourseArrangeRepository.Insert(arrange);
            }

            //只有创建成功才发送邮件和通知
            if (result != null)
            {
                var user = _userRepository.Load(AbpSession.UserId.Value); //input.TeacherId

                //使用领域事件触发发送通知操作
                _eventBus.Trigger(new AddTeacherCourseArrangeEventData(arrange, user));

                //TODO:需要配置QQ邮箱密码
                //_smtpEmailSender.Send("ysjshengjie@qq.com", task.AssignedPerson.EmailAddress, "New Todo item", message);

                //_notificationPublisher.Publish("安排了新的课程额", new MessageNotificationData(message), null,
                //    NotificationSeverity.Info, new[] { task.AssignedPerson.ToUserIdentifier() });
            }
            return result ?? new TeacherCourseArrange();
        }

    }
}
