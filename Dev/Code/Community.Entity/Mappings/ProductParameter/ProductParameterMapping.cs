using System.Data.Entity.ModelConfiguration;
using Community.Entity.Model.ProductParameter;
using Zerg.Common.Data;

namespace Community.Entity.Mappings.ProductParameter
{
	public class ProductParameterMapping : EntityTypeConfiguration<ProductParameterEntity>, IZergMapping
	{
		public ProductParameterMapping()
		{
			ToTable("ProductParameter");
			HasKey(c => c.Id);
			
			HasRequired(c =>c.ParameterValue);
			HasRequired(c =>c.Parameter);
			HasRequired(c =>c.Product);
			Property(c => c.Sort).HasColumnType("int").IsOptional();
			Property(c => c.AddUser).HasColumnType("int").IsOptional();
			Property(c => c.AddTime).HasColumnType("datetime").IsOptional();
			Property(c => c.UpdUser).HasColumnType("int").IsOptional();
			Property(c => c.UpdTime).HasColumnType("datetime").IsOptional();
		}
	}
}