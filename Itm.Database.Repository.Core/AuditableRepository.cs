using Itm.Database.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Itm.Database.Repository.Core
{
    public abstract class AuditableRepository<TEntity> : AuditableRepositoryBase where TEntity : class, IAuditableEntity
    {
        public AuditableRepository(IAppUser userInfo, DbContext dbContext) : base(userInfo, dbContext)
        {
        }

        #region "Read Method"
        public async Task<TEntity> GetByIDAsync(int itemID)
                => await DbContext.Set<TEntity>().FirstOrDefaultAsync(item => item.ID == itemID);

        public IQueryable<TEntity> GetAll(int pageSize = 10, int pageNumber = 1)
                => DbContext.Paging<TEntity>(pageSize, pageNumber);
        #endregion "Read Method"

        #region "Write Method"
        public async Task<int> AddAsync(TEntity entity)
        {
            Add(entity);

            return await CommitChangesAsync();
        }

        public async Task<int> DeleteAsync(TEntity entity)
        {
            Remove(entity);

            return await CommitChangesAsync();
        }

        public async Task<int> UpdateAsync(TEntity changes)
        {
            Update(changes);

            return await CommitChangesAsync();
        }
        #endregion "Write Method"

    }
}
