using System.Linq;
using Event.Entity.Entity.Coupon;
using YooPoon.Core.Autofac;

namespace Event.Service.Coupon
{
    public interface ICouponCategoryService : IDependency
    {
       bool CreateCouponCategory(CouponCategory entity);

        IQueryable<CouponCategory> GetCouponCategoriesByCondition(CouponCategorySearchCondition condition);

        int GetCouponCategoriesCountByCondition(CouponCategorySearchCondition condition);

        CouponCategory GetCouponCategoryById(int id);

        bool UpdateCouponCategory(CouponCategory entity);

        bool DeleteCouponCategory(int id);

        int GetCouponNums(int categoryId);
    }
}