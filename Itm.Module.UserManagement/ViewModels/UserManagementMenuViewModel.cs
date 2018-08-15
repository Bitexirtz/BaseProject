using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Itm.Definitions;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Itm.Module.UserManagement.ViewModels
{
	public class UserManagementMenuViewModel : BindableBase
	{
		private readonly IRegionManager _regionManager;

		public DelegateCommand OnNavigate { get; private set; }

		public string Color { get; private set; }

		public string Caption { get; private set; }

		public string ImagePath { get; private set; }

		public UserManagementMenuViewModel (IRegionManager regionManager)
		{
			_regionManager = regionManager;

			OnNavigate = new DelegateCommand (Navigate);

			Color = "#0000ff";
			Caption = "User Management";
			ImagePath = "../Resources/Images/menu-icon.png";
		}

		private void Navigate ()
		{
			_regionManager.RequestNavigate (RegionNames.ContentRegion, typeof(Views.UserManagement).ToString());
		}
	}
}
