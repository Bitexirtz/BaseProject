using Itm.Database.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itm.Database.Context.Configurations
{
	public class UserCredentialConfiguration : EntityMappingConfiguration<UserCredential>
	{
		public override void Map (EntityTypeBuilder<UserCredential> builder)
		{
			builder.Property (t => t.ID).ValueGeneratedOnAdd ();
			builder.HasKey (t => t.ID);
			builder.HasIndex (t => t.ID);
		}
	}
}
