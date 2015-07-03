﻿using System.Data.Entity.ModelConfiguration;
using Event.Entity.Entity.Coupon;
using Event.Entity.Model;
using Zerg.Common.Data;

namespace Event.Entity.Mappings.Coupon
{
    public class CouponCategoryMapping : EntityTypeConfiguration<CouponCategory>, IZergMapping
    {
        public CouponCategoryMapping()
        {
            ToTable("Coupon_Category");
            HasKey(c => c.Id);
            Property(c => c.Name).HasColumnType("varchar").IsOptional();
            Property(c => c.ReMark).HasColumnType("varchar").IsOptional();
            Property(c => c.Count).HasColumnType("int").IsOptional();
            Property(c => c.BrandId).HasColumnType("int").IsOptional();
            Property(c => c.Price).HasColumnType("varchar").IsOptional();
        }
    }
}