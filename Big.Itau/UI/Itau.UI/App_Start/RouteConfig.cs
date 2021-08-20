using System.Web.Mvc;
using System.Web.Routing;

namespace Mastercard.UI.Interface
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Login

            routes.MapRoute(
                        name: "Default",
                        url: "{controller}/{action}/{id}",
                        defaults: new { controller = "Account", action = "FirstLogin", id = UrlParameter.Optional }
                    );

            #endregion Login

            routes.MapRoute(
                name: "Inicio",
                url: "{id}",
                defaults: new { controller = "Account", action = "FirstLogin", id = UrlParameter.Optional }
            );
        }
    }
}