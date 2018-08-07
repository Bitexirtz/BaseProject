using System.Threading.Tasks;
using Itm.Database.Core.Services;
using Itm.Database.Core.Services.ResponseTypes;
using Itm.Database.Entities;

namespace Itm.Database.Services
{
	public interface IUserService : IService
	{
		Task<IListResponse<User>> GetUsersAsync (int pageSize = 0, int pageNumber = 0);

		Task<ISingleResponse<User>> GetUsersByIDAsync (int userID);

		Task<ISingleResponse<User>> UpdateUserAsync (User updates);

		Task<ISingleResponse<User>> AddUserAsync (User details);
	}
}
