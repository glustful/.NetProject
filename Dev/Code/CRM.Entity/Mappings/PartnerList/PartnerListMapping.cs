using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.PartnerList
{
	public class PartnerListMapping : EntityTypeConfiguration<PartnerListEntity>, Zerg.Common.Data.IZergMapping
	{
		public PartnerListMapping()
		{
			ToTable("PartnerList");
			HasKey(c => c.Id);
			 
			HasRequired(c =>c.Broker);
			Property(c => c.PartnerId).HasColumnType("int");
			Property(c => c.Brokername).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Phone).HasColumnType("int");
			Property(c => c.Agentlevel).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Regtime).HasColumnType("datetime");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		    Property(c => c.Status).HasColumnType("int");
		}
	}
}