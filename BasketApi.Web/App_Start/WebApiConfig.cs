using System.Web.Http;
using BasketApi.App_Start;
using BasketApi.Repositories;
using BasketApi.Services;
using Unity;
using Unity.Lifetime;

namespace BasketApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new UnityContainer();

            // Scope set to per thread to simplify manual / automated testing
            container.RegisterType<IRepositoryService, InMemoryRepositoryService>(new PerThreadLifetimeManager());
            container.RegisterType<IBasketService, BasketService>(new HierarchicalLifetimeManager());


            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
