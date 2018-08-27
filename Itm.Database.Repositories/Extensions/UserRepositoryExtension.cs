using System;
using System.Linq;
using System.Linq.Expressions;
using Itm.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Itm.Database.Repositories.Extensions
{
	public static class UserRepositoryExtension
	{
		public static IQueryable<User> EagerWhere (this DbSet<User> dbSet, Expression<Func<User, bool>> expr)
		{
			return dbSet
				.Include (m => m.UserCredential)
				.Where (expr);
		}

		public static IQueryable<User> PagingWithCredentials (this DbSet<User> dbSet, int pageSize = 0, int pageNumber = 0)
		{
			var query = dbSet
				.Include (m => m.UserCredential).AsQueryable ();

			return pageSize > 0 && pageNumber > 0 ? query
				.Skip ((pageNumber - 1) * pageSize)
				.Take (pageSize) : query;
		}
	}
}