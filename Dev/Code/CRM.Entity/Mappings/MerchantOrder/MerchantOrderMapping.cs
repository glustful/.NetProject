using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.MerchantOrder
{
	public class MerchantOrderMapping : EntityTypeConfiguration<MerchantOrderEntity>, IMapping
	{
		public MerchantOrderMapping()
		{
			ToTable("MerchantOrder");
			HasKey(c => c.Id);
			 
			HasOptional(c =>c.MerchantInfo);
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}