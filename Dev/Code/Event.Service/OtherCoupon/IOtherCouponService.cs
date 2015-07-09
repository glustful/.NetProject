using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event.Entity.Entity.OtherCoupon;
using YooPoon.Core.Autofac;

namespace Event.Service.OtherCoupon
{
    public interface IOtherCouponService : IDependency
    {
        Entity.Entity.OtherCoupon.OtherCoupon Create(Entity.Entity.OtherCoupon.OtherCoupon entity);

        bool Delete(Entity.Entity.OtherCoupon.OtherCoupon entity);

        Entity.Entity.OtherCoupon.OtherCoupon Update(Entity.Entity.OtherCoupon.OtherCoupon entity);

        Entity.Entity.OtherCoupon.OtherCoupon GetCouponById(int id);
        IQueryable<Entity.Entity.OtherCoupon.OtherCoupon> GetCouponByCondition(OtherCouponSearchCondition condition);
        int GetCouponCount(OtherCouponSearchCondition condition);

        Entity.Entity.OtherCoupon.OtherCoupon GetCouponByNumber(string number);
        bool BulkCreate(List<Entity.Entity.OtherCoupon.OtherCoupon> entities);
    }
}
