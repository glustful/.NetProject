using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Trading.Entity.Model;

namespace Trading.Entity.Mappings.Product
{
	public class ProductMapping : EntityTypeConfiguration<ProductEntity>, Zerg.Common.Data.IZergMapping
	{
		public ProductMapping()
		{
			ToTable("Product");
			HasKey(c => c.Id);
			HasOptional(c =>c.ProductDetail);
			HasOptional(c =>c.Classify);
			HasOptional(c =>c.ProductBrand);
			Property(c => c.Bussnessid).HasColumnType("int");
            Property(c => c.BussnessName).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Dealcommission).HasColumnType("decimal");
            Property(c => c.Commission).HasColumnType("decimal");
			Property(c => c.Price).HasColumnType("decimal");
			Property(c => c.Productname).HasColumnType("varchar").HasMaxLength(200);
			Property(c => c.Status).HasColumnType("bit");
			Property(c => c.Productimg).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Recommend).HasColumnType("bit");
			Property(c => c.Sort).HasColumnType("int");
			Property(c => c.Stockrule).HasColumnType("int");
			Property(c => c.Adduser).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upduser).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Updtime).HasColumnType("datetime");
		}
	}
}