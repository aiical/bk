using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using CourseManager.Authorization;
using CourseManager.Users.Dto;
using Microsoft.AspNet.Identity;
using CourseManager.Students;
using CourseManager.Students.Dto;
using System;

namespace CourseManager.Users
{
    /* THIS IS JUST A SAMPLE. */
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class StudentAppService : CourseManagerAppServiceBase, IStudentAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IPermissionManager _permissionManager;

        public StudentAppService(IRepository<User, long> userRepository, IPermissionManager permissionManager)
        {
            _userRepository = userRepository;
            _permissionManager = permissionManager;
        }

        public async Task ProhibitPermission(Students.Dto.ProhibitPermissionInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            var permission = _permissionManager.GetPermission(input.PermissionName);

            await UserManager.ProhibitPermissionAsync(user, permission);
        }

        public async Task<ListResultDto<StudentListDto>> GetStudents()
        {
            var users = await _userRepository.GetAllListAsync();

            return new ListResultDto<StudentListDto>(
                users.MapTo<List<StudentListDto>>()
                );
        }

        public async Task CreateStudent(CreateStudentInput input)
        {
            var user = input.MapTo<User>();

            user.TenantId = AbpSession.TenantId;
            

            CheckErrors(await UserManager.CreateAsync(user));
        }
    }
}