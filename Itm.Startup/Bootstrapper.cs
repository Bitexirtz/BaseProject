using System.Windows;
using System.Windows.Controls;
using Itm.RegionAdapters;
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
		}
	}
}
