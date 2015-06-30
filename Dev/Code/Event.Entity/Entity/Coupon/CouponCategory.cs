namespace Event.Entity.Entity.Coupon
{
    public class CouponCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ReMark { get; set; }
        public int Count { get; set; }
        public int BrandId { get; set; }
        public string Price { get; set; }
    }
}