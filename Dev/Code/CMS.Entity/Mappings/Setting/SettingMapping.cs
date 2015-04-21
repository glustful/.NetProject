using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CMS.Entity.Model;

namespace CMS.Entity.Mappings.Setting
{
	public class SettingMapping : EntityTypeConfiguration<SettingEntity>, IMapping
	{
		public SettingMapping()
		{
			ToTable("Setting");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int");
			Property(c => c.Key).HasColumnType("varchar");
			Property(c => c.Value).HasColumnType("varchar");
		}
	}
}