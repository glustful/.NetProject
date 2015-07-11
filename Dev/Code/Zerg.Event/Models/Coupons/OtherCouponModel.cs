using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Event.Entity.Entity.OtherCoupon;

namespace Zerg.Event.Models.Coupons
{
    public class OtherCouponModel
    {     
            public int Id { get; set; }
            public string Number { get; set; }
            public decimal Price { get; set; }
            public EnumOtherCouponStatus Status { get; set; }
            public int CouponCategoryId { get; set; }
            public int Count { get; set; }
    }
}