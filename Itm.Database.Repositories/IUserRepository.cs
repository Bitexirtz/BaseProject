using System.Linq;
using System.Threading.Tasks;
using Itm.Database.Core.Data;
using Itm.Database.Entities;

namespace Itm.Database.Repositories
{
    public interface IUserRepository : IRepository
    {
        IQueryable<User> GetAll(int pageSize = 10, int pageNumber = 1);

        Task<User> GetByIDAsync(int userID);

        Task<int> AddAsync(User entity);

        Task<int> UpdateAsync(User changes);

        Task<int> DeleteAsync(User entity);
    }
}
