using Itm.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itm.Database.Context.Configurations
{
	public class UserConfiguration : EntityMappingConfiguration<User>
	{
		public override void Map (EntityTypeBuilder<User> builder)
		{
			builder.Property (t => t.ID).ValueGeneratedOnAdd ();
			builder.HasKey (t => t.ID);
			builder.HasIndex (t => t.ID);

			// Set concurrency token for entity
			builder.Property(t => t.Timestamp)
				.ValueGeneratedOnAddOrUpdate()
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.IsRowVersion();


			builder.HasOne (t => t.UserCredential).WithMany()
					.HasForeignKey(t => t.UserCredentialID);


			// One To One Relation
			//builder.HasOne(t => t.UserCredential)
			//		.WithOne(t => t.User)
			//		.HasForeignKey<UserCredential>(t => t.UserID);
			//TODO: How to configure bidirectional navigation?
			//.HasForeignKey<User>(t=>t.UserCredentiallID);
		}
	}
}
