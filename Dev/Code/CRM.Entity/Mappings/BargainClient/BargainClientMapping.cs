using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.BargainClient
{
	public class BargainClientMapping : EntityTypeConfiguration<BargainClientEntity>, IMapping
	{
		public BargainClientMapping()
		{
			ToTable("BargainClient");
			HasKey(c => c.Id);

			HasOptional(c =>c.MerchantInfo);
			HasOptional(c =>c.ClientInfo);
			Property(c => c.Dealtime).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}