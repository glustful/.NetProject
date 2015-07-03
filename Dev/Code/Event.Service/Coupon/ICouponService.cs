using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event.Entity.Entity.Coupon;
using YooPoon.Core.Autofac;


namespace Event.Service.Coupon
{
    public interface ICouponService : IDependency
    {
        Entity.Entity.Coupon.Coupon Create(Entity.Entity.Coupon.Coupon entity);

        bool Delete(Entity.Entity.Coupon.Coupon entity);

        Entity.Entity.Coupon.Coupon Update(Entity.Entity.Coupon.Coupon entity);

        Entity.Entity.Coupon.Coupon GetCouponById(int id);
        IQueryable<Entity.Entity.Coupon.Coupon> GetCouponByCondition(CouponSearchCondition condition);
        int GetCouponCount(CouponSearchCondition condition);

        Entity.Entity.Coupon.Coupon GetCouponByNumber(string number);
        bool BulkCreate(List<Entity.Entity.Coupon.Coupon> entities);

    }
}
