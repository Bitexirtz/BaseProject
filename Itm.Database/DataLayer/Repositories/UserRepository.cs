using System.Linq;
using System.Threading.Tasks;
using Itm.Database.Context;
using Itm.Database.Core.EF.Repositories;
using Itm.Database.Core.Entities;
using Itm.Database.DataLayer.Contracts;
using Itm.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Itm.Database.DataLayer.Repositories
{
	public class UserRepository : Repository, IUserRepository
	{
		public UserRepository (IUserInfo userInfo, AppDbContext dbContext)
            : base(userInfo, dbContext)
        {
		}

		#region "Read Method"

		public async Task<User> GetByIDAsync (int userID)
				=> await DbContext.Set<User> ().FirstOrDefaultAsync (item => item.ID == userID);

		public IQueryable<User> GetAll (int pageSize = 10, int pageNumber = 1)
				=> DbContext.Paging<User> (pageSize, pageNumber);
		#endregion "Read Method"

		#region "Write Method"
		public async Task<int> AddAsync (User entity)
		{
			Add (entity);

			return await CommitChangesAsync ();
		}

		public async Task<int> DeleteAsync (User entity)
		{
			Remove (entity);

			return await CommitChangesAsync ();
		}

		public async Task<int> UpdateAsync (User changes)
		{
			Update (changes);

			return await CommitChangesAsync ();
		}
		#endregion "Write Method"
	}
}
