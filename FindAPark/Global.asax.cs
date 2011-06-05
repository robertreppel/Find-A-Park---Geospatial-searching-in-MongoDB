using System.Web.Mvc;
using System.Web.Routing;
using FindAPark.Models;
using GeoData;
using Ninject;
using Ninject.Web.Mvc;

namespace FindAPark
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            SetupDependencyInjection(); 

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        private static void SetupDependencyInjection()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IMuseums>().To<Museums>();
            kernel.Bind<IGeoDataStore>().To<MongoGeoDataStore>();
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}