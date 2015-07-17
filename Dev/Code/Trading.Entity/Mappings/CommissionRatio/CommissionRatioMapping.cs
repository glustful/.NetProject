using System.Data.Entity.ModelConfiguration;
using Trading.Entity.Entity.CommissionRatio;
using Zerg.Common.Data;

namespace Trading.Entity.Mappings.CommissionRatio
{
    public class CommissionRatioMapping : EntityTypeConfiguration<CommissionRatioEntity>,IZergMapping
    {
        public CommissionRatioMapping()
        {
            ToTable("CommissionRatio");
            HasKey(c=>c.Id);
            Property(c => c.RecAgentScale).HasColumnType("decimal");
            Property(c => c.RecCfbScale).HasColumnType("decimal");
            Property(c => c.TakeAgentScale).HasColumnType("decimal");
            Property(c => c.TakeCfbScale).HasColumnType("decimal");
            Property(c => c.RecPartnerScale).HasColumnType("decimal");
            Property(c => c.TakePartnerScale).HasColumnType("decimal");
        }
    }
}
