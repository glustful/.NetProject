using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.BankCard
{
	public class BankCardMapping : EntityTypeConfiguration<BankCardEntity>, IMapping
	{
		public BankCardMapping()
		{
			ToTable("BankCard");
			HasKey(c => c.Id);

			HasOptional(c =>c.Bank);
			HasOptional(c =>c.Broker);
			Property(c => c.Num).HasColumnType("int");
			Property(c => c.Type).HasColumnType("bit");
			Property(c => c.Deadline).HasColumnType("datetime").IsOptional();
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}