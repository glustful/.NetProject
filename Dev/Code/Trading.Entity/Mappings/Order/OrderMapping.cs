using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Trading.Entity.Model;

namespace Trading.Entity.Mappings.Order
{
	public class OrderMapping : EntityTypeConfiguration<OrderEntity>, Zerg.Common.Data.IZergMapping
	{
		public OrderMapping()
		{
			ToTable("Order");
			HasKey(c => c.Id);
			HasOptional(c =>c.OrderDetail);
			Property(c => c.Ordercode).HasColumnType("varchar").HasMaxLength(30).IsOptional();
			Property(c => c.Ordertype).HasColumnType("int");
			Property(c => c.Shipstatus).HasColumnType("int");
			Property(c => c.Status).HasColumnType("int");
			Property(c => c.BusId).HasColumnType("int").IsOptional();
			Property(c => c.Busname).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.AgentId).HasColumnType("int");
			Property(c => c.Agentname).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Agenttel).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Customname).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Remark).HasColumnType("varchar").HasMaxLength(500);
			Property(c => c.Adddate).HasColumnType("datetime");
			Property(c => c.Adduser).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.Upduser).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.Upddate).HasColumnType("datetime");
		}
	}
}