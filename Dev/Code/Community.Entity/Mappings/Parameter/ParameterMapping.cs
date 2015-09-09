using System.Data.Entity.ModelConfiguration;
using Community.Entity.Model.Parameter;
using Zerg.Common.Data;

namespace Community.Entity.Mappings.Parameter
{
	public class ParameterMapping : EntityTypeConfiguration<ParameterEntity>, IZergMapping
	{
		public ParameterMapping()
		{
			ToTable("Parameter");
			HasKey(c => c.Id);
			
			HasRequired(c =>c.Category).WithMany().WillCascadeOnDelete(false);
			Property(c => c.Name).HasColumnType("varchar").HasMaxLength(200).IsOptional();
			Property(c => c.Sort).HasColumnType("int").IsOptional();
			Property(c => c.AddUser).HasColumnType("int").IsOptional();
			Property(c => c.AddTime).HasColumnType("datetime").IsOptional();
			Property(c => c.UpdUser).HasColumnType("int").IsOptional();
			Property(c => c.UpdTime).HasColumnType("datetime").IsOptional();

		    HasMany(c => c.Values).WithRequired(c => c.Parameter).WillCascadeOnDelete(false);
		}
	}
}