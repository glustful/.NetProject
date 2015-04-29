using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.PointDetail
{
	public class PointDetailMapping : EntityTypeConfiguration<PointDetailEntity>, Zerg.Common.Data.IZergMapping
	{
		public PointDetailMapping()
		{
			ToTable("PointDetail");
			HasKey(c => c.Id);
			 
			HasOptional(c =>c.Broker);
			Property(c => c.Pointsds).HasColumnType("varchar").HasMaxLength(50).IsOptional();
			Property(c => c.Addpoints).HasColumnType("int");
			Property(c => c.Totalpoints).HasColumnType("int");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}