﻿using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Itm.Database.Context;
using Itm.Database.Core.Entities;
using Itm.Database.Core.Services;
using Itm.Database.Entities;
using Itm.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace Itm.Database.Console
{
	class Program
    {
        static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();

            #region SQL Server
            var dbConnection = ConfigurationManager.ConnectionStrings["AppDbConnection"].ConnectionString;
            container.RegisterType<IDatabaseConnection, DefaultDatabaseConnection>(new InjectionConstructor(dbConnection));
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(dbConnection)
            .Options;
            #endregion SQL Server

            #region SQLite
            //var options = new DbContextOptionsBuilder<AppDbContext>()
            //    .UseSqlite("Data Source=AppData.db;").Options;
            #endregion SQLite

            container.RegisterType<AppDbContext>(new TransientLifetimeManager(), new InjectionConstructor(options));
            container.RegisterType<IUserInfo, UserInfo>();
            container.RegisterType<ILogger>(new InjectionFactory((c) => null));
            container.RegisterType<IUserService, UserService>();
            IUserInfo user = container.Resolve<IUserInfo>();

            MainAsync(container).Wait();
        }

        static async Task MainAsync(IUnityContainer container)
        {
            IUserService repo = container.Resolve<IUserService>();

            var newUser = new User
            {
                FirstName = "User-" + DateTime.Now.ToString(),
                BirthDate = DateTime.Now,
                LastName = "Last Name"
            };


            await repo.AddUserAsync(newUser);

            var updateUser = repo.GetUsersByIDAsync(1);
            updateUser.Result.Model.FirstName = "Modified First Name";

            await repo.UpdateUserAsync(updateUser.Result.Model);

            var list = repo.GetUsersAsync().Result.Model.ToList();


            System.Console.WriteLine(list.Count);
        }
    }
}