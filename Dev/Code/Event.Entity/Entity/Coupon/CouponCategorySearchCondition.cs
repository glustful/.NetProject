﻿namespace Event.Entity.Entity.Coupon
{
    public class CouponCategorySearchCondition
    {
        public int? Page { set; get; }

        public int? PageSize { get; set; }

        public int? BrandId { get; set; }
        public bool IsDescending { get; set; }
        public string Name { get; set; }
        public EnumCouponCategorySearchOrderBy? OrderBy { get; set; }
    }
    public enum EnumCouponCategorySearchOrderBy
    {
        OrderById,
    }
}