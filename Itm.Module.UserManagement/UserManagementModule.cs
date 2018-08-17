using Itm.Database.Services;
using Itm.Definitions;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace Itm.Module.UserManagement
{
	public class UserManagementModule : IModule
	{
		private IRegionManager _regionManager;
		private IUnityContainer _container;

		public UserManagementModule (IUnityContainer container, IRegionManager regionManager)
		{
			_container = container;
			_regionManager = regionManager;
		}

		public void Initialize ()
		{
			_regionManager.RegisterViewWithRegion (RegionNames.MenuBarRegion, () => _container.Resolve<Views.UserManagementMenu>());
			_regionManager.RegisterViewWithRegion (RegionNames.ContentRegion, () => _container.Resolve <Views.UserManagement>());
		}
	}
}
