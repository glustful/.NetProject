using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Event.Entity.Model;
using Zerg.Common.Data;

namespace Event.Entity.Mappings.Crowd
{
	public class CrowdMapping : EntityTypeConfiguration<CrowdEntity>,IZergMapping
	{
		public CrowdMapping()
		{
			ToTable("Crowd");
			HasKey(c => c.Id);

			Property(c => c.Ttitle).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Intro).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Starttime).HasColumnType("datetime");
			Property(c => c.Endtime).HasColumnType("datetime");
			Property(c => c.Status).HasColumnType("int");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
            Property(c => c.crowdUrl).HasColumnType("varchar").HasMaxLength(100);
		}
	}
}