using System.Data.Entity.ModelConfiguration;
using Community.Entity.Model.ProductDetail;
using Zerg.Common.Data;

namespace Community.Entity.Mappings.ProductDetail
{
	public class ProductDetailMapping : EntityTypeConfiguration<ProductDetailEntity>, IZergMapping
	{
		public ProductDetailMapping()
		{
			ToTable("ProductDetail");
			HasKey(c => c.Id);
			
			Property(c => c.Name).HasColumnType("varchar").HasMaxLength(256).IsOptional();
			Property(c => c.Detail).HasColumnType("text").IsOptional();
			Property(c => c.Img).HasColumnType("varchar").HasMaxLength(256).IsOptional();
            Property(c => c.Img1).HasColumnType("varchar").HasMaxLength(255).IsOptional();
            Property(c => c.Img2).HasColumnType("varchar").HasMaxLength(255).IsOptional();
            Property(c => c.Img3).HasColumnType("varchar").HasMaxLength(255).IsOptional();
            Property(c => c.Img4).HasColumnType("varchar").HasMaxLength(255).IsOptional();
			Property(c => c.SericeInstruction).HasColumnType("text").IsOptional();
			Property(c => c.AddUser).HasColumnType("int").IsOptional();
			Property(c => c.AddTime).HasColumnType("datetime").IsOptional();
			Property(c => c.UpdUser).HasColumnType("int").IsOptional();
			Property(c => c.UpdTime).HasColumnType("datetime").IsOptional();
			Property(c => c.Ad1).HasColumnType("varchar").HasMaxLength(200).IsOptional();
			Property(c => c.Ad2).HasColumnType("varchar").HasMaxLength(200).IsOptional();
			Property(c => c.Ad3).HasColumnType("varchar").HasMaxLength(200).IsOptional();
		}
	}
}