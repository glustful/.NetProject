using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Event.Entity.Model;

namespace Event.Entity.Mappings.Crowd
{
	public class CrowdMapping : EntityTypeConfiguration<CrowdEntity>, IMapping
	{
		public CrowdMapping()
		{
			ToTable("Crowd");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int").IsOptional();
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