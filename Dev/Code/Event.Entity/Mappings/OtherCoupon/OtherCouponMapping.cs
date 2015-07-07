using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zerg.Common.Data;

namespace Event.Entity.Mappings.OtherCoupon
{
    class OtherCouponMapping : EntityTypeConfiguration<Entity.OtherCoupon.OtherCoupon>, IZergMapping
    {
        public OtherCouponMapping()
        {
            ToTable("OtherCoupon");
            HasKey(c => c.Id);
            Property(c => c.Number).HasColumnType("varchar").IsOptional();
            Property(c => c.Price).HasColumnType("decimal").IsOptional();
            Property(c => c.Status).HasColumnType("int").IsOptional();
            Property(c => c.CouponCategoryId).HasColumnType("int").IsOptional();
        }
    }
}
