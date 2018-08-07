using Itm.Database.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Itm.Database.Context.Configurations
{
    public class ChangeLogExclusionConfiguration : EntityMappingConfiguration<ChangeLogExclusion>
	{
        public override void Map(EntityTypeBuilder<ChangeLogExclusion> builder)
        {
            builder.Property(db => db.ID).ValueGeneratedOnAdd();
            builder.HasKey(db => db.ID);
            builder.HasIndex(db => db.ID);
        }
    }
}
