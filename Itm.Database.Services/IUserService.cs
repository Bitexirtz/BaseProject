using System.Threading.Tasks;
using Itm.Database.Core.Services;
using Itm.Database.Core.Services.ResponseTypes;
using Itm.Models;

namespace Itm.Database.Services
{
	public interface IUserService : IService
	{
		Task<IListResponse<UserModel>> GetUsersAsync (int pageSize = 0, int pageNumber = 0);

		Task<ISingleResponse<UserModel>> GetUsersByIDAsync (int userID);

		Task<ISingleResponse<UserModel>> UpdateUserAsync (UserModel updates);

		Task<ISingleResponse<UserModel>> AddUserAsync (UserModel details);
	}
}
