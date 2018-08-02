using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Itm.Database.Core.EF.Entities;

namespace Itm.Database.Core.EF.Data.Configurations
{
	public class ChangeLogExclusionConfiguration : EntityTypeConfiguration<ChangeLogExclusion>
	{
		public ChangeLogExclusionConfiguration ()
		{
			HasKey (db => db.ID);

			Property (p => p.RowID).HasDatabaseGeneratedOption (DatabaseGeneratedOption.Identity);
		}
	}
}
