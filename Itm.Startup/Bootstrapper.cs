using System.Windows;
using System.Windows.Controls;
using Itm.Database.Context;
using Itm.Database.Core.Entities;
using Itm.Database.ObjectMapper;
using Itm.Database.Services;
using Itm.Module.UserManagement;
using Itm.RegionAdapters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace Itm.Startup
{
	public class Bootstrapper : UnityBootstrapper
	{
		protected override DependencyObject CreateShell ()
		{
			return Container.TryResolve<MainWindow> ();
		}

		protected override void InitializeShell ()
		{
			Application.Current.MainWindow.Show ();
		}

		protected override RegionAdapterMappings ConfigureRegionAdapterMappings ()
		{
			RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings ();

			mappings.RegisterMapping (typeof (StackPanel), Container.TryResolve<StackPanelRegionAdapter> ());

			return mappings;
		}

		protected override void ConfigureModuleCatalog ()
		{
			var catalog = (ModuleCatalog)ModuleCatalog;

			catalog.AddModule (typeof (UserManagementModule));
		}

		protected override void ConfigureContainer ()
		{
			base.ConfigureContainer ();
			//string options = string.Empty;

			#region SQLite
			var options = new DbContextOptionsBuilder<AppDbContext> ().UseSqlite ("Data Source=AppData.db;").Options;
			#endregion SQLite

			Container.RegisterType<AppDbContext> (new TransientLifetimeManager (), new InjectionConstructor (options));

			Container.RegisterType<ObjectMapperProvider> (new TransientLifetimeManager ());
			Container.RegisterInstance (Container.Resolve<ObjectMapperProvider> ().Mapper);
			Container.RegisterType<IAppUser, AppUser> (new InjectionConstructor (1, "LoggedUser"));
			Container.RegisterType<ILogger> (new InjectionFactory ((c) => null));
			Container.RegisterType<IUserService, UserService> ();
		}
	}
}
