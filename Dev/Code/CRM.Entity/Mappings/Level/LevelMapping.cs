using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.Level
{
	public class LevelMapping : EntityTypeConfiguration<LevelEntity>, IMapping
	{
		public LevelMapping()
		{
			ToTable("Level");
			HasKey(c => c.Id);

			Property(c => c.CodeId).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Describe).HasColumnType("varchar").HasMaxLength(256).IsOptional();
			Property(c => c.Url).HasColumnType("varchar").HasMaxLength(256).IsOptional();
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}