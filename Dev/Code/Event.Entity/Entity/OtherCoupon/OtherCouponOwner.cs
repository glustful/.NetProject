using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YooPoon.Core.Data;

namespace Event.Entity.Entity.OtherCoupon
{
    public class OtherCouponOwner : IBaseEntity
    {
        public int Id { get; set; }
        public int CouponId { get; set; }
        public int UserId { get; set; }
    }
}
