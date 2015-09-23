using System.Data.Entity.ModelConfiguration;
using Community.Entity.Model.Product;
using Zerg.Common.Data;

namespace Community.Entity.Mappings.Product
{
	public class ProductMapping : EntityTypeConfiguration<ProductEntity>, IZergMapping
	{
		public ProductMapping()
		{
            ToTable("Product");
			HasKey(c => c.Id);
			
			HasRequired(c =>c.Category);
			Property(c =>c.BussnessId).HasColumnType("int").IsOptional();
			Property(c =>c.BussnessName).HasColumnType("varchar").HasMaxLength(100).IsOptional();
			Property(c => c.Price).HasColumnType("decimal").IsOptional();
			Property(c => c.Name).HasColumnType("varchar").HasMaxLength(200).IsOptional();
			Property(c => c.Status).HasColumnType("int").IsRequired();
			Property(c => c.MainImg).HasColumnType("varchar").HasMaxLength(256).IsOptional();
			Property(c => c.IsRecommend).HasColumnType("int").IsRequired();
			Property(c => c.Sort).HasColumnType("int").IsOptional();
			Property(c => c.Stock).HasColumnType("int").IsOptional();
			Property(c =>c.AddUser).HasColumnType("int").IsRequired();
			Property(c =>c.AddTime).HasColumnType("datetime").IsRequired();
			Property(c => c.UpdUser).HasColumnType("int").IsOptional();
			Property(c => c.UpdTime).HasColumnType("datetime").IsOptional();
			Property(c => c.Subtitte).HasColumnType("varchar").HasMaxLength(200).IsOptional();
			Property(c => c.Contactphone).HasColumnType("varchar").HasMaxLength(50).IsOptional();
			Property(c => c.Type).HasColumnType("int").IsOptional();
			HasRequired(c =>c.Detail).WithRequiredPrincipal();
			HasMany(c =>c.Comments).WithRequired(c=>c.Product);
			HasMany(c =>c.Parameters).WithRequired(c=>c.Product);
		    Property(c => c.OldPrice).HasColumnType("decimal").IsOptional();
		    Property(c => c.Owner).HasColumnType("int").IsOptional();
		}
	}
}