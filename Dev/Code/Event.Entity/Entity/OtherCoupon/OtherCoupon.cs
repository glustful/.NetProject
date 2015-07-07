using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YooPoon.Core.Data;

namespace Event.Entity.Entity.OtherCoupon
{
    public class OtherCoupon : IBaseEntity
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public decimal Price { get; set; }
        public EnumOtherCouponStatus Status { get; set; }
        public int CouponCategoryId { get; set; }
    }
    public enum EnumOtherCouponStatus
    {
        Created,
        Owned,
        Actived,
        Used
    }
}
