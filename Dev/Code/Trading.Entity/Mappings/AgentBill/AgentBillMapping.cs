using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Trading.Entity.Model;

namespace Trading.Entity.Mappings.AgentBill
{
	public class AgentBillMapping : EntityTypeConfiguration<AgentBillEntity>, Zerg.Common.Data.IZergMapping
	{
		public AgentBillMapping()
		{
			ToTable("AgentBill");
			HasKey(c => c.Id);
			////Property(c => c.Id).HasColumnType("int").IsOptional();
			HasOptional(c =>c.Order);
			Property(c => c.AgentId).HasColumnType("int");
			Property(c => c.Agentname).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.LandagentId).HasColumnType("int");
			Property(c => c.Landagentname).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Amount).HasColumnType("decimal");
			Property(c => c.Actualamount).HasColumnType("decimal").IsOptional();
			Property(c => c.Cardnumber).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.Isinvoice).HasColumnType("bit");
			Property(c => c.Remark).HasColumnType("varchar").HasMaxLength(500);
			Property(c => c.Beneficiary).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.Beneficiarynumber).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.Checkoutdate).HasColumnType("datetime");
			Property(c => c.Customname).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Adduser).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upduser).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Updtime).HasColumnType("datetime");
		}
	}
}