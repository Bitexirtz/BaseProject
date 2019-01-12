using Itm.Database.Context;
using Itm.Database.Core.Entities;
using Itm.Database.Entities;
using Itm.Database.Repository.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Itm.Database.Repositories
{
    public class UserRepository : AuditableRepository<User>, IUserRepository
	{
		public UserRepository (IAppUser userInfo, AppDbContext dbContext)
			: base (userInfo, dbContext)
		{
		}

        #region "Read Method"
        public async Task<User> GetFirstOrDefaultAsync (int userID)
        => await DbContext.Set<User> ().AsNoTracking ()
            .Where (u => u.ID == userID)
                .Include (u => u.UserRoles)
                .ThenInclude (ur => ur.Role)
                    .ThenInclude (role => role.RoleResources)
                        .ThenInclude (roleResource => roleResource.Resource)
            .FirstOrDefaultAsync ();

		public async Task<User> GetByIDWithDetailsAsync (int entityID)
				=> await DbContext.Set<User> ().EagerWhere (x => x.UserCredential, m => m.ID == entityID).FirstOrDefaultAsync ();


		public IQueryable<User> GetAllWithDetails (int pageSize = 10, int pageNumber = 1)
				=> DbContext.Set<User> ().Paging (x => x.UserCredential);

		public IQueryable <User> GetAllDetailsWithRole (int entityID)
				=> DbContext.Set<User> ().EagerWhere (x => x.UserRoles, m => m.ID == (entityID != 0 ? entityID : 0) );

		#endregion "Read Method"

		#region "Write Method"
		#endregion "Write Method"
	}
}
