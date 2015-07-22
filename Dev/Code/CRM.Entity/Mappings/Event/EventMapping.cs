using System;
using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;
//by yangyue 2015/7/21  ----活动表------------//
namespace CRM.Entity.Mappings.BrokeAccount
{
    public class EventMapping : EntityTypeConfiguration<EventEntity>, Zerg.Common.Data.IZergMapping
    {
        public EventMapping()
        {
            ToTable("Event");
            HasKey(c => c.Id);
            Property(c => c.EventContent).HasColumnType("varchar").HasMaxLength(256);
            Property(c => c.ActionControllers).HasColumnType("varchar").HasMaxLength(256);
            Property(c => c.Starttime).HasColumnType("datetime");
            Property(c => c.Endtime).HasColumnType("datetime");
            Property(c => c.State).HasColumnType("bit");


        }

    }
}
