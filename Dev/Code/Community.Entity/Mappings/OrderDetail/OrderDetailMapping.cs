using System.Data.Entity.ModelConfiguration;
using Community.Entity.Model.OrderDetail;
using Zerg.Common.Data;

namespace Community.Entity.Mappings.OrderDetail
{
	public class OrderDetailMapping : EntityTypeConfiguration<OrderDetailEntity>, IZergMapping
	{
		public OrderDetailMapping()
		{
			ToTable("OrderDetail");
			HasKey(c => c.Id);
			
			HasRequired(c =>c.Product);
			Property(c =>c.ProductName).HasColumnType("varchar").IsOptional();
			Property(c =>c.UnitPrice).HasColumnType("decimal").IsOptional();
			Property(c => c.Count).HasColumnType("decimal").IsOptional();
			Property(c => c.Snapshoturl).HasColumnType("varchar").HasMaxLength(500).IsOptional();
			Property(c => c.Remark).HasColumnType("varchar").HasMaxLength(500).IsOptional();
			Property(c => c.Adduser).HasColumnType("int").IsOptional();
			Property(c => c.Adddate).HasColumnType("datetime").IsOptional();
			Property(c => c.Upduser).HasColumnType("int").IsOptional();
			Property(c => c.Upddate).HasColumnType("datetime").IsOptional();
			Property(c => c.Totalprice).HasColumnType("decimal").IsOptional();
            Property(c => c.Status).HasColumnType("int").IsOptional();
			HasRequired(c =>c.Order);
		}
	}
}