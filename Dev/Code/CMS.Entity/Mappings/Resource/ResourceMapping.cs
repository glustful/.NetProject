using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CMS.Entity.Model;

namespace CMS.Entity.Mappings.Resource
{
	public class ResourceMapping : EntityTypeConfiguration<ResourceEntity>, IMapping
	{
		public ResourceMapping()
		{
			ToTable("Resource");
			HasKey(c => c.Id);
			Property(c => c.Guid).HasColumnType("uniqueidentifier");
			Property(c => c.Name).HasColumnType("varchar");
			Property(c => c.Type).HasColumnType("varchar");
			Property(c => c.Length).HasColumnType("bigint");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.UpdUser).HasColumnType("int").IsOptional();
			Property(c => c.UpdTime).HasColumnType("datetime").IsOptional();

		    HasOptional(c => c.Content).WithMany(c => c.Resources);
		}
	}
}