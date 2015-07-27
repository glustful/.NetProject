using YooPoon.Core.Data;

namespace Event.Entity.Entity.Coupon
{
    public class CouponCategory:IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ReMark { get; set; }
        public int Count { get; set; }
        public int BrandId { get; set; }
        public string Price { get; set; }
        public int ClassId { get; set; }
        public string Intro { get; set; }
        public string Content { get; set; }
    }
}