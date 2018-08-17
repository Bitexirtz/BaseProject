using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Itm.Database.Context;
using Itm.Database.Services.Extensions;
using Itm.Database.Core.Exception;
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
				var userDetails = await UserRepository.GetByIDAsync(userID);

				if(userDetails != null)
				{
					// Load also User Credentials
					DbContext.Entry(userDetails).Reference(r => r.UserCredential).Load();
				}

				response.Model = Mapper.Map<UserModel> (userDetails);
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

					User user = await UserRepository.GetByIDAsync (updates.ID);
					if(user == null) {
						throw new DatabaseException ("User record not found.");
					}

					//DO NOT USE: Will set User properties to NULL if property not exists in UserModel. Use instead: Mapper.Map(updates, user);
					//user = Mapper.Map<User> (updates); 

					Mapper.Map(updates, user);
					//Mapper.Map<UserCredential> (updates);

					await UserRepository.UpdateAsync (user);

					transaction.Commit ();
					response.Model = Mapper.Map<UserModel>(user);
				}
				catch (Exception ex) {
					transaction.Rollback ();
					response.SetError (ex, Logger);
				}
			}

			return response;
		}

		public async Task<ISingleResponse<UserModel>> RemoveUserAsync(int userID)
		{
			Logger?.LogInformation(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

			var response = new SingleResponse<UserModel>();

			try
			{
				// Retrieve user by id
				User user = await UserRepository.GetByIDAsync(userID);
				if (user == null)
				{
					throw new DatabaseException("User record not found.");
				}

				//await UserCredentialRepository.DeleteAsync(user.UserCredential);

				await UserRepository.DeleteAsync(user);
				response.Model = Mapper.Map<UserModel>(user);
			}
			catch (Exception ex)
			{
				response.SetError(ex, Logger);
			}

			return response;
		}
	}
}
