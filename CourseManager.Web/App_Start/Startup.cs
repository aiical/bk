using System;
using System.Configuration;
using Abp.Owin;
using CourseManager.Api.Controllers;
using CourseManager.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
//using Microsoft.Owin.Security.Facebook;
//using Microsoft.Owin.Security.Google;
//using Microsoft.Owin.Security.Twitter;
using Owin;
using Hangfire;
[assembly: OwinStartup(typeof(Startup))]

namespace CourseManager.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAbp();

            app.UseOAuthBearerAuthentication(AccountController.OAuthBearerOptions);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            //http://www.cnblogs.com/ecin/p/6201262.html
            GlobalConfiguration.Configuration.UseSqlServerStorage("default");
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            ConfigureTask(app);

            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //if (IsTrue("ExternalAuth.Facebook.IsEnabled"))
            //{
            //    app.UseFacebookAuthentication(CreateFacebookAuthOptions());
            //}

            //if (IsTrue("ExternalAuth.Twitter.IsEnabled"))
            //{
            //    app.UseTwitterAuthentication(CreateTwitterAuthOptions());
            //}

            //if (IsTrue("ExternalAuth.Google.IsEnabled"))
            //{
            //    app.UseGoogleAuthentication(CreateGoogleAuthOptions());
            //}

            // app.MapSignalR();
        }
        //测试用 我们可以在各个controller中创建任务   访问http://localhost:10086/hangfire查看面板  分别执行下面配置的地址查看效果
        private void ConfigureTask(IAppBuilder app)
        {
            //BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget"));

            //延迟任务执行：不是马上调用方法，而是设定一个未来时间点再来执行。使用方法：
            //BackgroundJob.Schedule(() => Console.WriteLine("Reliable!"), TimeSpan.FromDays(7));

            // 循环任务执行：只需要简单的一行代码就可以添加重复执行的任务，其内置了常见的时间循环模式，也可以基于CRON表达式来设定复杂的模式。使用方法：
            //RecurringJob.AddOrUpdate(() => Console.WriteLine("Transparent!"), Cron.Daily);
            app.Map("/index", r =>
            {
                r.Run(context =>
                {
                    //任务每分钟执行一次
                    RecurringJob.AddOrUpdate(() => Console.WriteLine($"ASP.NET Core LineZero"), Cron.Daily());
                    return context.Response.WriteAsync("ok");
                });
            });

            app.Map("/one", r =>
            {
                r.Run(context =>
                {
                    //任务执行一次
                    BackgroundJob.Enqueue(() => Console.WriteLine($"ASP.NET Core One Start LineZero{DateTime.Now}"));
                    return context.Response.WriteAsync("ok");
                });
            });

            app.Map("/await", r =>
            {
                r.Run(context =>
                {
                    //任务延时两分钟执行
                    BackgroundJob.Schedule(() => Console.WriteLine($"ASP.NET Core await LineZero{DateTime.Now}"), TimeSpan.FromDays(2));//TimeSpan.FromMinutes(2)
                    return context.Response.WriteAsync("ok");
                });
            });
        }
        //private static FacebookAuthenticationOptions CreateFacebookAuthOptions()
        //{
        //    var options = new FacebookAuthenticationOptions
        //    {
        //        AppId = ConfigurationManager.AppSettings["ExternalAuth.Facebook.AppId"],
        //        AppSecret = ConfigurationManager.AppSettings["ExternalAuth.Facebook.AppSecret"]
        //    };

        //    options.Scope.Add("email");
        //    options.Scope.Add("public_profile");

        //    return options;
        //}

        //private static TwitterAuthenticationOptions CreateTwitterAuthOptions()
        //{
        //    return new TwitterAuthenticationOptions
        //    {
        //        ConsumerKey = ConfigurationManager.AppSettings["ExternalAuth.Twitter.ConsumerKey"],
        //        ConsumerSecret = ConfigurationManager.AppSettings["ExternalAuth.Twitter.ConsumerSecret"]
        //    };
        //}

        //private static GoogleOAuth2AuthenticationOptions CreateGoogleAuthOptions()
        //{
        //    return new GoogleOAuth2AuthenticationOptions
        //    {
        //        ClientId = ConfigurationManager.AppSettings["ExternalAuth.Google.ClientId"],
        //        ClientSecret = ConfigurationManager.AppSettings["ExternalAuth.Google.ClientSecret"]
        //    };
        //}

        private static bool IsTrue(string appSettingName)
        {
            return string.Equals(
                ConfigurationManager.AppSettings[appSettingName],
                "true",
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}