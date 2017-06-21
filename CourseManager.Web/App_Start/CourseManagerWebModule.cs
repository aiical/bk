using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.Modules;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using Abp.Zero.Configuration;
using CourseManager.Api;
using Hangfire;
using Abp.Configuration.Startup;
using System;

namespace CourseManager.Web
{
    [DependsOn(
        typeof(CourseManagerDataModule),
        typeof(CourseManagerApplicationModule),
        typeof(CourseManagerWebApiModule),
        // typeof(AbpWebSignalRModule),
        //typeof(AbpHangfireModule), - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
        typeof(AbpWebMvcModule))]
    public class CourseManagerWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Enable database based localization
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<CourseManagerNavigationProvider>();
            Configuration.Modules.AbpWeb().AntiForgery.IsEnabled = false; //禁用csrf跨站验证
            //配置所有Cache的默认过期时间为2小时
            Configuration.Caching.ConfigureAll(cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromHours(2);
            });
            //配置指定的Cache过期时间为10分钟
            Configuration.Caching.Configure("ControllerCache", cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(10);
            });
            Configuration.Auditing.IsEnabled = false;
            //Configuration.Modules.AbpWebCommon().MultiTenancy.DomainFormat = WebUrlService.WebSiteRootAddress;
            //Configure Hangfire - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
            //Configuration.BackgroundJobs.UseHangfire(configuration =>
            //{
            //    configuration.GlobalConfiguration.UseSqlServerStorage("Default");
            //});
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
