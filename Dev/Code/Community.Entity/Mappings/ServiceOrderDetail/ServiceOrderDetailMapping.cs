using System.Data.Entity.ModelConfiguration;
using Community.Entity.Model.ServiceOrderDetail;
using Zerg.Common.Data;

namespace Community.Entity.Mappings.ServiceOrderDetail
{
	public class ServiceOrderDetailMapping : EntityTypeConfiguration<ServiceOrderDetailEntity>, IZergMapping
	{
		public ServiceOrderDetailMapping()
		{
			ToTable("ServiceOrderDetail");
			HasKey(c => c.Id);
			
			HasRequired(c =>c.ServiceOrder);
			HasRequired(c =>c.Product);
			Property(c => c.Count).HasColumnType("decimal").IsRequired();
			Property(c => c.Price).HasColumnType("decimal").IsRequired();
		}
	}
}