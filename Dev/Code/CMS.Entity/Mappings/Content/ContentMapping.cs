using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CMS.Entity.Model;

namespace CMS.Entity.Mappings.Content
{
	public class ContentMapping : EntityTypeConfiguration<ContentEntity>, IMapping
	{
		public ContentMapping()
		{
			ToTable("Content");
			HasKey(c => c.Id);
			Property(c => c.Content).HasColumnType("varchar").IsOptional();
			Property(c => c.Title).HasColumnType("varchar");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.UpdUser).HasColumnType("int").IsOptional();
			Property(c => c.UpdTime).HasColumnType("datetime").IsOptional();
			Property(c => c.Status).HasColumnType("int");
			Property(c => c.Praise).HasColumnType("int");
			Property(c => c.Unpraise).HasColumnType("int");
			Property(c => c.Viewcount).HasColumnType("int");
			HasMany(c =>c.Resources).WithOptional(r=>r.Content);
            HasMany(c => c.Tags).WithMany(t=>t.Content);
            HasRequired(c => c.Channel).WithMany(c=>c.Contents);
		}
	}
}