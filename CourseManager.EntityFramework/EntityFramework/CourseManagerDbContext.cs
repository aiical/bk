using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using Abp.Zero.EntityFramework;
using CourseManager.Authorization.Roles;
using CourseManager.MultiTenancy;
using CourseManager.Users;
using CourseManager.File;
using CourseManager.Student;
using CourseManager.Category;
using CourseManager.SignIn;
using CourseManager.CourseArrange;

namespace CourseManager.EntityFramework
{
    public class CourseManagerDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        //TODO: Define an IDbSet for your Entities...
        public IDbSet<Students> Students { get; set; }
        public IDbSet<Files> Files { get; set; }
        public IDbSet<Categorys> Categorys { get; set; }

        public IDbSet<TeacherCourseArrange> TeacherCourseArrange { get; set; }
        public IDbSet<SignInRecord> SignInRecord { get; set; }
        public IDbSet<StudentCourseArrange> StudentCourseArrange { get; set; }
        /// <summary>
        /// 覆盖方法OnModelCreating  然后将SurName设置为忽略  然后将Name EmailAddress设置为可空。
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().Ignore(u => u.Surname);
            modelBuilder.Entity<User>().Property(u => u.Name).IsOptional();
            modelBuilder.Entity<User>().Property(u => u.EmailAddress).IsOptional();//设定为非必填项
        }
        /* NOTE: 
		 *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
		 *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
		 *   pass connection string name to base classes. ABP works either way.
		 */
        public CourseManagerDbContext()
            : base("Default")
        {
            //调试时输出EF执行sql到VS的输出窗口
            Database.Log = sql => Debug.Write(sql);
        }

        /* NOTE:
		 *   This constructor is used by ABP to pass connection string defined in CourseManagerDataModule.PreInitialize.
		 *   Notice that, actually you will not directly create an instance of CourseManagerDbContext since ABP automatically handles it.
		 */
        public CourseManagerDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Database.Log = sql => Debug.Write(sql);
        }

        //This constructor is used in tests
        public CourseManagerDbContext(DbConnection existingConnection)
         : base(existingConnection, false)
        {
            Database.Log = sql => Debug.Write(sql);
        }

        public CourseManagerDbContext(DbConnection existingConnection, bool contextOwnsConnection)
         : base(existingConnection, contextOwnsConnection)
        {
            Database.Log = sql => Debug.Write(sql);
        }
    }
}
