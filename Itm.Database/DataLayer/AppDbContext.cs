using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Itm.Database.Core.EF.Entities;
using Itm.Database.Core.Services;
using Itm.Database.EntityLayer;

namespace Itm.Database.DataLayer
{
	public class AppDbContext : DbContext
	{
		public AppDbContext (IDatabaseConnection connection)
			: base (connection.NameOrConnectionString)
		{
			Configuration.LazyLoadingEnabled = false;

			System.Data.Entity.Database.SetInitializer<AppDbContext> (new CreateDatabaseIfNotExists<AppDbContext> ());
		}

		public DbSet<User> Users { get; set; }
		public DbSet<ChangeLogExclusion> ChangeLogExclusions { get; set; }
		public DbSet<ChangeLog> ChangeLogs { get; set; }
		public DbSet<EventLog> EventLogs { get; set; }
		

		public AppDbContext () : base (@"Server=192.168.19.218;initial catalog=Iwsp.TCS_DB;Integrated Security=False;user id=sa;password=sata;MultipleActiveResultSets=True;")
		{
			Configuration.LazyLoadingEnabled = false;

			//Database.SetInitializer<WriteDbContext> (new DropCreateDatabaseAlways<WriteDbContext> ());
			System.Data.Entity.Database.SetInitializer<AppDbContext> (new CreateDatabaseIfNotExists<AppDbContext> ());
		}

		protected override void OnModelCreating (DbModelBuilder modelBuilder)
		{
			RemoveConventions (modelBuilder);
			AddAllEntityConfigurations (modelBuilder);

			base.OnModelCreating (modelBuilder);
		}


		private void AddAllEntityConfigurations (DbModelBuilder modelBuilder)
		{
			var configurationsToRegister = GetAllEntityConfigurationsToRegister ();

			RegisterEntityTypeConfigurations (modelBuilder, configurationsToRegister);
		}

		private static void RegisterEntityTypeConfigurations (DbModelBuilder modelBuilder, IEnumerable<System.Type> configurationsToRegister)
		{
			foreach (var type in configurationsToRegister) {
				dynamic configurationInstance = Activator.CreateInstance (type);
				modelBuilder.Configurations.Add (configurationInstance);
			}
		}

		private static IEnumerable<System.Type> GetAllEntityConfigurationsToRegister ()
		{
			var entityConfigurationsToRegister = Assembly.GetExecutingAssembly ().GetTypes ()
				.Where (type => !String.IsNullOrEmpty (type.Namespace))
				.Where (type => type.BaseType != null
								&& type.BaseType.IsGenericType
								&& type.BaseType.GetGenericTypeDefinition () == typeof (EntityTypeConfiguration<>));

			return entityConfigurationsToRegister;
		}

		private static void RemoveConventions (DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention> ();
		}
	}
}
