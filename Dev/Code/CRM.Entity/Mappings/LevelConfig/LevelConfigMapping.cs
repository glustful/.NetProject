using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.LevelConfig
{
	public class LevelConfigMapping : EntityTypeConfiguration<LevelConfigEntity>, Zerg.Common.Data.IZergMapping
	{
		public LevelConfigMapping()
		{
			ToTable("LevelConfig");
			HasKey(c => c.Id);
			 
			Property(c => c.Name).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Describe).HasColumnType("varchar").HasMaxLength(256).IsOptional();
			Property(c => c.Value).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}