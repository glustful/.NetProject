using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Event.Entity.Model;
using Zerg.Common.Data;

namespace Event.Entity.Mappings.Phone
{
	public class PhoneMapping : EntityTypeConfiguration<PhoneEntity>,IZergMapping
	{
		public PhoneMapping()
		{
			ToTable("Phone");
			HasKey(c => c.Id);

			HasRequired(c =>c.Follower);
			Property(c => c.Openid).HasColumnType("varchar").HasMaxLength(30).IsOptional();
			Property(c => c.Phone).HasColumnType("varchar").HasMaxLength(11);
			Property(c => c.Adduser).HasColumnType("int");
            Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}