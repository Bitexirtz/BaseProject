using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using Itm.Database.Context;
using Itm.Database.Core.Entities;
using Itm.Database.Core.Services;
using Itm.Database.Services;
using Itm.Log;
using Itm.Log.Core;
using Itm.Models;
using Itm.ObjectMap;
using Microsoft.EntityFrameworkCore;
using Unity;
using Unity.Injection;

namespace Itm.Database.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			IUnityContainer container = new UnityContainer();
			//Logger _logger = new Logger ("Internal.log");

			var dbConnection = ConfigurationManager.ConnectionStrings["AppDbConnection"].ConnectionString;
			container.RegisterType<IDatabaseConnection, DefaultDatabaseConnection>(new InjectionConstructor(dbConnection));

			#region SQL Server
			//var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(dbConnection).Options;
			#endregion SQL Server

			#region SQLite
			var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite("Data Source=AppData.db3;").Options;
			#endregion SQLite

			container.RegisterType<ILogger, Logger> (new InjectionConstructor ());
			container.RegisterType<AppDbContext>(new InjectionConstructor(options));
			container.RegisterType<IMapper, ObjectMapper> ();
			container.RegisterType<IAppUser, AppUser>(new InjectionConstructor(1, "LoggedUser"));
			container.RegisterType<IUserService, UserService>();
            container.RegisterType<IRoleService, RoleService>();
            IAppUser user = container.Resolve<IAppUser>();

            Trace.WriteLine("Trace Start");

            Trace.Flush();
            MainAsync(container).Wait();

            System.Console.Write("--Finished--");
		}

		static async Task MainAsync(IUnityContainer container)
		{
			IUserService userService = container.Resolve<IUserService>();
            IRoleService roleService = container.Resolve<IRoleService>();

            var newRole = new RoleModel
            {
                Name = "Role 1",
                Description = "Role 1 Description"
            };

            var newDbRole = await roleService.AddRoleAsync(newRole);

            var newUser = new UserModel
            {
                FirstName = "User-" + DateTime.Now.ToString(),
                LastName = "Last Name",
                UserName = "Username",
                Password = "Password",
                Roles = new List<RoleModel> { newRole }
            };

            var newDbUser = await userService.AddUserAsync(newUser);
            if(newDbRole.DidError == false && newDbUser.DidError == false)
            {
                await userService.AddUserRoleAsync(newDbUser.Model, new List<RoleModel> { newDbRole.Model });
            }

            //var updateUser = repo.GetUserByIDWithDetailsAsync(1);
            //if (updateUser.Result.Model != null && updateUser.Result.DidError == false)
            //{
            //    updateUser.Result.Model.LastName = "Update-1";
            //    await repo.UpdateUserAsync(updateUser.Result.Model);
            //}

            //var updateUser = repo.GetUsersByIDAsync (1);
            //if (updateUser.Result.Model != null && updateUser.Result.DidError == false) {
            //	updateUser.Result.Model.LastName = "Update-1";
            //	await repo.UpdateUserAsync (updateUser.Result.Model);
            //}

            //var list = repo.GetUsersAsync ().Result.Model.ToList ();
            //System.Console.WriteLine (list.Count);
        }
	}
}