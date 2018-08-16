using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Itm.Database.Services;
using Itm.Definitions;
using Itm.Model.Extensions;
using Itm.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Itm.Module.UserManagement.ViewModels
{
	public class UserRegistrationViewModel : BindableBase
	{
		IUserService _userService;
		IRegionManager _regionManager;

		public DelegateCommand OnOkCommand { get; private set; }

		UserModel _newUser;
		public UserModel NewUser
		{
			get { return _newUser; }
			set { SetProperty(ref _newUser, value); }
		}

		public UserRegistrationViewModel(IUserService userService, IRegionManager regionManager)
		{
			_regionManager = regionManager;
			_userService = userService;
			_newUser = new UserModel();
			_newUser.FirstName = "Mark";

			OnOkCommand = new DelegateCommand(OnOk);
		}

		private void OnOk()
		{
			if (_newUser.IsComplete())
			{
				_userService.AddUserAsync(_newUser);
			}
			
			_newUser = new UserModel();
			_regionManager.RequestNavigate(RegionNames.ContentRegion, typeof(Views.UserManagement).ToString());
		}
	}
}
