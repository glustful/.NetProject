using System.Data.Entity.ModelConfiguration;
using Zerg.Common.Data;

namespace Event.Entity.Mappings.Coupon
{
    public class CouponMapping : EntityTypeConfiguration<Entity.Coupon.Coupon>, IZergMapping
    {
        public CouponMapping()
        {
            ToTable("Coupon");
            HasKey(c => c.Id);
            Property(c => c.Number).HasColumnType("varchar").IsOptional();
            Property(c => c.Price).HasColumnType("decimal").IsOptional();
            Property(c => c.Status).HasColumnType("int").IsOptional();
            Property(c => c.CouponCategoryId).HasColumnType("int").IsOptional();
            Property(c => c.Content).HasColumnType("text").IsOptional();
        }
    }
}