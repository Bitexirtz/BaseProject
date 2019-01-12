using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itm.Database.Context;
using Itm.Database.Core.Entities;
using Itm.Database.Services;
using Itm.Log;
using Itm.Log.Core;
using Itm.Models;
using Itm.ObjectMap;
using Microsoft.EntityFrameworkCore;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using AutoMapper;

namespace Itm.Database.Console
{
    internal class Program
    {
        private static void Main (string[] args)
        {
            IUnityContainer container = new UnityContainer ();

            #region SQL Server
            //var dbConnection = ConfigurationManager.ConnectionStrings["AppDbConnection"].ConnectionString;
            //container.RegisterType<IDatabaseConnection, DefaultDatabaseConnection>(new InjectionConstructor(dbConnection));
            //var options = new DbContextOptionsBuilder<AppDbContext>()
            //.UseSqlServer(dbConnection)
            //.Options;
            #endregion SQL Server

            #region SQLite
            var options = new DbContextOptionsBuilder<AppDbContext> ()
                .UseSqlite ("Data Source=Itm.db3;").Options;
            #endregion SQLite

            //container.RegisterType<ILogger>(new InjectionFactory(l => LogManager.GetCurrentClassLogger()));
            //container.RegisterType<ILogger>(LogHelper.GetLogger<NLog>(LogManager.GetCurrentClassLogger()));
            container.RegisterType<AppDbContext> (new TransientLifetimeManager (), new InjectionConstructor (options));

            container.RegisterType<IMapper, ObjectMapper> ();

            container.RegisterType<IAppUser, AppUser> (new InjectionConstructor (1, "LoggedUser"));
            container.RegisterType<ILogger, Logger> (new InjectionConstructor ());
            container.RegisterType<IAccountService, AccountService> ();

            CreateAccount (container).Wait ();

        }

        private static UserModel CreateUser(string first, string middle, string last)
        {
            return new UserModel
            {
                FirstName = first,
                MiddleName = middle,
                LastName = last,
                UserName = $"{first}-username",
                Password = $"{first}-password"
            };
        }

        private static RoleModel CreateRole(string roleName, bool allow = true)
        {
            return new RoleModel
            {
                Name = roleName,
                Description = $"{roleName}-description",
                Allow = allow
            };
        }

        private static ResourceModel CreateResource(string resourceName)
        {
            return new ResourceModel {
                Name = resourceName,
                Description = $"{resourceName}-description"
            };
        }

        private static async Task CreateAccount (IUnityContainer container)
        {
            IAccountService accountService = container.Resolve<IAccountService> ();

            // About this test
            // 1. Create User
            // 2. Create Roles
            // 3. Create Resources
            // 4. Create Role <-> Resources
            // 5. Create User <-> Role

            #region "User"
            // 1. Create User
            var newUser = await accountService.AddUserAsync (CreateUser ("user1", "user1", "user1"));
            #endregion "User"

            #region "Role"
            // 2. Create Roles
            var role1 = await accountService.AddRoleAsync (CreateRole ("role1"));
            var role2 = await accountService.AddRoleAsync (CreateRole ("role2"));
            var role3 = await accountService.AddRoleAsync (CreateRole ("role3"));
            #endregion "Role"

            #region "Resource"
            // 3. Create Resources
            var resource1 = await accountService.AddResourceAsync (CreateResource ("resource1"));
            var resource2 = await accountService.AddResourceAsync (CreateResource ("resource2"));
            var resource3 = await accountService.AddResourceAsync (CreateResource ("resource3"));
            var resource4 = await accountService.AddResourceAsync (CreateResource ("resource4"));
            #endregion "Resource"

            #region "Role Resource"
            // 4. Create Role <-> Resource
            await accountService.AddRoleResourceAsync (role1.Model.ID, resource1.Model.ID);
            await accountService.AddRoleResourceAsync (role1.Model.ID, resource2.Model.ID);
            await accountService.AddRoleResourceAsync (role1.Model.ID, resource3.Model.ID);
            await accountService.AddRoleResourceAsync (role2.Model.ID, resource1.Model.ID);
            await accountService.AddRoleResourceAsync (role2.Model.ID, resource4.Model.ID);
            await accountService.AddRoleResourceAsync (role3.Model.ID, resource1.Model.ID);
            #endregion "Role Resource"

            #region "User <-> Role"
            // 5. Create User <-> Role
            await accountService.AddUserRoleAsync (newUser.Model.ID, role3.Model.ID);
            #endregion "User <-> Role"

            // Get user
            var response = await accountService.GetFirstOrDefaultAsync (1);
        }
    }
}