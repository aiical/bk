using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CourseManager.Students.Dto;

namespace CourseManager.Students
{
    public interface IStudentAppService : IApplicationService
    {

        Task<ListResultDto<StudentListDto>> GetStudents();

        Task CreateStudent(CreateStudentInput input);
    }
}