using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.BrokeAccount
{
    public class EventOrderMapping : EntityTypeConfiguration<EventOrderEntity>, Zerg.Common.Data.IZergMapping
    {
        public EventOrderMapping()
        {
            ToTable("EventOrder");
            HasKey(c => c.Id);

            HasOptional(c => c.Broker);
            HasOptional(c => c.Event);
            Property(c => c.AcDetail).HasColumnType("varchar").HasMaxLength(256);
            Property(c => c.MoneyCount).HasColumnType("decimal");
            Property(c => c.Addtime).HasColumnType("datetime");
        }
    }
}
