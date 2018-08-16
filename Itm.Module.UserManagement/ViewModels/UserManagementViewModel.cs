using System.Linq;
using Itm.Database.Services;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Itm.Models;
using System;
using Prism.Commands;
using Prism.Regions;
using Itm.Definitions;

namespace Itm.Module.UserManagement.ViewModels
{
	public class UserManagementViewModel : BindableBase
	{

		IUserService _userService;
		IEventAggregator _eventAggregator;
		IRegionManager _regionManager;

		public DelegateCommand OnNewUser { get; private set; }

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

		public UserManagementViewModel (IUserService userService, IEventAggregator eventAggregator, IRegionManager regionManager)
		{
			_regionManager = regionManager;
			_userService = userService;
			_eventAggregator = eventAggregator;

			Users = new ObservableCollection<UserModel>(_userService.GetUsersAsync ().Result.Model.ToList ());

			_title = "-User Management Title-";

			OnNewUser = new DelegateCommand(CreateNewUser);
		}

		private void CreateNewUser()
		{
			_regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(Views.UserRegistration).ToString());
		}
	}
}
