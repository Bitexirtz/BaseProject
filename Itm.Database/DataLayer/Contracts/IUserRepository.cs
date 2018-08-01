using System.Linq;
using System.Threading.Tasks;
using Itm.Database.Core.Services;
using Itm.Database.EntityLayer;

namespace Itm.Database.DataLayer.Contracts
{
	public interface IUserRepository : IRepository
	{
		IQueryable<User> GetAll (int pageSize = 10, int pageNumber = 1);

		Task<User> GetAsync (User entity);

		Task<int> AddAsync (User entity);

		Task<int> UpdateAsync (User changes);

		Task<int> DeleteAsync (User entity);
	}
}
