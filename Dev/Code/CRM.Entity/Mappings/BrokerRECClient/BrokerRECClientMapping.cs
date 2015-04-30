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
			Property(c => c.Phone).HasColumnType("int");
			Property(c => c.Qq).HasColumnType("int");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}