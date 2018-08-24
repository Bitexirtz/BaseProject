using System.Windows;
using System.Windows.Controls;
using AutoMapper;
using Itm.Database.Context;
using Itm.Database.Core.Entities;
using Itm.Database.Services;
using Itm.Log;
using Itm.Log.Core;
using Itm.Module.UserManagement;
using Itm.ObjectMap;
using Itm.RegionAdapters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace Itm.Startup
{
	public class Bootstrapper : UnityBootstrapper
	{
		#region Fields
		#endregion Fields

		#region Constructors
		public Bootstrapper()
		{
		}
		#endregion Constructors

		protected override DependencyObject CreateShell()
		{
			return Container.TryResolve<MainWindow>();
		}

		protected override void InitializeShell()
		{
			Application.Current.MainWindow.Show();
		}

		protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
		{
			RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();

			mappings.RegisterMapping(typeof(StackPanel), Container.TryResolve<StackPanelRegionAdapter>());

			return mappings;
		}

		protected override void ConfigureModuleCatalog()
		{
			var catalog = (ModuleCatalog)ModuleCatalog;

			catalog.AddModule(typeof(UserManagementModule));
		}

		protected override void ConfigureContainer()
		{
			base.ConfigureContainer();
			//string options = string.Empty;

			#region SQLite
			var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite("Data Source=AppData.db3;").Options;
			#endregion SQLite

			Container.RegisterType<AppDbContext>(new InjectionConstructor(options));
			Container.RegisterType<ILogger, Logger>(new InjectionConstructor());
			Container.RegisterType<IMapper, ObjectMapper> ();
			Container.RegisterType<IAppUser, AppUser>(new InjectionConstructor(1, "LoggedUser"));
			Container.RegisterType<IUserService, UserService>();
		}
	}
}
