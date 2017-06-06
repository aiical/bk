using System.Threading.Tasks;
using Abp.Application.Services;
using CourseManager.Roles.Dto;

namespace CourseManager.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);
    }
}
