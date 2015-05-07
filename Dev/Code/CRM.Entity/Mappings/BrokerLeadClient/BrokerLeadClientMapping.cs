using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.BrokerLeadClient
{
	public class BrokerLeadClientMapping : EntityTypeConfiguration<BrokerLeadClientEntity>, IMapping
	{
		public BrokerLeadClientMapping()
		{
			ToTable("BrokerLeadClient");
			HasKey(c => c.Id);

			HasOptional(c =>c.Broker);
			HasOptional(c =>c.ClientInfo);
			Property(c => c.Appointmenttime).HasColumnType("datetime");
			Property(c => c.Appointmentstatus).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Details).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Adduser).HasColumnType("int").IsOptional();
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}