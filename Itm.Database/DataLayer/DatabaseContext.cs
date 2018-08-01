using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Itm.Database.DataLayer
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext (string connectionString)
			: base (connectionString)
		{
			Configuration.LazyLoadingEnabled = false;

			System.Data.Entity.Database.SetInitializer<DatabaseContext> (new CreateDatabaseIfNotExists<DatabaseContext> ());
			//Database.SetInitializer<WriteDbContext> (new DropCreateDatabaseAlways<WriteDbContext> ());
			//Database.SetInitializer<DatabaseContext> (new CreateDatabaseIfNotExists<DatabaseContext> ());
		}

		public DatabaseContext () : base (@"Server=192.168.19.218;initial catalog=Iwsp.TCS_DB;Integrated Security=False;user id=sa;password=sata;MultipleActiveResultSets=True;")
		{
			Configuration.LazyLoadingEnabled = false;

			//Database.SetInitializer<WriteDbContext> (new DropCreateDatabaseAlways<WriteDbContext> ());
			System.Data.Entity.Database.SetInitializer<DatabaseContext> (new CreateDatabaseIfNotExists<DatabaseContext> ());
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
