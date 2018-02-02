using System.Web.Http;
using Measurements.App_Start;
using Owin;

namespace Measurements
{
    public class Startup
    {
        public void Configuration(IAppBuilder appbuilder)
        {
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            appbuilder.UseWebApi(httpConfiguration);
        }
    }
}
