using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;
using Zerg.Common.Data;

namespace CRM.Entity.Mappings.Bank
{
    public class BankMapping : EntityTypeConfiguration<BankEntity>, Zerg.Common.Data.IZergMapping
	{
		public BankMapping()
		{
			ToTable("Bank");
			HasKey(c => c.Id);

			Property(c => c.Codeid).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.address).HasColumnType("varchar").HasMaxLength(256);
		}
	}
}