using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.MerchantInfo
{
	public class MerchantInfoMapping : EntityTypeConfiguration<MerchantInfoEntity>, IMapping
	{
		public MerchantInfoMapping()
		{
			ToTable("MerchantInfo");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int").IsOptional();
			Property(c => c.UserId).HasColumnType("int");
			Property(c => c.Merchantname).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Mail).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Address).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Phone).HasColumnType("varchar").HasMaxLength(11);
			Property(c => c.Describe).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.License).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Legalhuman).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Legalsfz).HasColumnType("varchar").HasMaxLength(18);
			Property(c => c.Orgnum).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Taxnum).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}