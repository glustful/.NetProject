using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CMS.Entity.Model;

namespace CMS.Entity.Mappings.Tag
{
	public class TagMapping : EntityTypeConfiguration<TagEntity>, IMapping
	{
		public TagMapping()
		{
			ToTable("Tag");
			HasKey(c => c.Id);
			Property(c => c.Tag).HasColumnType("varchar");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.UpdUser).HasColumnType("int").IsOptional();
			Property(c => c.UpdTime).HasColumnType("datetime").IsOptional();
			HasMany(c =>c.Content).WithMany(c=>c.Tags);
		}
	}
}