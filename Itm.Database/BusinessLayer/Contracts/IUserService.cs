using System.Threading.Tasks;
using Itm.Database.Core.Contracts;
using Itm.Database.Core.Services;
using Itm.Database.EntityLayer;

namespace Itm.Database.BusinessLayer.Contracts
{
	public interface IUserService : IService
	{
		Task<IListResponse<User>> GetUsersAsync (int pageSize = 0, int pageNumber = 0);

		Task<ISingleResponse<User>> GetUserAsync (User entity);

		Task<ISingleResponse<User>> UpdateUserAsync (User updates);

		Task<ISingleResponse<User>> CreateOrderAsync (User details);
	}
}
