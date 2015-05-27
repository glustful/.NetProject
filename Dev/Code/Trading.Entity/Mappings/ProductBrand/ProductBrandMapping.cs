using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Trading.Entity.Model;

namespace Trading.Entity.Mappings.ProductBrand
{
	public class ProductBrandMapping : EntityTypeConfiguration<ProductBrandEntity>, Zerg.Common.Data.IZergMapping
	{
		public ProductBrandMapping()
		{
			ToTable("ProductBrand");
			HasKey(c => c.Id);
			//Property(c => c.Id).HasColumnType("int").IsOptional();
			Property(c => c.Bname).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Bimg).HasColumnType("varchar").HasMaxLength(200);
			Property(c => c.Adduser).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upduser).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Updtime).HasColumnType("datetime");
		    Property(c => c.SubTitle).HasColumnType("varchar").HasMaxLength(900);
		    Property(c => c.Content).HasColumnType("varchar").HasMaxLength(900);
		    HasOptional(c => c.Father);
		}
	}
}