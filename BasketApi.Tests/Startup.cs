using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Owin;
using BasketApi.Controllers;

namespace BasketApi.Tests
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            config.MapHttpAttributeRoutes();

            config.Services.Replace(typeof(IAssembliesResolver), new CustomAssembliesResolver());
            
            appBuilder.UseWebApi(config);
        }
    }

    internal class CustomAssembliesResolver : DefaultAssembliesResolver
    {
        public override ICollection<System.Reflection.Assembly> GetAssemblies()
        {
            var assemblies = base.GetAssemblies();
            var customControllersAssembly = typeof(BasketController).Assembly;

            if (!assemblies.Contains(customControllersAssembly))
                assemblies.Add(customControllersAssembly);

            return assemblies;
        }
    }
}
