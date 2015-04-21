using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.Bank
{
	public class BankMapping : EntityTypeConfiguration<BankEntity>, IMapping
	{
		public BankMapping()
		{
			ToTable("Bank");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int");
			Property(c => c.Codeid).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.address).HasColumnType("varchar").HasMaxLength(256);
		}
	}
}