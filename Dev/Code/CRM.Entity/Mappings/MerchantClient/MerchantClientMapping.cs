using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.MerchantClient
{
	public class MerchantClientMapping : EntityTypeConfiguration<MerchantClientEntity>, Zerg.Common.Data.IZergMapping
	{
		public MerchantClientMapping()
		{
			ToTable("MerchantClient");
			HasKey(c => c.Id);
			 
			HasOptional(c =>c.MerchantInfo);
			HasOptional(c =>c.ClientInfo);
			Property(c => c.Appointmenttime).HasColumnType("datetime");
			Property(c => c.Appointmentstatus).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Details).HasColumnType("varchar").HasMaxLength(256).IsOptional();
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}