using YooPoon.Core.Data;

namespace Event.Entity.Entity.Coupon
{
    public class CouponOwner:IBaseEntity
    {
        public int Id { get; set; }
        public int CouponId { get; set; }
        public int UserId { get; set; }
    }
}