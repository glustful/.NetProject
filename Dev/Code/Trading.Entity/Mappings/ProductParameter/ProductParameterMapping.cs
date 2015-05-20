using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Trading.Entity.Model;

namespace Trading.Entity.Mappings.ProductParameter
{
	public class ProductParameterMapping : EntityTypeConfiguration<ProductParameterEntity>, Zerg.Common.Data.IZergMapping
	{
		public ProductParameterMapping()
		{
			ToTable("ProductParameter");
			HasKey(c => c.Id);
			//Property(c => c.Id).HasColumnType("int").IsOptional();
			HasOptional(c =>c.ParameterValue);
			HasOptional(c =>c.Parameter);
			HasOptional(c =>c.Product);
			Property(c => c.Sort).HasColumnType("int");
			Property(c => c.Adduser).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upduser).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Updtime).HasColumnType("datetime");
		}
	}
}