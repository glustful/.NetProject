using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Trading.Entity.Model;

namespace Trading.Entity.Mappings.ParameterValue
{
	public class ParameterValueMapping : EntityTypeConfiguration<ParameterValueEntity>, Zerg.Common.Data.IZergMapping
	{
		public ParameterValueMapping()
		{
			ToTable("ParameterValue");
			HasKey(c => c.Id);
			//Property(c => c.Id).HasColumnType("int").IsOptional();
			HasOptional(c =>c.Parameter);
			Property(c => c.Parametervalue).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Sort).HasColumnType("int");
			Property(c => c.Adduser).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upduser).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Updtime).HasColumnType("datetime");
		}
	}
}