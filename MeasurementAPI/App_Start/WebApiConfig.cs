using System.Web.Http;

namespace Measurements.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
              name: "DefaultAPI",
              routeTemplate: "api/{controller}/{id}",
              defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
