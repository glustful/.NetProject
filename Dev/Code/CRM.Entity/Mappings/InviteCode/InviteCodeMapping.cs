using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.BrokeAccount
{
    public class InviteCodeMapping : EntityTypeConfiguration<InviteCodeEntity>, Zerg.Common.Data.IZergMapping
    {
        public InviteCodeMapping()
        {
            ToTable("InviteCode");
            HasKey(c => c.Id);
            HasOptional(c => c.Broker);


            Property(c => c.CreatTime).HasColumnType("datetime");
            Property(c => c.UseTime).HasColumnType("datetime").IsOptional();
            Property(c => c.NumUser).HasColumnType("int").IsOptional();
            Property(c => c.Number).HasColumnType("varchar").HasMaxLength(256);
            Property(c => c.State).HasColumnType("bit");

        }

    }
}
