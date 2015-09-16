using System.Data.Entity.ModelConfiguration;
using Community.Entity.Model.Order;
using Zerg.Common.Data;

namespace Community.Entity.Mappings.Order
{
	public class OrderMapping : EntityTypeConfiguration<OrderEntity>, IZergMapping
	{
		public OrderMapping()
		{
			ToTable("Order");
			HasKey(c => c.Id);
			
			Property(c => c.No).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.Status).HasColumnType("int").IsOptional();
			Property(c => c.CustomerName).HasColumnType("varchar").HasMaxLength(50).IsOptional();
			Property(c => c.Remark).HasColumnType("varchar").HasMaxLength(500).IsOptional();
			Property(c => c.AddDate).HasColumnType("datetime").IsOptional();
			Property(c =>c.AddUser).HasColumnType("int").IsRequired();
			Property(c =>c.UpdUser).HasColumnType("int").IsOptional();
			Property(c => c.UpdDate).HasColumnType("datetime").IsOptional();
			Property(c => c.Totalprice).HasColumnType("decimal").IsOptional();
			Property(c => c.Actualprice).HasColumnType("decimal").IsOptional();
			HasMany(c =>c.Details).WithRequired(c=>c.Order);
		    HasRequired(c => c.AddMember);
		    HasRequired(c => c.Address).WithMany().WillCascadeOnDelete(false);
		}
	}
}