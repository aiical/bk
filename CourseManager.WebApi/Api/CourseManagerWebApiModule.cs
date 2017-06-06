using System.Reflection;
using System.Web.Http;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;
using CourseManager.Students;

namespace CourseManager.Api
{
    [DependsOn(typeof(AbpWebApiModule), typeof(CourseManagerApplicationModule))]
    public class CourseManagerWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(CourseManagerApplicationModule).Assembly, "app")
                .Build();
            //Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
            //    .For<IStudentAppService>("app/student")
            //    .Build();
            Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationFilter("Bearer"));
        }
    }
}
