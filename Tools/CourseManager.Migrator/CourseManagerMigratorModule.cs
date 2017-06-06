using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using CourseManager.EntityFramework;

namespace CourseManager.Migrator
{
    [DependsOn(typeof(CourseManagerDataModule))]
    public class CourseManagerMigratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<CourseManagerDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}