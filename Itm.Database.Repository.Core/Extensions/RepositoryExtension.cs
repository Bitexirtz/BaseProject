using System;
using System.Linq;
using System.Linq.Expressions;
using Itm.Database.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Itm.Database.Repository.Core
{
    public static class RepositoryExtension
    {
        public static IQueryable<TEntity> GetFirstOrDefault<TEntity> (this DbSet<TEntity> dbSet,
                                          Expression<Func<TEntity, bool>> predicate = null,
                                          Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, Object>> include = null,
                                          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                          bool disableTracking = true) where TEntity : class, IEntity
        {
            IQueryable<TEntity> query = dbSet;
            if (disableTracking == true) {
                query = query.AsNoTracking ();
            }

            if (include != null) {
                query = include (query);
            }

            if (predicate != null) {
                query = query.Where (predicate);
            }

            if (orderBy != null) {
                return orderBy (query);
            }
            else {
                return query;
            }
        }



        //public static TResult GetFirstOrDefault<TEntity, TResult> (this DbSet<TEntity> dbSet,
        //                                  Expression<Func<TEntity, TResult>> selector,
        //                                  Expression<Func<TEntity, bool>> predicate = null,
        //                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        //                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        //                                  bool disableTracking = true) where TEntity : class, IEntity
        //{
        //    IQueryable<TEntity> query = dbSet;
        //    if (disableTracking == true) {
        //        query = query.AsNoTracking ();
        //    }

        //    if (include != null) {
        //        query = include (query);
        //    }

        //    if (predicate != null) {
        //        query = query.Where (predicate);
        //    }

        //    if (orderBy != null) {
        //        return orderBy (query).Select (selector).FirstOrDefault ();
        //    }
        //    else {
        //        return query.Select (selector).FirstOrDefault ();
        //    }
        //}

        public static IQueryable<TEntity> EagerWhere<TEntity, TProperty> (this DbSet<TEntity> dbSet, Expression<Func<TEntity, TProperty>> navigationPropertyPath, Expression<Func<TEntity, bool>> expr) where TEntity : class, IEntity
		{
			return dbSet
				.Include (navigationPropertyPath)
				.Where (expr);
		}

		public static IQueryable<TEntity> Paging<TEntity> (this DbContext dbContext, Int32 pageSize = 0, Int32 pageNumber = 0) where TEntity : class, IEntity
		{
			var query = dbContext.Set<TEntity> ().AsQueryable ();

			return pageSize > 0 && pageNumber > 0 ? query
				.Skip ((pageNumber - 1) * pageSize)
				.Take (pageSize) : query;
		}

		public static IQueryable<TModel> Paging<TModel> (this IQueryable<TModel> query, Int32 pageSize = 0, Int32 pageNumber = 0) where TModel : class
			=> pageSize > 0 && pageNumber > 0 ? query.Skip ((pageNumber - 1) * pageSize).Take (pageSize) : query;

		public static IQueryable<TEntity> Paging<TEntity, TProperty> (this DbSet<TEntity> dbSet, Expression<Func<TEntity, TProperty>> navigationPropertyPath, Int32 pageSize = 0, Int32 pageNumber = 0) where TEntity : class
		{
			var query = dbSet.Include(navigationPropertyPath).AsQueryable ();

			return pageSize > 0 && pageNumber > 0 ? query
				.Skip ((pageNumber - 1) * pageSize)
				.Take (pageSize) : query;
		}
	}
}
