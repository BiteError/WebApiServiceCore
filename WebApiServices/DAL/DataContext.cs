using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using WebApiServices.BaseServices;
using WebApiServices.Entities.Base;

namespace WebApiServices.DAL
{
    public class DataContext : DbContext, IDataContext
    {
        public IQueryable<T> GetQueryable<T>(bool trackChanges = true, bool disabled = false) where T : DeletedAudit, new()
        {
            return GetQueryable<T>(null, trackChanges, disabled);
        }

        public IQueryable<T> GetQueryable<T>(Expression<Func<T, bool>> expression, bool trackChanges = true, bool disabled = false, bool deleted = false) where T : DeletedAudit, new()
        {
            var query = GetQueryableNonAudit(expression, trackChanges, disabled, deleted);
            if (!deleted) query = query.Where(e => e.DeletedOn == null);
            return query;
        }
        public IQueryable<T> GetQueryableNonAudit<T>(Expression<Func<T, bool>> expression, bool trackChanges = true, bool disabled = false, bool deleted = false) where T : Audit, new()
        {
            var query = trackChanges 
                ? Set<T>().AsQueryable() 
                : Set<T>().AsNoTracking();

            if (typeof(T).IsSubclassOf(typeof(IDisablable)))
            {
                query = query.Cast<IDisablable>().Where(e => e.IsDisabled || disabled).Cast<T>();
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }

            return query;
        }

        public T Delete<T>(T item) where T : Audit, new()
        {
            //may do some intercepts
            //return AfterDelete(Set<T>().Remove(BeforeDelete(item)));
            return Set<T>().Remove(item);
        }

        public T Insert<T>(T item) where T : Audit, new()
        {
            //may do some intercepts
            //return AfterInsert(Set<T>().Add(BeforeInsert(item)));
            return Set<T>().Add(item);
        }

        //Save all changes and flush to DB
        public int Save(Guid userId)
        {
            foreach (var entry in ChangeTracker.Entries<Audit>())
            {
                var date = DateTime.UtcNow;

                var entity = entry.Entity as DeletedAudit;

                if (entity?.DeletedOn != null)
                {
                    entity.DeletedBy = userId;
                    entity.DeletedOn = date;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = userId;

                        if (entry.Entity.CreatedOn == DateTime.MinValue)
                        {
                            entry.Entity.CreatedOn = date;
                        }

                        entry.Entity.ModifiedBy = userId;
                        entry.Entity.ModifiedOn = date;

                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = userId;
                        entry.Entity.ModifiedOn = date;

                        break;

                    case EntityState.Unchanged:
                    case EntityState.Detached:
                    case EntityState.Deleted:
                        break;
                }
            }

            return SaveChanges();
        }

        public void DetachContext(Audit entity)
        {
            ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }
    }
}
