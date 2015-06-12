using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.BrokerLeadClient
{
	public class BrokerLeadClientMapping : EntityTypeConfiguration<BrokerLeadClientEntity>, Zerg.Common.Data.IZergMapping
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
            Property(c => c.Brokername).HasColumnType("varchar").HasMaxLength(50);
            Property(c => c.BrokerLevel).HasColumnType("varchar").HasMaxLength(50);
            Property(c => c.Projectname).HasColumnType("varchar").HasMaxLength(256);
            Property(c => c.ProjectId).HasColumnType("int");
            Property(c => c.Status).HasColumnType("int");
            HasOptional(c => c.SecretaryId);
            Property(c => c.SecretaryPhone).HasColumnType("varchar").HasMaxLength(50);
            HasOptional(c => c.WriterId);
            Property(c => c.WriterPhone).HasColumnType("varchar").HasMaxLength(50);
            Property(c => c.ClientName).HasColumnType("varchar").HasMaxLength(50);
            Property(c => c.Phone).HasColumnType("varchar").HasMaxLength(50);
            Property(c => c.ComOrder).HasColumnType("int");
            Property(c => c.DealOrder).HasColumnType("int");
            Property(c => c.DelFlag).HasColumnType("int");
		}
	}
}