using System.Data.Entity.ModelConfiguration;
using Community.Entity.Model.ProductComment;
using Zerg.Common.Data;

namespace Community.Entity.Mappings.ProductComment
{
	public class ProductCommentMapping : EntityTypeConfiguration<ProductCommentEntity>, IZergMapping
	{
		public ProductCommentMapping()
		{
			ToTable("ProductComment");
			HasKey(c => c.Id);
			
			HasRequired(c =>c.Product);
			Property(c => c.AddUser).HasColumnType("int").IsOptional();
			Property(c => c.AddTime).HasColumnType("datetime").IsOptional();
			Property(c => c.Content).HasColumnType("text").IsOptional();
			Property(c => c.Stars).HasColumnType("int").IsOptional();
		}
	}
}