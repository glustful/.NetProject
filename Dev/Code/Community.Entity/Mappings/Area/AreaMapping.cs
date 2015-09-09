using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Community.Entity.Model;
using Community.Entity.Model.Area;
using Zerg.Common.Data;

namespace Community.Entity.Mappings.Area
{
	public class AreaMapping : EntityTypeConfiguration<AreaEntity>, IZergMapping
	{
		public AreaMapping()
		{
			ToTable("Area");
			HasKey(c => c.Id);

			Property(c =>c.CodeId);
			Property(c =>c.AddDate);
			HasOptional(c =>c.Parent);
			Property(c => c.Name).HasColumnType("char").HasMaxLength(10).IsOptional();
		}
	}
}