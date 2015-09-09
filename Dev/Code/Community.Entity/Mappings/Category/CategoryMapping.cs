using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Community.Entity.Model;
using Community.Entity.Model.Category;
using Zerg.Common.Data;

namespace Community.Entity.Mappings.Category
{
	public class CategoryMapping : EntityTypeConfiguration<CategoryEntity>, IZergMapping
	{
		public CategoryMapping()
		{
			ToTable("Category");
			HasKey(c => c.Id);

			HasOptional(c =>c.Father);
			Property(c => c.Name).HasColumnType("varchar").HasMaxLength(200).IsOptional();
			Property(c => c.Sort).HasColumnType("int").IsOptional();
			Property(c => c.AddUser).HasColumnType("int").IsOptional();
			Property(c => c.AddTime).HasColumnType("datetime").IsOptional();
			Property(c => c.UpdUser).HasColumnType("int").IsOptional();
			Property(c => c.UpdTime).HasColumnType("datetime").IsOptional();
		}
	}
}