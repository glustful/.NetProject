using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.MerchantBill
{
	public class MerchantBillMapping : EntityTypeConfiguration<MerchantBillEntity>, IMapping
	{
		public MerchantBillMapping()
		{
			ToTable("MerchantBill");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int").IsOptional();
			HasOptional(c =>c.MerchantInfo);
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}