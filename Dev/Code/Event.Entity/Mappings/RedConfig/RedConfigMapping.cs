using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Event.Entity.Model;

namespace Event.Entity.Mappings.RedConfig
{
	public class RedConfigMapping : EntityTypeConfiguration<RedConfigEntity>, IMapping
	{
		public RedConfigMapping()
		{
			ToTable("RedConfig");
			HasKey(c => c.Id);

			Property(c => c.商家关联id).HasColumnType("int");
			Property(c => c.Ttitle).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Intro).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Starttime).HasColumnType("datetime");
			Property(c => c.Endtime).HasColumnType("datetime");
			Property(c => c.Status).HasColumnType("enum");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}