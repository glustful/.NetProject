using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.Broker
{
	public class BrokerMapping : EntityTypeConfiguration<BrokerEntity>, Zerg.Common.Data.IZergMapping
	{
		public BrokerMapping()
		{
			ToTable("Broker");
			HasKey(c => c.Id);

			HasOptional(c =>c.Level);
			Property(c => c.UserId).HasColumnType("int");
			Property(c => c.Brokername).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Nickname).HasColumnType("varchar").HasMaxLength(50).IsOptional();
			Property(c => c.Realname).HasColumnType("");
			Property(c => c.Sfz).HasColumnType("varchar").HasMaxLength(18);
            Property(c => c.SfzPhoto).HasColumnType("varchar").HasMaxLength(256).IsOptional();
			Property(c => c.Sexy).HasColumnType("varchar").HasMaxLength(10).IsOptional();
			Property(c => c.Phone).HasColumnType("int");
			Property(c => c.Qq).HasColumnType("int").IsOptional();
			Property(c => c.Zip).HasColumnType("int").IsOptional();
			Property(c => c.Headphoto).HasColumnType("varchar").HasMaxLength(256).IsOptional();
			Property(c => c.Totalpoints).HasColumnType("int");
            Property(c => c.Amount).HasColumnType("decimal");
			Property(c => c.Agentlevel).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Usertype).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Address).HasColumnType("varchar").HasMaxLength(256).IsOptional();
			Property(c => c.Regtime).HasColumnType("datetime");
			Property(c => c.State).HasColumnType("int");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}