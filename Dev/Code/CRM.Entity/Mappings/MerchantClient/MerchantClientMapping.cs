using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.MerchantClient
{
	public class MerchantClientMapping : EntityTypeConfiguration<MerchantClientEntity>, IMapping
	{
		public MerchantClientMapping()
		{
			ToTable("MerchantClient");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int").IsOptional();
			HasOptional(c =>c.MerchantInfo);
			HasOptional(c =>c.ClientInfo);
			Property(c => c.Appointmenttime).HasColumnType("datetime");
			Property(c => c.Appointmentstatus).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Details).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}