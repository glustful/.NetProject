using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.BRECPay
{
    public class BRECPayMapping : EntityTypeConfiguration<BRECPayEntity>, Zerg.Common.Data.IZergMapping
	{
		public BRECPayMapping()
		{
			ToTable("BRECPay");
			HasKey(c => c.Id);

			HasOptional(c =>c.BrokerRECClient);
			Property(c => c.Name).HasColumnType("varchar").HasMaxLength(256).IsOptional();
            Property(c => c.Statusname).HasColumnType("int");
			Property(c => c.Describe).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Amount).HasColumnType("decimal");
			Property(c => c.Accountantid).HasColumnType("int");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
            Property(c => c.BankCard).HasColumnType("int");
		}
	}
}