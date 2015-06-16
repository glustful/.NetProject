using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Event.Entity.Model;
using Zerg.Common.Data;

namespace Event.Entity.Mappings.PartImage
{
	public class PartImageMapping : EntityTypeConfiguration<PartImageEntity>, IZergMapping
	{
		public PartImageMapping()
		{
			ToTable("PartImage");
			HasKey(c => c.Id);

			HasRequired(c =>c.Crowd).WithOptional();
			Property(c => c.Orderby).HasColumnType("int");
			Property(c => c.Imgurl).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}