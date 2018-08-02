using System.Data.Entity.ModelConfiguration;
using Itm.Database.EntityLayer;

namespace Itm.Database.DataLayer.Configurations
{
	public class UserConfiguration : EntityTypeConfiguration<User>
	{
		public UserConfiguration ()
		{
			HasKey (db => db.ID);
		}
	}
}
