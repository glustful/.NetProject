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
			Property(c => c.Id).HasColumnType("int").IsOptional();
			HasOptional(c =>c.Broker);
			HasOptional(c =>c.ClientInfo);
			Property(c => c.Clientname).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Phone).HasColumnType("int");
			Property(c => c.Qq).HasColumnType("int");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
			Property(c => c.Brokername).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Brokerlevel).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Projectname).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Projectid).HasColumnType("int");
			Property(c => c.Status).HasColumnType("int");
			HasOptional(c =>c.SecretaryID);
			HasOptional(c =>c.SecretaryPhone);
			HasOptional(c =>c.WriterID);
			HasOptional(c =>c.WriterPhone);
			HasOptional(c =>c.AccountantID);
			Property(c => c.Payment).HasColumnType("Bool");
			Property(c => c.Amount).HasColumnType("Decimal");
		}
	}
}