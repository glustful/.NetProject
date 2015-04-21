using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.LevelConfig
{
	public class LevelConfigMapping : EntityTypeConfiguration<LevelConfigEntity>, IMapping
	{
		public LevelConfigMapping()
		{
			ToTable("LevelConfig");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int").IsOptional();
			Property(c => c.Name).HasColumnType("varchar").HasMaxLength(50).IsOptional();
			Property(c => c.Describe).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Value).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}