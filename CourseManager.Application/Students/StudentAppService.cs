using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using CourseManager.Authorization;
using CourseManager.Core.EntitiesFromCustom;
using CourseManager.Students;
using CourseManager.Students.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CourseManager.Users
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class StudentAppService : CourseManagerAppServiceBase, IStudentAppService
    {
        private readonly IRepository<Student.Students, string> _studentRepository;

        public StudentAppService(IRepository<Student.Students, string> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<ListResultDto<StudentListDto>> GetStudentsAsync()
        {
            var stus = await _studentRepository.GetAllListAsync();

            return new ListResultDto<StudentListDto>(
                stus.MapTo<List<StudentListDto>>()
                );
        }
        public ListResultDto<StudentListDto> GetStudents()
        {
            var stus = _studentRepository.GetAllListAsync();

            return new ListResultDto<StudentListDto>(
                stus.MapTo<List<StudentListDto>>()
                );
        }
        private IQueryable<Student.Students> GetStudentsByCondition(StudentInput input)
        {
            //WhereIf 是ABP针对IQueryable<T>的扩展方法 第一个参数为条件，第二个参数为一个Predicate 当条件为true执行后面的条件过滤
            var query = _studentRepository.GetAll()
                        .WhereIf(!input.Id.IsNullOrEmpty(), o => o.Id == input.Id)
                        .WhereIf(!input.Filter.IsNullOrEmpty(), t => t.CnName.Contains(input.Filter))
                        .WhereIf(input.IsActive.HasValue, o => o.IsActive == input.IsActive)
                        .Where(o => o.IsDeleted == false);
            query = string.IsNullOrEmpty(input.Sorting)
                        ? query.OrderByDescending(t => t.CreationTime)
                        : query.OrderBy(t => input.Sorting);
            return query;
        }

        public PagedResultDto<StudentListDto> GetPagedDegrees(StudentInput input)
        {
            var query = GetStudentsByCondition(input);
            var count = query.Count();
            var list = query.PageBy(input).ToList();

            return new PagedResultDto<StudentListDto>(count, list.MapTo<List<StudentListDto>>());
        }
        public async Task CreateStudent(CreateStudentInput input)
        {
            var student = input.MapTo<Student.Students>();
            await _studentRepository.InsertAsync(student);
        }
        public ResultData UpdateActiveState(StudentUpdateInput updateInput)
        {
            ResultData result = new ResultData();
            var model = _studentRepository.Get(updateInput.Id);
            if (model != null)
            {
                model.IsActive = updateInput.IsActive;
                _studentRepository.Update(model);
            }
            else
            {
                result.isSuccess = false;
            }
            return result;
        }
        public void DeleteStudent(string id)
        {
            if (_studentRepository.Get(id) != null)
                _studentRepository.Delete(id);
        }
    }
}