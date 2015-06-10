using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.BrokerRECClient
{
    public class BrokerRECClientMapping : EntityTypeConfiguration<BrokerRECClientEntity>, Zerg.Common.Data.IZergMapping
	{
		public BrokerRECClientMapping()
		{
			ToTable("BrokerRECClient");
			HasKey(c => c.Id);
			HasOptional(c =>c.Broker);
			HasOptional(c =>c.ClientInfo);
			Property(c => c.Clientname).HasColumnType("varchar").HasMaxLength(50);
            Property(c => c.Phone).HasColumnType("varchar").HasMaxLength(50);
            Property(c => c.Qq).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
			Property(c => c.Brokername).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Brokerlevel).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Projectname).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Projectid).HasColumnType("int");
			Property(c => c.Status).HasColumnType("int");
			HasOptional(c =>c.SecretaryId);
            Property(c => c.SecretaryPhone).HasColumnType("varchar").HasMaxLength(50);
			HasOptional(c =>c.WriterId);
            Property(c => c.WriterPhone).HasColumnType("varchar").HasMaxLength(50);
            Property(c => c.RecOrder).HasColumnType("int");
            Property(c => c.DealOrder).HasColumnType("int");
		}
	}
}