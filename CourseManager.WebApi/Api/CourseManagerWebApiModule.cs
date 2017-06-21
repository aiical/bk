using System.Reflection;
using System.Web.Http;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;
using CourseManager.Students;
using Swashbuckle.Application;
using System;
using System.IO;
using System.Linq;
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
            ConfigureSwaggerUi();
        }

        private void ConfigureSwaggerUi() ///swagger/ui/index apis/index
        {
            Configuration.Modules.AbpWebApi().HttpConfiguration
               .EnableSwagger(c =>
               {
                   c.SingleApiVersion("v1", "CourseManager Api文档");
                   c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                   var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;  //将application层中的注释添加到SwaggerUI中
                   var commentsFileName = "Bin//CourseManager.Application.xml";
                   var commentsFile = Path.Combine(baseDirectory, commentsFileName);
                   c.IncludeXmlComments(commentsFile);  //将注释的XML文档添加到SwaggerUI中
               })
               .EnableSwaggerUi("apis/{*assetPath}", b =>
               {
                   //对js进行了拓展
                   b.InjectJavaScript(Assembly.GetExecutingAssembly(), "CourseManager.SwaggerUi.scripts.swagger.js");
               });
            //.EnableSwaggerUi();
        }
    }
}
