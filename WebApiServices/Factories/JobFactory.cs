using System;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using WebApiServices.BaseFactories;

namespace WebApiServices.Factories
{
    public class JobFactory : IJobFactory
    {
        private readonly IUnityContainer container;

        public JobFactory(IUnityContainer container)
        {
            this.container = container;
        }

        public Task CreateJob<TService>(Action<TService> jobAction)
        {
            return Task.Factory.StartNew(() =>
            {
                var service = container.Resolve<TService>();
                jobAction(service);
            });
        }

        public Task<TResult> CreateJob<TService, TResult>(Func<TService, TResult> jobAction)
        {
            return Task.Factory.StartNew(() =>
            {
                var service = container.Resolve<TService>();
                return jobAction(service);
            });
        }
    }
}
