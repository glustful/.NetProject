using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Event.Entity.Entity.Coupon;

namespace Zerg.Event.Models.Coupons
{
    public class CouponModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public decimal Price { get; set; }
        public EnumCouponStatus Status { get; set; }
        public int CouponCategoryId { get; set; }
    }
}