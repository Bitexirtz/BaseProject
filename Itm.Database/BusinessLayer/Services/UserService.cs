using System;
using System.Data.Entity;
using System.Reflection;
using System.Threading.Tasks;
using Itm.Database.BusinessLayer.Contracts;
using Itm.Database.Core.Contracts;
using Itm.Database.Core.Responses;
using Itm.Database.DataLayer;
using Itm.Database.EntityLayer;
using Microsoft.Extensions.Logging;

namespace Itm.Database.BusinessLayer.Services
{
	public class UserService : ServiceBase, IUserService
	{
		public UserService (ILogger logger, IUserInfo userInfo, AppDbContext dbContext)
			: base (logger, userInfo, dbContext)
		{
		}

		public async Task<ISingleResponse<User>> GetUserAsync (User entity)
		{
			Logger?.LogInformation (CreateInvokedMethodLog (MethodBase.GetCurrentMethod ().ReflectedType.FullName));

			var response = new SingleResponse<User> ();

			try {
				response.Model = await UserRepository.GetAsync (entity);
			}
			catch (Exception ex) {
				response.SetError (ex, Logger);
			}

			return response;
		}

		public async Task<IListResponse<User>> GetUsersAsync (int pageSize = 0, int pageNumber = 0)
		{
			Logger?.LogInformation (CreateInvokedMethodLog (MethodBase.GetCurrentMethod ().ReflectedType.FullName));

			var response = new ListResponse<User> ();

			try {
				response.Model = await UserRepository.GetAll (pageSize, pageNumber).ToListAsync ();
			}
			catch (Exception ex) {
				response.SetError (ex, Logger);
			}

			return response;
		}

		public async Task<ISingleResponse<User>> CreateOrderAsync (User details)
		{
			var response = new SingleResponse<User> ();

			using (var transaction = DbContext.Database.BeginTransaction ()) {
				try {
					await UserRepository.AddAsync (details);

					transaction.Commit ();
				}
				catch (Exception ex) {
					transaction.Rollback ();
					throw ex;
				}
			}

			return response;
		}

		public async Task<ISingleResponse<User>> UpdateUserAsync (User updates)
		{
			Logger?.LogInformation (CreateInvokedMethodLog (MethodBase.GetCurrentMethod ().ReflectedType.FullName));

			var response = new SingleResponse<User> ();

			using (var transaction = DbContext.Database.BeginTransaction ()) {
				try {
					await UserRepository.UpdateAsync (updates);

					transaction.Commit ();
					response.Model = updates;
				}
				catch (Exception ex) {
					transaction.Rollback ();
					response.SetError (ex, Logger);
				}
			}

			return response;
		}
	}
}
