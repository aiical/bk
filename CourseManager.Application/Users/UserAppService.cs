using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using CourseManager.Authorization;
using CourseManager.Users.Dto;
using Microsoft.AspNet.Identity;
using System.Linq;
using Abp.Linq.Extensions;
using Abp.Extensions;

namespace CourseManager.Users
{
    /* THIS IS JUST A SAMPLE. */
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserAppService : CourseManagerAppServiceBase, IUserAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IPermissionManager _permissionManager;

        public UserAppService(IRepository<User, long> userRepository, IPermissionManager permissionManager)
        {
            _userRepository = userRepository;
            _permissionManager = permissionManager;
        }

        public async Task ProhibitPermission(ProhibitPermissionInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            var permission = _permissionManager.GetPermission(input.PermissionName);

            await UserManager.ProhibitPermissionAsync(user, permission);
        }

        //Example for primitive method parameters.
        public async Task RemoveFromRole(long userId, string roleName)
        {
            CheckErrors(await UserManager.RemoveFromRoleAsync(userId, roleName));
        }

        public async Task<ListResultDto<UserListDto>> GetUsersAsync()
        {
            var users = await _userRepository.GetAllListAsync();

            return new ListResultDto<UserListDto>(
                users.MapTo<List<UserListDto>>()
                );
        }

        public ListResultDto<UserListDto> GetUsers(UserInput input)
        {
            var users = GetUsersByCondition(input);
            return new ListResultDto<UserListDto>(
                users.MapTo<List<UserListDto>>()
                );
        }
        public async Task CreateUser(CreateUserInput input)
        {
            var user = input.MapTo<User>();

            user.TenantId = AbpSession.TenantId;
            user.Password = new PasswordHasher().HashPassword(input.Password);
            user.IsEmailConfirmed = true;

            CheckErrors(await UserManager.CreateAsync(user));
        }

        private IQueryable<User> GetUsersByCondition(UserInput input)
        {
            var query = _userRepository.GetAll()
                .WhereIf(!input.UserName.IsNullOrEmpty(), o => o.UserName == input.UserName)
                        .WhereIf(!input.Name.IsNullOrEmpty(), o => o.Name.Contains(input.Name))
                        .Where(o => o.IsDeleted == false);
            query = string.IsNullOrEmpty(input.Sorting)
                        ? query.OrderByDescending(t => t.CreationTime)
                        : query.OrderBy(t => input.Sorting);
            return query;
        }
    }
}