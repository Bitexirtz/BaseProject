using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Itm.Database.BusinessLayer.Contracts;
using Itm.Database.BusinessLayer.Services;
using Itm.Database.Core.Contracts;
using Itm.Database.Core.Services;
using Itm.Database.DataLayer;
using Itm.Database.EntityLayer;
using Microsoft.Extensions.Logging;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace Itm.Database.Console
{
	class Program
	{
		static void Main (string[] args)
		{
			IUnityContainer container = new UnityContainer ();

			var dbConnection = ConfigurationManager.ConnectionStrings["AppDbConnection"].ConnectionString;
			container.RegisterType<IDatabaseConnection, DefaultDatabaseConnection> (new InjectionConstructor (dbConnection));
			container.RegisterType<IUserInfo, UserInfo> ();

			container.RegisterType<ILogger> (new InjectionFactory ((c) => null));

			container.RegisterType<AppDbContext> (new TransientLifetimeManager ());
			//container.RegisterType<AppDbContext> (new PerResolveLifetimeManager (), new InjectionConstructor (new ResolvedParameter<IDatabaseConnection> ("DefaultDatabaseConnection")));

			container.RegisterType<IUserService, UserService> ();
			//container.RegisterType<IUserRepository, UserRepository> ();
			IDatabaseConnection connection = container.Resolve<IDatabaseConnection> ();
			IUserInfo user = container.Resolve<IUserInfo> ();

			MainAsync (container).Wait ();
		}

		static async Task MainAsync (IUnityContainer container)
		{
			IUserService repo = container.Resolve<IUserService> ();

			var newUser = new User
			{
				ID = 7,
				FirstName = "New User",
				BirthDate = DateTime.Now,
				LastName = "Amena"
			};


			await repo.AddUserAsync (newUser);
			//await repo.AddAsync (newUser);

			var updateUser = repo.GetUsersByIDAsync (8);
			updateUser.Result.Model.FirstName = "Aki";

			await repo.UpdateUserAsync (updateUser.Result.Model);

			var list = repo.GetUsersAsync ().Result.Model.ToList ();


			System.Console.WriteLine (list.Count);
		}
	}
}