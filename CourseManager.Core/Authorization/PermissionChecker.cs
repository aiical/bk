using Abp.Authorization;
using CourseManager.Authorization.Roles;
using CourseManager.MultiTenancy;
using CourseManager.Users;

namespace CourseManager.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
