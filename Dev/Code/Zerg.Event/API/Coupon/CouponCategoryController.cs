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
using Trading.Service.ProductBrand;
using YooPoon.Core.Site;
using Zerg.Common;
using Zerg.Event.Models.Coupons;

namespace Zerg.Event.API.Coupon
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    public class CouponCategoryController : ApiController
    {
        private readonly ICouponCategoryService _couponCategoryService;
        private readonly IProductBrandService _productBrandService;       

        public CouponCategoryController(ICouponCategoryService couponCategoryService,IProductBrandService productBrandService)
        {
            _couponCategoryService = couponCategoryService;
            _productBrandService = productBrandService;         
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
                ReMark = model.ReMark,
                ClassId=model.ClassId
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
        public HttpResponseMessage coupons()
        {
            var ConConditon = new CouponCategorySearchCondition
            {
                OrderBy = EnumCouponCategorySearchOrderBy.OrderById               
            };
            var list = _couponCategoryService.GetCouponCategoriesByCondition(ConConditon).Select(p => new
            {
                p.Id,
                p.BrandId,
                p.Name,
                p.ReMark,
                p.Price,
                p.Count
            }).ToList().Select(pp => new CouponCategoryModel
            {
                Id = pp.Id,
                Name = pp.Name,
                Price = pp.Price,
                Count = pp.Count,
                ReMark = pp.ReMark,
                BrandImg = _productBrandService.GetProductBrandById(pp.BrandId).Bimg,
                SubTitle = _productBrandService.GetProductBrandById(pp.BrandId).SubTitle,
                ProductParamater = _productBrandService.GetProductBrandById(pp.BrandId).ParameterEntities.ToDictionary(k => k.Parametername, v => v.Parametervaule)               
            });
            return PageHelper.toJson(list);
        }
    }
}
