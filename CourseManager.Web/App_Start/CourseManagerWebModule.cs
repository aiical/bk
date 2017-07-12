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
using Abp.Threading.BackgroundWorkers;
using CourseManager.HangfireTest;
using CourseManager.AuditLog;
using Abp.Runtime.Caching.Redis;
namespace CourseManager.Web
{
    [DependsOn(
        typeof(CourseManagerDataModule),
        typeof(CourseManagerApplicationModule),
        typeof(CourseManagerWebApiModule),
        // typeof(AbpWebSignalRModule),
        typeof(AbpHangfireModule), //-ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
        typeof(AbpWebMvcModule),
        typeof(AbpRedisCacheModule)
        )]
    public class CourseManagerWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Caching.UseRedis();
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
            Configuration.Auditing.IsEnabled = false; //记录访问日志
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
        /// <summary>
        /// 虽然一般我们将工作者添加到PostInitialize方法中，但是没有强制要求。你可以在任何地方注入IBackgroundWorkerManager，在运行时添加工作者。当应用要关闭时，IBackgroundWorkerManager会停止并释放所有注册的工作者
        /// </summary>

        //注释后台任务
        public override void PostInitialize()
        {
            //var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            //workManager.Add(IocManager.Resolve<MakeInactiveUsersPassiveWorker>());
        }
    }
}
