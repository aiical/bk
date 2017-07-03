using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using CourseManager.Category;
using CourseManager.Category.Dtos;
using CourseManager.ClassHourStatistics.Dto;
using CourseManager.SignIn;
using CourseManager.Students;
using CourseManager.Students.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManager.ClassHourStatistics
{
    public class ClassHourStatisticsAppService : CourseManagerAppServiceBase, IClassHourStatisticsAppService
    {

        private readonly IRepository<SignInRecord, string> _signInRepository;
        private readonly ICategorysAppService _categorysAppService;
        private readonly IStudentAppService _studentAppService;
        public ClassHourStatisticsAppService(
            IRepository<SignInRecord, string> signInRepository,
            ICategorysAppService categorysAppService,
             IStudentAppService studentAppService)
        {
            this._signInRepository = signInRepository;
            this._categorysAppService = categorysAppService;
            this._studentAppService = studentAppService;
        }

        private IQueryable<SignInRecord> GetArrangesByCondition(ClassHourStatisticsInput input)
        {
            var endTime = input.EndTime.AddDays(1);
            var query = _signInRepository.GetAll()
                 //.WhereIf(input.TeacherId!=null,t=>t.TeacherId==input.TeacherId.Value)
                 .WhereIf(!input.StudentId.IsNullOrEmpty(), t => t.StudentId == input.StudentId)
                           .WhereIf(input.BeginTime != null && input.BeginTime.ToString() != "0001/1/1 0:00:00", o => o.BeginTime > input.BeginTime)
                        .WhereIf(input.BeginTime != null && input.BeginTime.ToString() != "0001/1/1 0:00:00" && input.EndTime != null, o => (input.BeginTime < o.BeginTime && o.EndTime < endTime))
                        .Where(o => o.IsDeleted == false);
            query = string.IsNullOrEmpty(input.Sorting)
                        ? query.OrderByDescending(t => t.CreationTime)
                        : query.OrderBy(t => input.Sorting);
            return query;
        }



        public ListResultDto<ClassHourStatisticsOutput> GetClassHourStatistics(ClassHourStatisticsInput input)
        {
            var classHours = GetArrangesByCondition(input);// _signInRepository.GetAllList();
            if (classHours == null) return new ListResultDto<ClassHourStatisticsOutput>();
            var list = classHours.MapTo<List<ClassHourStatisticsOutput>>();
            SetOtherExtendData(list);
            return new ListResultDto<ClassHourStatisticsOutput>(list);
        }
        private void SetOtherExtendData(IEnumerable<ClassHourStatisticsOutput> list)
        {
            if (list != null && list.Any())
            {
                //
                var categoryData = _categorysAppService.GetCategorysPageListBy(new CategorysInput { });
                var students = _studentAppService.GetStudents();
                StudentListDto studentModel = new StudentListDto();
                foreach (var item in list)
                {
                    if (!string.IsNullOrEmpty(item.ClassType) && categoryData.Any(c => c.Id == item.ClassType))
                        item.ClassTypeName = categoryData.FirstOrDefault(c => c.Id == item.ClassType).CategoryName;
                    if (!string.IsNullOrEmpty(item.Type) && categoryData.Any(c => c.Id == item.Type))
                        item.TypeName = categoryData.FirstOrDefault(c => c.Id == item.Type).CategoryName;
                    if (!string.IsNullOrEmpty(item.CourseType) && categoryData.Any(c => c.Id == item.CourseType))
                        item.CourseTypeName = categoryData.FirstOrDefault(c => c.Id == item.CourseType).CategoryName;
                    if (!string.IsNullOrEmpty(item.UnNormalType) && categoryData.Any(c => c.Id == item.UnNormalType))
                        item.UnNormalTypeName = categoryData.FirstOrDefault(c => c.Id == item.UnNormalType).CategoryName;
                    var studentIds = item.StudentId.Split(',');
                    string stuName = string.Empty;
                    foreach (var stu in studentIds)
                    {
                        studentModel = students.Items.FirstOrDefault(s => s.Id == stu);
                        if (studentModel != null && !string.IsNullOrEmpty(studentModel.Id))
                            stuName += students.Items.FirstOrDefault(s => s.Id == stu).CnName;
                    }
                    item.StudentName = stuName;
                }
            }
        }

        public async Task<ListResultDto<ClassHourStatisticsOutput>> GetClassHourStatisticsAsync()
        {
            var stus = await _signInRepository.GetAllListAsync();

            return new ListResultDto<ClassHourStatisticsOutput>(
                stus.MapTo<List<ClassHourStatisticsOutput>>()
                );
        }
    }
}
