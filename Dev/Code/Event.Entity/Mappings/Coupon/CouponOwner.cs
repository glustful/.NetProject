using System.Data.Entity.ModelConfiguration;
using Event.Entity.Entity.Coupon;
using Zerg.Common.Data;

namespace Event.Entity.Mappings.Coupon
{
    public class CouponOwnerMapping : EntityTypeConfiguration<CouponOwner>, IZergMapping
    {
        public CouponOwnerMapping()
        {
            ToTable("Coupon_Owner");
            HasKey(c => c.Id);
            Property(c => c.CouponId).HasColumnType("int").IsOptional();
            Property(c => c.UserId).HasColumnType("int").IsOptional();
        }
    }
}