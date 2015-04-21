using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.BrokerWithdrawDetail
{
	public class BrokerWithdrawDetailMapping : EntityTypeConfiguration<BrokerWithdrawDetailEntity>, IMapping
	{
		public BrokerWithdrawDetailMapping()
		{
			ToTable("BrokerWithdrawDetail");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int");
			HasOptional(c =>c.Broker);
			HasOptional(c =>c.BankCard);
			Property(c => c.Withdrawtime).HasColumnType("datetime");
			Property(c => c.Withdrawnum).HasColumnType("float");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}