using System.Data.Entity.ModelConfiguration;
using Trading.Entity.Model;
using Zerg.Common.Data;

namespace Trading.Entity.Mappings.ProductBrand
{
	public class ProductBrandMapping : EntityTypeConfiguration<ProductBrandEntity>, IZergMapping
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
            Property(c => c.AdTitle).HasColumnType("varchar").HasMaxLength(500);
		    Property(c => c.Content).HasColumnType("varchar").IsMaxLength();
            Property(c => c.ClassId).HasColumnType("int").IsOptional();
		    HasOptional(c => c.Father);
            HasMany(c => c.ParameterEntities).WithOptional(c => c.ProductBrand);
		}
	}
}