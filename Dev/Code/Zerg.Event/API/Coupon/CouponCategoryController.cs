using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Event.Entity.Entity.Coupon;
using Event.Service.Coupon;
using Zerg.Common;
using Zerg.Event.Models.Coupons;

namespace Zerg.Event.API.Coupon
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    public class CouponCategoryController : ApiController
    {
        private readonly ICouponCategoryService _couponCategoryService;

        public CouponCategoryController(ICouponCategoryService couponCategoryService)
        {
            _couponCategoryService = couponCategoryService;
        }
        [HttpGet]
        public HttpResponseMessage Index(int page, int pageSize,string name=null)
        {
            var condition=new CouponCategorySearchCondition
            {
                Name = name,
                Page = page,
                PageSize =pageSize,
                OrderBy = EnumCouponCategorySearchOrderBy.OrderById
            };
            var couponCategory = _couponCategoryService.GetCouponCategoriesByCondition(condition).Select(p =>new
            {
                p.Id,
                p.Name,
                p.Count,
                p.Price,
                p.ReMark
            }).ToList();
            var count = _couponCategoryService.GetCouponCategoriesCountByCondition(condition);
            return PageHelper.toJson(new {List=couponCategory,TotalCount=count,Condition=condition});
        }
        [HttpPost]
        public HttpResponseMessage Create(CategoryModel model)
        {
            var couponCategory=new CouponCategory
            {
                Name = model.Name,
                Price = model.Price,
                BrandId =model.BrandId,
                Count = model.Count,
                ReMark = model.ReMark
            };
            if (_couponCategoryService.CreateCouponCategory(couponCategory))
            {
                 return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败"));
        }
        [HttpGet]
        public HttpResponseMessage Detailed(int id)
        {
            var couponCategory = _couponCategoryService.GetCouponCategoryById(id);
            return PageHelper.toJson(couponCategory);
        }
        [HttpPost]
        public HttpResponseMessage Edit(CategoryModel model)
        {
            var couponCategory = _couponCategoryService.GetCouponCategoryById(model.Id);
            couponCategory.Name = model.Name;
            couponCategory.Count = model.Count;
            couponCategory.Price = model.Price;
            couponCategory.ReMark = model.ReMark;
            couponCategory.BrandId = model.BrandId;
            if (_couponCategoryService.UpdateCouponCategory(couponCategory))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据修改成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据修改失败"));
        }
        [HttpGet]
        public HttpResponseMessage Delete(int id)
        {
            if (_couponCategoryService.DeleteCouponCategory(id))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败"));
        }
    }
}
