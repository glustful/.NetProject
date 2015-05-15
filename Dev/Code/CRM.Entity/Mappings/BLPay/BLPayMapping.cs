using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.BLPay
{
    public class BLPayMapping : EntityTypeConfiguration<BLPayEntity>, Zerg.Common.Data.IZergMapping
	{
		public BLPayMapping()
		{
			ToTable("BLPay");
			HasKey(c => c.Id);

			HasOptional(c =>c.BrokerLeadClient);
			Property(c => c.Name).HasColumnType("varchar").HasMaxLength(256).IsOptional();
			Property(c => c.Statusname).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Describe).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Amount).HasColumnType("decimal");
			Property(c => c.Accountantid).HasColumnType("int");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}