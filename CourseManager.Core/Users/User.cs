using System;
using Abp.Authorization.Users;
using Abp.Extensions;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;

namespace CourseManager.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "mq123";

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }
        //隐藏abpzero中的surname 这几个字段没用 如下编写的话 EF在做实体验证的时候就不会对private的属性字段进行验证 生成数据库的时候就没有这个字段了。
        private new string Surname { get; set; }
        [Required(AllowEmptyStrings = true)]
        public override string EmailAddress { get => base.EmailAddress; set => base.EmailAddress = value; }
        public static User CreateTenantAdminUser(int tenantId, string emailAddress, string password)
        {
            return new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Password = new PasswordHasher().HashPassword(password)
            };
        }
    }
}