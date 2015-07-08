using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event.Entity.Entity.OtherCoupon;
using Zerg.Common.Data;

namespace Event.Entity.Mappings.OtherCoupon
{
    class OtherCouponOwnerMapping : EntityTypeConfiguration<OtherCouponOwner>, IZergMapping
    {
        public OtherCouponOwnerMapping()
        {
            ToTable("OtherCouponOwner");
            HasKey(c => c.Id);
            Property(c => c.CouponId).HasColumnType("int").IsOptional();
            Property(c => c.UserId).HasColumnType("int").IsOptional();
        }
    }
}
