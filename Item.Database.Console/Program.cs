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
using Microsoft.EntityFrameworkCore;
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
            container.RegisterType<IDatabaseConnection, DefaultDatabaseConnection>(new InjectionConstructor(dbConnection));

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("Data Source=AppData.db;").Options;


            container.RegisterType<AppDbContext>(new TransientLifetimeManager(), new InjectionConstructor(options));
            container.RegisterType<IUserInfo, UserInfo> ();
			container.RegisterType<ILogger> (new InjectionFactory ((c) => null));
			container.RegisterType<IUserService, UserService> ();
			IUserInfo user = container.Resolve<IUserInfo> ();

			MainAsync (container).Wait ();
		}

		static async Task MainAsync (IUnityContainer container)
		{
			IUserService repo = container.Resolve<IUserService> ();

			var newUser = new User
			{
				FirstName = "User-" + DateTime.Now.ToString(),
				BirthDate = DateTime.Now,
				LastName = "Amena"
			};


			await repo.AddUserAsync (newUser);
			//await repo.AddAsync (newUser);

			var updateUser = repo.GetUsersByIDAsync (1);
			updateUser.Result.Model.FirstName = "Aki";

			await repo.UpdateUserAsync (updateUser.Result.Model);

			var list = repo.GetUsersAsync ().Result.Model.ToList ();


			System.Console.WriteLine (list.Count);
		}
	}
}