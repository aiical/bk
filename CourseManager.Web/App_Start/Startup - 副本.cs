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
using Abp.Hangfire;

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

            //Hangfire可以提供一个面板页面，实时显示所有后台作业的状态，你可以按它自己的文档描述那样配置，默认情况下，所有用户都可以使用这个面板页面，不需要授权，你可以用定义在Abp.HangFire包里的AbphangfireAuthorizationFilter类，把它集成到ABP的授权系统里
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization=new[] {new AbpHangfireAuthorizationFilter() } //如果需要得到一个额外的许可可以再 AbpHangfireAuthorizationFilter 构造函数中传参数 MyHangFireDashboardPermissionName
            });
            //注意：UsehangifreDashboard应该在你的Startup类里的授权中间件运行后调用（可能是在最后一行）。否则，授权会一直失败。

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