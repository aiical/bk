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
using Abp.Domain.Uow;
using CourseManager.Students;
using CourseManager.Students.Dto;

namespace CourseManager.CourseArrange
{
    public class TeacherCourseArrangeAppService : CourseManagerAppServiceBase, ITeacherCourseArrangeAppService
    {
        private readonly IRepository<TeacherCourseArrange, string> _teacherCourseArrangeRepository;
        private readonly IEventBus _eventBus;
        private readonly IStudentAppService _studentAppService;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IRepository<User, long> _userRepository;
        private readonly ISmtpEmailSender _smtpEmailSender;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public TeacherCourseArrangeAppService(
            IRepository<TeacherCourseArrange, string> teacherCourseArrangeRepository,
            IStudentAppService studentAppService,
        IRepository<User, long> userRepository,
            ISmtpEmailSender smtpEmailSender,
            INotificationPublisher notificationPublisher,
            IEventBus eventBus,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            this._teacherCourseArrangeRepository = teacherCourseArrangeRepository;
            this._studentAppService = studentAppService;
            _userRepository = userRepository;
            _smtpEmailSender = smtpEmailSender;
            _notificationPublisher = notificationPublisher;
            _eventBus = eventBus;
            this._unitOfWorkManager = unitOfWorkManager;
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
            var mapData = res.MapTo<List<TeacherCourseArrangeListDto>>();
            // SetOtherExtendData(mapData);
            return new ListResultDto<TeacherCourseArrangeListDto>(mapData);
        }

        private void SetOtherExtendData(IEnumerable<TeacherCourseArrangeListDto> list)
        {

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
        public bool TeacherArrangeCourse(CreateTeacherCourseArrangeInput input)
        {
            Logger.Info("AddTeacherCourseArrange: " + input);
            var arrange = input.MapTo<TeacherCourseArrange>();
            string studentIds = GenerateStudentIds(input);
            arrange.StudentId = studentIds;
            bool result = true;
            if (!string.IsNullOrEmpty(input.Id)) result = !string.IsNullOrEmpty(_teacherCourseArrangeRepository.Update(arrange).Id);
            else
            {
                arrange.Id = IdentityCreator.NewGuid;
                arrange.ArrangeTime = DateTime.Now;
                arrange.CreatorUserId = AbpSession.UserId.Value;
                if (input.CrossWeek != null && input.CrossWeek.Value)
                {
                    StringBuilder builder = new StringBuilder();
                    using (var unitOfWork = _unitOfWorkManager.Begin()) //启用工作单元  
                    {
                        try
                        {
                            var lastDate = CalendarHelper.LastDayOfMonth(input.BeginTime);
                            var crossTimes = Math.Floor(Convert.ToDecimal((lastDate.Day - input.BeginTime.Day) / 7));
                            for (int i = 0; i <= crossTimes; i++)
                            {
                                var newArrange = input.MapTo<TeacherCourseArrange>();
                                newArrange.Id = IdentityCreator.NewGuid;
                                newArrange.ArrangeTime = DateTime.Now;
                                newArrange.CreatorUserId = AbpSession.UserId.Value;
                                newArrange.StudentId = studentIds;
                                newArrange.BeginTime = newArrange.BeginTime.Value.AddDays(7 * i);
                                newArrange.EndTime = newArrange.EndTime.Value.AddDays(7 * i);
                                _teacherCourseArrangeRepository.Insert(newArrange);
                            }
                            unitOfWork.Complete(); //提交事务  
                        }
                        catch (Exception ex)
                        {
                            builder.AppendLine(ex.Message);
                        }
                        result = builder.Length == 0;
                    }
                }
                else
                {
                    result = !string.IsNullOrEmpty(_teacherCourseArrangeRepository.Insert(arrange).Id); //.InsertOrUpdate(arrange);
                }
            }
            //只有创建成功才发送邮件和通知
            //if (result != null&&!string.IsNullOrEmpty(result.Id))
            if (result)
            {
                var user = _userRepository.Load(input.TeacherId); //AbpSession.UserId.Valueinput.TeacherId

                //使用领域事件触发发送通知操作
                _eventBus.Trigger(new AddTeacherCourseArrangeEventData(arrange, user));
                // _notificationPublisher.Publish("安排了新的课程额", new MessageNotificationData("安排了新的课程额"), null,
                //  NotificationSeverity.Info, new[] { user.ToUserIdentifier() });
                //TODO:需要配置QQ邮箱密码
                //_smtpEmailSender.Send("ysjshengjie@qq.com", task.AssignedPerson.EmailAddress, "New Todo item", message);

                //_notificationPublisher.Publish("安排了新的课程额", new MessageNotificationData(message), null,
                //    NotificationSeverity.Info, new[] { task.AssignedPerson.ToUserIdentifier() });
            }
            // return result ?? new TeacherCourseArrange();
            return result;
        }
        private string GenerateStudentIds(CreateTeacherCourseArrangeInput input)
        {
            var students = input.StudentId.Trim().Split(',');
            var studentIds = "";
            foreach (var stu in students)
            {
                studentIds += _studentAppService.GetStudent(new StudentInput() { CnName = stu }).Id + ",";
            }
            return studentIds.TrimEnd(',');
        }
        public bool UpdateCourseArrange(UpdateTeacherCourseArrangeInput updateInput)
        {
            var upadteArrange = updateInput.MapTo<TeacherCourseArrange>();
            return !string.IsNullOrEmpty(_teacherCourseArrangeRepository.Update(upadteArrange).Id);
        }
        public bool UpdateCourseArrange(TeacherCourseArrange updateModel)
        {
            return !string.IsNullOrEmpty(_teacherCourseArrangeRepository.Update(updateModel).Id);
        }
    }
}
