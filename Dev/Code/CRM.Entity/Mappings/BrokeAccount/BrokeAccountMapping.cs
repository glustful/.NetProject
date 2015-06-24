using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.BrokeAccount
{
	public class BrokeAccountMapping : EntityTypeConfiguration<BrokeAccountEntity>, Zerg.Common.Data.IZergMapping
	{
		public BrokeAccountMapping()
		{
			ToTable("BrokeAccount");
			HasKey(c => c.Id);

			HasOptional(c =>c.Broker);
            Property(c => c.MoneyDesc).HasColumnType("varchar").HasMaxLength(256);
            Property(c => c.Balancenum).HasColumnType("decimal");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}