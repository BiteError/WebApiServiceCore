using System;
using System.Linq;
using System.Linq.Expressions;
using WebApiServices.Entities.Base;

namespace WebApiServices.BaseServices
{
    public interface IDataContext
    {
        IQueryable<T> GetQueryable<T>(bool trackChanges = true, bool disabled = false) 
            where T : DeletedAudit, new();
        IQueryable<T> GetQueryable<T>(Expression<Func<T, bool>> expression, bool trackChanges = true, bool disabled = false, bool deleted = false) 
            where T : DeletedAudit, new();
        T Delete<T>(T item) where T : Audit, new();
        T Insert<T>(T item) where T : Audit, new();
        int Save(Guid userId);
        void DetachContext(Audit entity);
    }
}