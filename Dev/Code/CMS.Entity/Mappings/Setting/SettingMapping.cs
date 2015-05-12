using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CMS.Entity.Model;
using Zerg.Common.Data;

namespace CMS.Entity.Mappings.Setting
{
	public class SettingMapping : EntityTypeConfiguration<SettingEntity>, IZergMapping
	{
		public SettingMapping()
		{
			ToTable("Setting");
			HasKey(c => c.Id);
			Property(c => c.Key).HasColumnType("varchar");
			Property(c => c.Value).HasColumnType("varchar");
		}
	}
}