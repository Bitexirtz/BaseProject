using System.Linq;
using Itm.Database.Services;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Itm.Models;
using System;

namespace Itm.Module.UserManagement.ViewModels
{
	public class UserManagementViewModel : BindableBase
	{
		IUserService _userService;
		IEventAggregator _eventAggregator;

		private string _title;

		public string Title
		{
			get { return _title; }
			set { SetProperty (ref _title, value); }
		}


		private ObservableCollection<UserModel> _users;
		public ObservableCollection<UserModel> Users
		{
			get { return _users; }
			set { SetProperty (ref _users, value); }
		}

		public UserManagementViewModel (IUserService userService, IEventAggregator eventAggregator)
		{
			_userService = userService;
			_eventAggregator = eventAggregator;

			var newUser = new UserModel
			{
				FirstName = "User-" + DateTime.Now.ToString (),
				BirthDate = DateTime.Now,
				LastName = "Last Name",
				UserName = "Username",
				Password = "Password"
			};

			_userService.AddUserAsync (newUser);

			Users = new ObservableCollection<UserModel>(_userService.GetUsersAsync ().Result.Model.ToList ());

			_title = "-User Management Title-";
		}
	}
}
