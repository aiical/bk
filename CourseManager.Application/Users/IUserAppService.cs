using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CourseManager.Users.Dto;

namespace CourseManager.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task ProhibitPermission(ProhibitPermissionInput input);

        Task RemoveFromRole(long userId, string roleName);

        Task<ListResultDto<UserListDto>> GetUsersAsync();
        ListResultDto<UserListDto> GetUsers(UserInput input);
        Task CreateUser(CreateUserInput input);
    }
}