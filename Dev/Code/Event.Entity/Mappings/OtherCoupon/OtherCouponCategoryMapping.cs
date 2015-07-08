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
    public class OtherCouponCategoryMapping : EntityTypeConfiguration<OtherCouponCategory>, IZergMapping
    {
        public OtherCouponCategoryMapping()
        {
            ToTable("OtherCouponCategory");
            HasKey(c => c.Id);
            Property(c => c.Name).HasColumnType("varchar").IsOptional();
            Property(c => c.ReMark).HasColumnType("varchar").IsOptional();
            Property(c => c.Count).HasColumnType("int").IsOptional();
            Property(c => c.BrandId).HasColumnType("int").IsOptional();
            Property(c => c.Price).HasColumnType("varchar").IsOptional();
            Property(c => c.ClassId).HasColumnType("int").IsOptional();
        }
    }
}
