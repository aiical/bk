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
    public class StudentCourseArrangeAppService : CourseManagerAppServiceBase, IStudentCourseArrangeAppService
    {
        private readonly IRepository<StudentCourseArrange, string> _studentCourseArrangeRepository;
        private readonly IEventBus _eventBus;
        private readonly IStudentAppService _studentAppService;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IRepository<User, long> _userRepository;
        private readonly ISmtpEmailSender _smtpEmailSender;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public StudentCourseArrangeAppService(
             IRepository<StudentCourseArrange, string> studentCourseArrangeRepository,
            IStudentAppService studentAppService,
        IRepository<User, long> userRepository,
            ISmtpEmailSender smtpEmailSender,
            INotificationPublisher notificationPublisher,
            IEventBus eventBus,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            this._studentCourseArrangeRepository = studentCourseArrangeRepository;
            this._studentAppService = studentAppService;
             _userRepository = userRepository;
            _smtpEmailSender = smtpEmailSender;
            _notificationPublisher = notificationPublisher;
            _eventBus = eventBus;
            this._unitOfWorkManager = unitOfWorkManager;
        }
        private IQueryable<StudentCourseArrange> GetArrangesByCondition(StudentCourseArrangeInput input)
        {
            //WhereIf 是ABP针对IQueryable<T>的扩展方法 第一个参数为条件，第二个参数为一个Predicate 当条件为true执行后面的条件过滤
            var query = _studentCourseArrangeRepository.GetAll()
                        .WhereIf(!input.Id.IsNullOrEmpty(), o => o.Id == input.Id)
                         .WhereIf(!input.StudentId.IsNullOrEmpty(), o => o.StudentId == input.StudentId)
                        .WhereIf(!input.ClassType.IsNullOrEmpty(), t => t.ClassType == input.Filter)
                        .WhereIf(input.BeginTime != null, o => o.BeginTime.Value > input.BeginTime)
                        .WhereIf(input.BeginTime != null && input.EndTime != null, o => (input.BeginTime < o.BeginTime.Value && o.EndTime.Value < input.EndTime))
                        .Where(o => o.IsDeleted == false);
            query = string.IsNullOrEmpty(input.Sorting)
                        ? query.OrderBy(t => t.BeginTime)
                        : query.OrderBy(t => input.Sorting);
            return query;
        }
        public async Task<ListResultDto<StudentCourseArrangeListDto>> GetArranagesAsync()
        {
            var stus = await _studentCourseArrangeRepository.GetAllListAsync();

            return new ListResultDto<StudentCourseArrangeListDto>(
                stus.MapTo<List<StudentCourseArrangeListDto>>()
                );
        }

        public ListResultDto<StudentCourseArrangeListDto> GetArranages(StudentCourseArrangeInput input)
        {
            var res = GetArrangesByCondition(input);
            var mapData = res.MapTo<List<StudentCourseArrangeListDto>>();
          // SetOtherExtendData(mapData);
            return new ListResultDto<StudentCourseArrangeListDto>(mapData);
        }

        private void SetOtherExtendData(IEnumerable<StudentCourseArrangeListDto> list)
        {
           
        }

        public StudentCourseArrange GetArranage(StudentCourseArrangeInput input)
        {
            return _studentCourseArrangeRepository.Get(input.Id);
        }
        public List<StudentCourseArrange2SignInOutput> GetStudentCourseArrange2SignIn(StudentCourseArrangeInput input)
        {
            var res = GetArrangesByCondition(input).ToList();
            List<StudentCourseArrange2SignInOutput> output = new List<StudentCourseArrange2SignInOutput>();
            foreach (var item in res)
            {
                output.Add(new StudentCourseArrange2SignInOutput()
                {
                    TimeDuration = string.Format("{0}--{1}", item.BeginTime.Value.ToString("HH:mm"), item.EndTime.Value.ToString("HH:mm")),
                    Id = item.Id
                });
            }
            return output;
        }
        public PagedResultDto<StudentCourseArrangeListDto> GetPagedArranges(StudentCourseArrangeInput input)
        {
            var query = GetArrangesByCondition(input);
            var count = query.Count();
            input.SkipCount = (input.PIndex ?? 1 - 1) * input.PSize ?? 10;
            input.MaxResultCount = input.PSize ?? 10;
            var list = query.PageBy(input).ToList();//ABP提供了扩展方法PageBy分页方式

            return new PagedResultDto<StudentCourseArrangeListDto>(count, list.MapTo<List<StudentCourseArrangeListDto>>());
        }

        /// <summary>
        /// 排课
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool StudentArrangeCourse(CreateStudentCourseArrangeInput input)
        {
            Logger.Info("AddStudentCourseArrange: " + input);
            var arrange = input.MapTo<StudentCourseArrange>();
            string studentIds = GenerateStudentIds(input);
            arrange.StudentId = studentIds;
            bool result = true;
            if (!string.IsNullOrEmpty(input.Id)) result = !string.IsNullOrEmpty(_studentCourseArrangeRepository.Update(arrange).Id);
            else
            {
                
                if (input.CrossWeek.Value)
                {
                    StringBuilder builder = new StringBuilder();
                    using (var unitOfWork = _unitOfWorkManager.Begin()) //启用工作单元  
                    {
                        try
                        {
                            var lastDate=CalendarHelper.LastDayOfMonth(input.BeginTime);
                            var crossTimes = Math.Floor(Convert.ToDecimal((lastDate.Day - input.BeginTime.Day) / 7));
                            for (int i =0; i <= crossTimes; i++)
                            {
                                var newArrange = input.MapTo<StudentCourseArrange>();
                                newArrange.Id = IdentityCreator.NewGuid;
                                newArrange.ArrangeTime = DateTime.Now;
                                newArrange.CreatorUserId = AbpSession.UserId.Value;
                                newArrange.StudentId = studentIds;
                                newArrange.BeginTime = newArrange.BeginTime.Value.AddDays(7*i);
                                newArrange.EndTime = newArrange.EndTime.Value.AddDays(7*i);
                                _studentCourseArrangeRepository.Insert(newArrange);
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
                    result = !string.IsNullOrEmpty(_studentCourseArrangeRepository.Insert(arrange).Id); //.InsertOrUpdate(arrange);
                }
            }
           
            return result;
        }
        private string GenerateStudentIds(CreateStudentCourseArrangeInput input)
        {
            var students = input.StudentId.Trim().Split(',');
            var studentIds = "";
            foreach (var stu in students)
            {
                studentIds += _studentAppService.GetStudent(new StudentInput() { CnName = stu }).Id + ",";
            }
            return studentIds.TrimEnd(',');
        }
        public bool UpdateCourseArrange(UpdateStudentCourseArrangeInput updateInput)
        {
            var upadteArrange = updateInput.MapTo<StudentCourseArrange>();
            return !string.IsNullOrEmpty(_studentCourseArrangeRepository.Update(upadteArrange).Id);
        }
        public bool UpdateCourseArrange(StudentCourseArrange updateModel)
        {
            return !string.IsNullOrEmpty(_studentCourseArrangeRepository.Update(updateModel).Id);
        }
    }
}
