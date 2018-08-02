using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Itm.Database.Core.Contracts;
using Itm.Database.Core.Services;
using Itm.Database.DataLayer;
using Itm.Database.DataLayer.Contracts;
using Itm.Database.DataLayer.Repositories;
using Itm.Database.EntityLayer;
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

			container.RegisterType<AppDbContext> ();
			//container.RegisterType<AppDbContext> (new PerResolveLifetimeManager (), new InjectionConstructor (new ResolvedParameter<IDatabaseConnection> ("DefaultDatabaseConnection")));

			container.RegisterType<IUserRepository, UserRepository> ();
			IDatabaseConnection connection = container.Resolve<IDatabaseConnection> ();
			IUserInfo user = container.Resolve<IUserInfo> ();
			IUserRepository repo = container.Resolve<IUserRepository> ();

			repo.AddAsync (new User
			{
				FirstName = "Mark",
				BirthDate = DateTime.Now,
				LastName = "Amena"
			});

			//var list = repo.GetAll ().ToList();


			//System.Console.WriteLine (list.Count);
		}
	}
}