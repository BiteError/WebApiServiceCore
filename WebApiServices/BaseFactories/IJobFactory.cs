using System;
using System.Threading.Tasks;

namespace WebApiServices.BaseFactories
{
    public interface IJobFactory
    {
        Task CreateJob<T>(Action<T> jobAction);
        Task<TResult> CreateJob<TService, TResult>(Func<TService, TResult> jobAction);
    }
}