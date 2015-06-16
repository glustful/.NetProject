using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Event.Entity.Model;

namespace Event.Entity.Mappings.Phone
{
	public class PhoneMapping : EntityTypeConfiguration<PhoneEntity>, IMapping
	{
		public PhoneMapping()
		{
			ToTable("Phone");
			HasKey(c => c.Id);

			HasOptional(c =>c.Follower);
			Property(c => c.Openid).HasColumnType("varchar").HasMaxLength(30).IsOptional();
			Property(c => c.Phone).HasColumnType("varchar").HasMaxLength(11);
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("DateTime");
		}
	}
}