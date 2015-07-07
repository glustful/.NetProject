using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event.Entity.Entity.Coupon;
using Event.Entity.Entity.OtherCoupon;
using YooPoon.Core.Autofac;

namespace Event.Service.OtherCoupon
{
    public interface IOtherCouponOwnerService : IDependency
    {
        OtherCouponOwner CreateRecord(int userId, int couponId);

        bool DeleteRecordById(int id);

        bool DeleteRecordByCouponId(int couponId);
        IQueryable<OtherCouponOwner> GetCouponByUserId(int userid);
        IQueryable<OtherCouponOwner> GetCouponOwnByCondition(OtherCouponOwnerSearchCondition condition);
        IQueryable<OtherCouponOwner> GetCouponOwnCountCondition(CouponOwnerSearchCondition condition);
    }
}
