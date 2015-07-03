using Event.Entity.Entity.Coupon;
using YooPoon.Core.Autofac;
using System.Linq;

namespace Event.Service.Coupon
{
    public interface ICouponOwnerService : IDependency
    {
        CouponOwner CreateRecord(int userId, int couponId);

        bool DeleteRecordById(int id);

        bool DeleteRecordByCouponId(int couponId);
        IQueryable<CouponOwner> GetCouponByUserId(int userid);
    }
}