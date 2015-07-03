using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Event.Entity.Model;
using Zerg.Common.Data;

namespace Event.Entity.Mappings.Follower
{
	public class FollowerMapping : EntityTypeConfiguration<FollowerEntity>,IZergMapping
	{
		public FollowerMapping()
		{
			ToTable("Follower");
			HasKey(c => c.Id);

			Property(c => c.Openid).HasColumnType("varchar").HasMaxLength(30).IsOptional();
			Property(c => c.Nickname).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Sex).HasColumnType("varchar").HasMaxLength(2);
			Property(c => c.City).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Country).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Private).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Language).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Headimgurl).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Subscribetime).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Unioid).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Remark).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Groupid).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}