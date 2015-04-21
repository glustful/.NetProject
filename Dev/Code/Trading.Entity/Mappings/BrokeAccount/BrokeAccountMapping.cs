using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.BrokeAccount
{
	public class BrokeAccountMapping : EntityTypeConfiguration<BrokeAccountEntity>, IMapping
	{
		public BrokeAccountMapping()
		{
			ToTable("BrokeAccount");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int");
			HasOptional(c =>c.Broker);
			Property(c => c.Balancenum).HasColumnType("float");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}