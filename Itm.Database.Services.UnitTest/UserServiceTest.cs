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
        Mock<ILogger> _loggerMock;
        IMapper _mapper;
        AppDbContext _appDbContext;
        AppUser _appUser;

        public UserServiceTest()
        {
            // ILogger
            _loggerMock = new Mock<ILogger>();
            _loggerMock.Setup(_ => _.Info(It.IsAny<string>()))
                .Callback((string message) => Trace.WriteLine(message));

            // IMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new RoleProfile());
            });
            _mapper = config.CreateMapper();

            // IAppUser
            _appUser = new AppUser(1, "LoggedUser");

            //AppDbContext
            var options = new DbContextOptionsBuilder<AppDbContext>()
                  .UseInMemoryDatabase(Guid.NewGuid().ToString())
                  .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                  .Options;

            _appDbContext = new AppDbContext(options);
        }

        [Fact]
        public void AddUserAsync_AddUserWithRole_RelationIsCreated ()
        {
            // arrange
            IRoleService roleService = new RoleService(_loggerMock.Object, _mapper, _appUser, _appDbContext);
            IUserService userService = new UserService(_loggerMock.Object, _mapper, _appUser, _appDbContext);

            var newRole = new RoleModel
            {
                Name = "Role 1",
                Description = "Role 1 Description"
            };

            // act
            var dbRole = roleService.AddRoleAsync(newRole);

            var newUser = new UserModel
            {
                FirstName = "User With Role",
                LastName = "Last Name",
                UserName = "Username",
                Password = "Password",
            };

            // act
            var dbUser = userService.AddUserAsync(newUser);

            // assert
            Assert.Equal(newUser.FirstName, dbUser.Result.Model.FirstName);
        }

        [Fact]
        public void AddUserAsync_AddUser_Success()
        {
            // arrange
			IUserService userService = new UserService(_loggerMock.Object, _mapper, _appUser, _appDbContext);

			// act
			var newUser = new UserModel
            {
                FirstName = "User-" + DateTime.Now.ToString(),
                LastName = "Last Name",
                UserName = "Username",
                Password = "Password",
            };

            // act
            var dbUser = userService.AddUserAsync(newUser);

            // assert
            Assert.Equal(newUser.FirstName, dbUser.Result.Model.FirstName);
        }
    }
}
