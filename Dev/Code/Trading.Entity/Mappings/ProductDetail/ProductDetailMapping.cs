using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Trading.Entity.Model;

namespace Trading.Entity.Mappings.ProductDetail
{
	public class ProductDetailMapping : EntityTypeConfiguration<ProductDetailEntity>, Zerg.Common.Data.IZergMapping
	{
		public ProductDetailMapping()
		{
			ToTable("ProductDetail");
			HasKey(c => c.Id);
			//Property(c => c.Id).HasColumnType("int").IsOptional();
			Property(c => c.Productname).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Productdetail).HasColumnType("varchar").HasMaxLength(5000);
			Property(c => c.Productimg).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Productimg1).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Productimg2).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Productimg3).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Productimg4).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Sericeinstruction).HasColumnType("varchar").HasMaxLength(500);
			Property(c => c.Adduser).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upduser).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Updtime).HasColumnType("datetime");
		    Property(c => c.Ad1).HasColumnType("varchar").HasMaxLength(200);
            Property(c => c.Ad2).HasColumnType("varchar").HasMaxLength(200);
            Property(c => c.Ad3).HasColumnType("varchar").HasMaxLength(200);
		}
	}
}