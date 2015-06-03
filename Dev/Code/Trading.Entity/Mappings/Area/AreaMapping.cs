using System.Data.Entity.ModelConfiguration;
using Trading.Entity.Entity.Area;
using Zerg.Common.Data;

namespace Trading.Entity.Mappings.Area
{
    public class AreaMapping : EntityTypeConfiguration<AreaEntity>, IZergMapping
    {
        public AreaMapping()
        {
            ToTable("Area");
            HasKey(c => c.Id);
            Property(c => c.AreaName).HasColumnType("varchar").HasMaxLength(50);
            Property(c => c.ParentId).HasColumnType("int");
            Property(c => c.Level).HasColumnType("int");
        }
    }
}
