using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Itm.Database.Context;
using Itm.Database.Core.EF.Extensions;
using Itm.Database.Core.Entities;
using Itm.Database.Core.Services.ResponseTypes;
using Itm.Database.Entities;
using Itm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Itm.Database.Services
{
	public class UserService : BaseService, IUserService
	{
		public UserService (ILogger logger, IMapper mapper, IAppUser userInfo, AppDbContext dbContext)
			: base (logger, mapper, userInfo, dbContext)
		{
		}

		public async Task<IListResponse<UserModel>> GetUsersAsync (int pageSize = 0, int pageNumber = 0)
		{
			Logger?.LogInformation (CreateInvokedMethodLog (MethodBase.GetCurrentMethod ().ReflectedType.FullName));

			var response = new ListResponse<UserModel> ();

			try {

				/*response.Model =*/// await UserRepository.GetAll (pageSize, pageNumber).ToListAsync ();

				response.Model = await UserRepository.GetAll (pageSize, pageNumber).Select (o => Mapper.Map<UserModel> (o)).ToListAsync ();
			}
			catch (Exception ex) {
				response.SetError (ex, Logger);
			}

			return response;
		}

		public async Task<ISingleResponse<UserModel>> GetUsersByIDAsync (int userID)
		{
			Logger?.LogInformation (CreateInvokedMethodLog (MethodBase.GetCurrentMethod ().ReflectedType.FullName));

			var response = new SingleResponse<UserModel> ();

			try {
				response.Model = Mapper.Map<UserModel> (await UserRepository.GetByIDAsync (userID));
			}
			catch (Exception ex) {
				response.SetError (ex, Logger);
			}

			return response;
		}


		public async Task<ISingleResponse<UserModel>> AddUserAsync (UserModel details)
		{
			var response = new SingleResponse<UserModel> ();

			using (var transaction = DbContext.Database.BeginTransaction ()) {
				try {

					var user = Mapper.Map<User>(details);
					await UserRepository.AddAsync(user);

					var userCredential = Mapper.Map<UserCredential>(details);
					userCredential.User = user;
					await UserCredentialRepository.AddAsync(userCredential);

					transaction.Commit ();
					response.Model = Mapper.Map<UserModel> (user);
				}
				catch (Exception ex) {
					transaction.Rollback ();
					throw ex;
				}
			}

			return response;
		}

		public async Task<ISingleResponse<UserModel>> UpdateUserAsync (UserModel updates)
		{
			Logger?.LogInformation (CreateInvokedMethodLog (MethodBase.GetCurrentMethod ().ReflectedType.FullName));

			var response = new SingleResponse<UserModel> ();

			using (var transaction = DbContext.Database.BeginTransaction ()) {
				try {
					await UserRepository.UpdateAsync (Mapper.Map<User> (updates));

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
