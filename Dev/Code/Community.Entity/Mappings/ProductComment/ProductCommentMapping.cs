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
			HasRequired(c=>c.Member);
			Property(c => c.AddTime).HasColumnType("datetime").IsOptional();
			Property(c => c.Content).HasColumnType("text").IsOptional();
			Property(c => c.Stars).HasColumnType("int").IsOptional();
		    Property(c => c.AddUser).HasColumnType("int").IsOptional();
		    HasOptional(c => c.OrderDetail).WithOptionalDependent();
		}
	}
}