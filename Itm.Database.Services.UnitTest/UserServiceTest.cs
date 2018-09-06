using System;
using System.Collections.Generic;
using System.Diagnostics;
using AutoMapper;
using Itm.Database.Context;
using Itm.Database.Core.Entities;
using Itm.Log.Core;
using Itm.Models;
using Itm.ObjectMap;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Xunit;

namespace Itm.Database.Services.UnitTest
{
    public class UserServiceTest
    {
        [Fact]
        //public async Task AddUserAsync_AddUser_Success()
        public void AddUserAsync_AddUser_Success()
        {
            // arrange
            // ILogger
            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(_ => _.Info(It.IsAny<string>()))
                .Callback((string message) => Trace.WriteLine(message));

            // IMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
				cfg.AddProfile (new RoleProfile ());
			});
            var mapper = config.CreateMapper();

            // IAppUser
            var appUser = new AppUser(1, "LoggedUser");

            //AppDbContext
            var options = new DbContextOptionsBuilder<AppDbContext>()
                  .UseInMemoryDatabase(Guid.NewGuid().ToString())
                  .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                  .Options;

            var appDbContext = new AppDbContext(options);

			IRoleService roleService = new RoleService (loggerMock.Object, mapper, appUser, appDbContext);
			IUserService userService = new UserService(loggerMock.Object, mapper, appUser, appDbContext);

			var newRole = new RoleModel
			{
				Name = "Role 1",
				Description = "Role 1 Description"
			};

			// act
			var dbRole = roleService.AddRoleAsync (newRole);

			var newUser = new UserModel
            {
                FirstName = "User-" + DateTime.Now.ToString(),
                LastName = "Last Name",
                UserName = "Username",
                Password = "Password",
				Roles = new List<RoleModel> { dbRole.Result.Model }
            };

            // act
            var dbUser = userService.AddUserAsync(newUser);

            // assert
            Assert.Equal(newUser.FirstName, dbUser.Result.Model.FirstName);
        }
    }
}
