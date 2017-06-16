using System;
using Abp.Castle.Logging.Log4Net;
using Abp.Web;
using Castle.Facilities.Logging;
using System.Web.Mvc;
using log4net;
using System.Web;

namespace CourseManager.Web
{
    public class MvcApplication : AbpWebApplication<CourseManagerWebModule>
    {

        protected override void Application_Start(object sender, EventArgs e)
        {
            AbpBootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig("log4net.config")
            );
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            base.Application_Start(sender, e);
        }

        protected override void Application_Error(object sender, EventArgs e)
        {
            var exception = ((HttpApplication)sender).Server.GetLastError();
            var lastError = exception.GetBaseException() as HttpException;
            if (lastError != null)
            {
                var httpStatusCode = lastError.GetHttpCode();
                LogManager.GetLogger("Application_Error").Error(lastError.InnerException == null ? lastError.Message : lastError.InnerException.Message, lastError.InnerException ?? lastError);
                if (httpStatusCode == 404)
                {
                    Response.Redirect("~/404.html");
                }
            }
            base.Application_Error(sender, e);
        }
    }
}
