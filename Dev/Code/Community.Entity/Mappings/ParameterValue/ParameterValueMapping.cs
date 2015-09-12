using System.Data.Entity.ModelConfiguration;
using Community.Entity.Model.ParameterValue;
using Zerg.Common.Data;

namespace Community.Entity.Mappings.ParameterValue
{
	public class ParameterValueMapping : EntityTypeConfiguration<ParameterValueEntity>, IZergMapping
	{
		public ParameterValueMapping()
		{
			ToTable("ParameterValue");
			HasKey(c => c.Id);
			
			HasRequired(c =>c.Parameter);
			Property(c => c.Value).HasColumnType("varchar").HasMaxLength(50).IsOptional();
			Property(c => c.Sort).HasColumnType("int").IsOptional();
			Property(c => c.AddUser).HasColumnType("int").IsOptional();
			Property(c => c.AddTime).HasColumnType("datetime").IsOptional();
			Property(c => c.UpdUser).HasColumnType("int").IsOptional();
			Property(c => c.UpdTime).HasColumnType("datetime").IsOptional();
		}
	}
}