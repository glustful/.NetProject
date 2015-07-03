using System;
using System.Linq;
using Event.Entity.Entity.Coupon;
using YooPoon.Core.Autofac;

namespace Event.Service.Coupon
{
    public interface ICouponOwnerService : IDependency
    {
        CouponOwner CreateRecord(int userId, int couponId);

        bool DeleteRecordById(int id);

        bool DeleteRecordByCouponId(int couponId);
        IQueryable<CouponOwner> GetCouponOwnByCondition(CouponOwnerSearchCondition condition);
        IQueryable<CouponOwner> GetCouponOwnCountCondition(CouponOwnerSearchCondition condition);
    }
}