using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Event.Entity.Model;

namespace Event.Entity.Mappings.Discount
{
	public class DiscountMapping : EntityTypeConfiguration<DiscountEntity>, IMapping
	{
		public DiscountMapping()
		{
			ToTable("Discount");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int").IsOptional();
			HasOptional(c =>c.Crowd);
			Property(c => c.Number).HasColumnType("int");
			Property(c => c.Discount).HasColumnType("int");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}