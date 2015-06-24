using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.RecommendAgent
{
	public class RecommendAgentMapping : EntityTypeConfiguration<RecommendAgentEntity>, Zerg.Common.Data.IZergMapping
	{
		public RecommendAgentMapping()
		{
			ToTable("RecommendAgent");
			HasKey(c => c.Id);
			 
			HasOptional(c =>c.Broker);
			Property(c => c.PresenteebId).HasColumnType("int");
			Property(c => c.Brokername).HasColumnType("varchar").HasMaxLength(50);
            Property(c => c.Phone).HasColumnType("varchar").HasMaxLength(50);
            Property(c => c.Qq).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Agentlevel).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Regtime).HasColumnType("datetime");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}