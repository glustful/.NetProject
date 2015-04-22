using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.BrokerOrder
{
	public class BrokerOrderMapping : EntityTypeConfiguration<BrokerOrderEntity>, IMapping
	{
		public BrokerOrderMapping()
		{
			ToTable("BrokerOrder");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int");
			HasOptional(c =>c.Broker);
			Property(c => c.OrderId).HasColumnType("int");
			Property(c => c.Merchantname).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Status).HasColumnType("bit");
			Property(c => c.Note).HasColumnType("varchar").HasMaxLength(500).IsOptional();
			Property(c => c.Ordertime).HasColumnType("datetime");
			Property(c => c.Orderuser).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.Modifyuser).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.Modifytime).HasColumnType("datetime");
			Property(c => c.Customername).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}