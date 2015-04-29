using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.BrokerBill
{
	public class BrokerBillMapping : EntityTypeConfiguration<BrokerBillEntity>, Zerg.Common.Data.IZergMapping
	{
		public BrokerBillMapping()
		{
			ToTable("BrokerBill");
			HasKey(c => c.Id);

			HasOptional(c =>c.Broker);
			Property(c => c.BillId).HasColumnType("int");
			Property(c => c.Type).HasColumnType("bit");
			Property(c => c.Billamount).HasColumnType("decimal");
			Property(c => c.Paidinamount).HasColumnType("decimal");
			Property(c => c.Cardnum).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.Merchantid).HasColumnType("int");
			Property(c => c.Merchantname).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Payeeuser).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.Payeenum).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.Paytime).HasColumnType("datetime");
			Property(c => c.Customername).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Note).HasColumnType("varchar").HasMaxLength(256).IsOptional();
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}