using System;

namespace WebApiServices.BaseServices
{
    public interface ICacheProvider
    {
        void Invalidate<T>(string id);
        T Get<T>(string id, Func<string, T> func);
        TResult Get<TResult, TParam>(TParam id, Func<TParam, TResult> func);
    }
}