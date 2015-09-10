using System.Data.Entity.ModelConfiguration;
using Community.Entity.Model.ServiceOrder;
using Zerg.Common.Data;

namespace Community.Entity.Mappings.ServiceOrder
{
	public class ServiceOrderMapping : EntityTypeConfiguration<ServiceOrderEntity>, IZergMapping
	{
		public ServiceOrderMapping()
		{
			ToTable("ServiceOrder");
			HasKey(c => c.Id);
			
			Property(c => c.OrderNo).HasColumnType("varchar").HasMaxLength(30).IsOptional();
			Property(c =>c.AddTime).HasColumnType("datetime").IsRequired();
			Property(c => c.AddUser).HasColumnType("int").IsRequired();
			Property(c => c.Flee).HasColumnType("decimal").IsOptional();
			Property(c => c.Address).HasColumnType("varchar").HasMaxLength(200).IsOptional();
			Property(c =>c.Servicetime).HasColumnType("datetime").IsOptional();
			Property(c => c.Remark).HasColumnType("text").IsOptional();
		    Property(c => c.Status).HasColumnType("int").IsRequired();
		    Property(c => c.UpdUser).HasColumnType("int").IsOptional();
		    Property(c => c.UpdTime).HasColumnType("datetime").IsOptional();
		    HasMany(c => c.Details).WithRequired(c => c.ServiceOrder);
		}
	}
}