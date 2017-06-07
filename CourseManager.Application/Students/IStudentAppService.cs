using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CourseManager.Students.Dto;

namespace CourseManager.Students
{
    public interface IStudentAppService : IApplicationService
    {
        Student.Students GetStudent(StudentInput input);
        Task<ListResultDto<StudentListDto>> GetStudentsAsync();
        ListResultDto<StudentListDto> GetStudents();
        Task CreateStudent(CreateStudentInput input);
        PagedResultDto<StudentListDto> GetPagedStudents(StudentInput input);
        void UpdateActiveState(StudentUpdateInput updateInput);
        void DeleteStudent(string id);
    }
}