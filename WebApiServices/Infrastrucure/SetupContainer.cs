using System.Configuration;
using System.Linq;
using Microsoft.Practices.Unity;
using WebApiServices.BaseFactories;
using WebApiServices.BaseServices;
using WebApiServices.DAL;
using WebApiServices.Factories;

namespace WebApiServices.Infrastrucure
{
    public static class SetupContainer
    {
        public static IUnityContainer RegisterCacheProvider(this IUnityContainer container)
        {
            //container.RegisterType<ICacheProvider, CacheProvider>(new PerResolveLifetimeManager());
            return container;
        }

        public static IUnityContainer RegisterServices(this IUnityContainer container)
        {
            //Register all services based on IService except IPrincipalService
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies()
                    .Where(type => typeof(IService).IsAssignableFrom(type)
                    && !typeof(IPrincipalService).IsAssignableFrom(type)),
                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.PerResolve);

            //Register IPrincipalService
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies()
                    .Where(type => typeof(IPrincipalService).IsAssignableFrom(type)),
                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.Hierarchical);

            return container;
        }

        public static IUnityContainer RegisterFactories(this IUnityContainer container)
        {
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies()
                    .Where(type => typeof(IFactory).IsAssignableFrom(type)),
                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.PerResolve);

            container.RegisterType<IJobFactory, JobFactory>(
                new PerResolveLifetimeManager(),
                new InjectionFactory(c => new JobFactory(c))
                );

            return container;
        }

        public static IUnityContainer RegisterDataContext(this IUnityContainer container)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
            
            container.RegisterType<IDataContext, DataContext>(new PerResolveLifetimeManager(),
                new InjectionConstructor(connectionString));
            
            return container;
        }
    }
}
