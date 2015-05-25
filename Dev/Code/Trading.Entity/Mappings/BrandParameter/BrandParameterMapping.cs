using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Trading.Entity.Model;

namespace Trading.Entity.Mappings.BrandParameter
{
	public class BrandParameterMapping : EntityTypeConfiguration<BrandParameterEntity>, Zerg.Common.Data.IZergMapping
	{
		public BrandParameterMapping()
		{
			ToTable("BrandParameter");
			HasKey(c => c.Id);
			////Property(c => c.Id).HasColumnType("int").IsOptional();
			HasOptional(c =>c.ProductBrand);
			Property(c => c.Sort).HasColumnType("int");
			Property(c => c.Parametername).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Parametervaule).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Adduser).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upduser).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Updtime).HasColumnType("datetime");
		}
	}
}