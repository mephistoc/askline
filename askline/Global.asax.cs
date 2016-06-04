using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using log4net;

[assembly:log4net.Config.XmlConfigurator(Watch = true)]
namespace askline
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static ILog Logger = LogManager.GetLogger(typeof(WebApiApplication));

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
