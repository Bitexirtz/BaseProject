using System.Linq;
using System.Threading.Tasks;
using Itm.Database.Core.Entities;
using Itm.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Itm.Database.Repositories
{

    public class JUserRoleRepository : BaseRepository, IJUserRoleRoleRepository
    {
        public JUserRoleRepository(IAppUser userInfo, DbContext dbContext) : base (userInfo, dbContext)
        {

        }

        #region "Read Method"

        public IQueryable<JUserRole> GetAll(int pageSize = 10, int pageNumber = 1)
                => DbContext.Paging<JUserRole>(pageSize, pageNumber);
        #endregion "Read Method"

        #region "Write Method"
        public async Task<int> AddAsync(JUserRole entity)
        {
            DbContext.Set<JUserRole>().Add(entity);

            return await CommitChangesAsync();
        }

        public async Task<int> DeleteAsync(JUserRole entity)
        {
            DbContext.Set<JUserRole>().Remove(entity);

            return await CommitChangesAsync();
        }
        #endregion "Write Method"
    }
}
