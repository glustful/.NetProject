using YooPoon.Core.Data;

namespace Event.Entity.Entity.Coupon
{
    public class Coupon : IBaseEntity
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        public int CouponCategoryId { get; set; }
    }
}