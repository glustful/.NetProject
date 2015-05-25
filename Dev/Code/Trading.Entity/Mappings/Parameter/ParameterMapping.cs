using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Trading.Entity.Model;

namespace Trading.Entity.Mappings.Parameter
{
	public class ParameterMapping : EntityTypeConfiguration<ParameterEntity>, Zerg.Common.Data.IZergMapping
	{
		public ParameterMapping()
		{
			ToTable("Parameter");
			HasKey(c => c.Id);
			//Property(c => c.Id).HasColumnType("int").IsOptional();
			HasOptional(c =>c.Classify);
			Property(c => c.Name).HasColumnType("varchar").HasMaxLength(200);
			Property(c => c.Sort).HasColumnType("int");
			Property(c => c.Adduser).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upduser).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Updtime).HasColumnType("datetime");
		}
	}
}