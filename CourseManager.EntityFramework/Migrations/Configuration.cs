using System.Data.Entity.Migrations;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using CourseManager.Migrations.SeedData;
using EntityFramework.DynamicFilters;

namespace CourseManager.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<CourseManager.EntityFramework.CourseManagerDbContext>, IMultiTenantSeed
    {
        public AbpTenantBase Tenant { get; set; }

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "CourseManager";
        }

        protected override void Seed(CourseManager.EntityFramework.CourseManagerDbContext context)
        {
            context.DisableAllFilters();

            if (Tenant == null)
            {
                //Host seed
                new InitialHostDbBuilder(context).Create();

                //Default tenant seed (in host database).
                new DefaultTenantCreator(context).Create();
                new TenantRoleAndUserBuilder(context, 1).Create();

                new DefaultStudentCreator(context).Create();
                new DefaultCategorysCreator(context).Create();
                new DefaultTeacherCourseArrangesCreator(context).Create();
            }
            else
            {
                //You can add seed for tenant databases and use Tenant property...
            }

            context.SaveChanges();
        }
    }
}
