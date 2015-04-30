using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.PointExchangeDetail
{
	public class PointExchangeDetailMapping : EntityTypeConfiguration<PointExchangeDetailEntity>, Zerg.Common.Data.IZergMapping
	{
		public PointExchangeDetailMapping()
		{
			ToTable("PointExchangeDetail");
			HasKey(c => c.Id);
			 
			HasOptional(c =>c.Broker);
			Property(c => c.Userpointsds).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Userpoints).HasColumnType("int");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}