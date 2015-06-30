namespace Event.Entity.Entity.Coupon
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        public int CouponCategoryId { get; set; }
    }
}