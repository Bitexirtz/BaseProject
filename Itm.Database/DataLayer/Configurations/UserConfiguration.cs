using Itm.Database.Core.EF.Data.Configurations;
using Itm.Database.EntityLayer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itm.Database.DataLayer.Configurations
{
    public class UserConfiguration : EntityMappingConfiguration<User>
	{
        public override void Map(EntityTypeBuilder<User> builder)
        {
            builder.Property(db => db.ID).ValueGeneratedOnAdd();
            builder.HasKey(db => db.ID);
            builder.HasIndex(db => db.ID);

            // Set concurrency token for entity
            builder.Property(db => db.Timestamp)
                .ValueGeneratedOnAddOrUpdate()
                .IsRowVersion();
        }
    }
}
