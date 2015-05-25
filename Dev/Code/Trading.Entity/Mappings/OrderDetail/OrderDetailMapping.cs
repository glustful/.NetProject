using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Trading.Entity.Model;

namespace Trading.Entity.Mappings.OrderDetail
{
	public class OrderDetailMapping : EntityTypeConfiguration<OrderDetailEntity>, Zerg.Common.Data.IZergMapping
	{
		public OrderDetailMapping()
		{
			ToTable("OrderDetail");
			HasKey(c => c.Id);
			HasOptional(c =>c.Product);
			Property(c => c.Productname).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Price).HasColumnType("decimal");
			Property(c => c.Commission).HasColumnType("decimal");
            Property(c => c.Dealcommission).HasColumnType("decimal");
			Property(c => c.Snapshoturl).HasColumnType("varchar").HasMaxLength(500);
			Property(c => c.Remark).HasColumnType("varchar").HasMaxLength(500);
			Property(c => c.Adduser).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.Adddate).HasColumnType("datetime");
			Property(c => c.Upduser).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.Upddate).HasColumnType("datetime");
		}
	}
}