using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event.Entity.Entity.OtherCoupon;
using YooPoon.Core.Autofac;

namespace Event.Service.OtherCoupon
{
    public interface IOtherCouponCategoryService : IDependency
    {
        bool CreateCouponCategory(OtherCouponCategory entity);

        IQueryable<OtherCouponCategory> GetCouponCategoriesByCondition(OtherCouponCategorySearchCondition condition);

        int GetCouponCategoriesCountByCondition(OtherCouponCategorySearchCondition condition);

        OtherCouponCategory GetCouponCategoryById(int id);

        bool UpdateCouponCategory(OtherCouponCategory entity);

        bool DeleteCouponCategory(int id);

        int GetCouponNums(int categoryId);
    }
}
