using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using Event.Entity.Model;
using Zerg.Common.Data;

namespace Event.Entity.Mappings.Discount
{
	public class DiscountMapping : EntityTypeConfiguration<DiscountEntity>, IZergMapping
	{
		public DiscountMapping()
		{
			ToTable("Discount");
			HasKey(c => c.Id);

		    HasOptional(c => c.Crowd);
			Property(c => c.Number).HasColumnType("int");
			Property(c => c.Discount).HasColumnType("int");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}