using Itm.Database.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Itm.Database.Repository.Core
{
    public abstract class Repository<TEntity> : RepositoryBase where TEntity : class, IEntity
    {
        public Repository(IAppUser userInfo, DbContext dbContext) : base(userInfo, dbContext)
        {
        }

        #region "Read Method"
        public IQueryable<TEntity> GetAll(int pageSize = 10, int pageNumber = 1)
                => DbContext.Paging<TEntity>(pageSize, pageNumber);
        #endregion "Read Method"

        #region "Write Method"
        public async Task<int> AddAsync(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);

            return await CommitChangesAsync();
        }

        public async Task<int> DeleteAsync(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);

            return await CommitChangesAsync();
        }

        public async Task<int> UpdateAsync(TEntity changes)
        {
            DbContext.Set<TEntity>().Update(changes);

            return await CommitChangesAsync();
        }
        #endregion "Write Method"

    }
}
