using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CMS.Entity.Model;

namespace CMS.Entity.Mappings.PublishProduct
{
	public class PublishProductMapping : EntityTypeConfiguration<PublishProductEntity>, IMapping
	{
		public PublishProductMapping()
		{
			ToTable("PublishProduct");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int");
			Property(c => c.ProductId).HasColumnType("int");
			Property(c => c.ProductName).HasColumnType("varchar");
			Property(c => c.Detail).HasColumnType("varchar");
			Property(c => c.Publishtime).HasColumnType("datetime");
			Property(c => c.PublishUser).HasColumnType("int");
			HasMany(c =>c.Tags);
		}
	}
}