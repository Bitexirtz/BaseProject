using Itm.Database.Core.Entities;
using Itm.Database.Entities;
using Itm.Database.Repository.Core;
using Microsoft.EntityFrameworkCore;

namespace Itm.Database.Repositories
{
    public class ResourceRepository : AuditableRepository<Resource>, IResourceRepository
	{
		public ResourceRepository (IAppUser userInfo, DbContext dbContext) : base (userInfo, dbContext)
		{
		}

		#region "Read Method"
		#endregion "Read Method"

		#region "Write Method"
		#endregion "Write Method"
	}
}
