using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using CourseManager.Authorization;
using CourseManager.Common;
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

        public Student.Students GetStudent(StudentInput input)
        {
            return GetStudentsByCondition(input).FirstOrDefault(); //_studentRepository.Get(input.Id);
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
            var stus = _studentRepository.GetAllList();

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

        public PagedResultDto<StudentListDto> GetPagedStudents(StudentInput input)
        {
            var query = GetStudentsByCondition(input);
            var count = query.Count();
            input.SkipCount = ((input.PIndex ?? 1) - 1) * (input.PSize ?? 10);
            input.MaxResultCount = input.PSize ?? 10;
            var list = query.PageBy(input).ToList();

            return new PagedResultDto<StudentListDto>(count, list.MapTo<List<StudentListDto>>());
        }
        public async Task CreateStudent(CreateStudentInput input)
        {
            var student = input.MapTo<Student.Students>();
            if (!string.IsNullOrEmpty(input.Id)) await _studentRepository.UpdateAsync(student);
            else
            {
                student.Id = IdentityCreator.NewGuid;
                await _studentRepository.InsertAsync(student);
            }
        }
        public void UpdateActiveState(StudentUpdateInput updateInput)
        {
            var model = _studentRepository.Get(updateInput.Id);
            if (model != null)
            {
                model.IsActive = !updateInput.IsActive;
                _studentRepository.Update(model);
            }
        }
        public void DeleteStudent(string id)
        {
            var student = _studentRepository.Get(id);
            if (student != null)
                _studentRepository.Delete(id);
            //{
            //    student.IsActive = false;
            //    _studentRepository.Update(student);
            //}
        }
    }
}