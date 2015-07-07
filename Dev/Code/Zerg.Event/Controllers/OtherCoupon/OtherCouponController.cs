using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Event.Entity.Entity.Coupon;
using Event.Entity.Entity.OtherCoupon;
using Event.Service.OtherCoupon;
using Trading.Service.ProductBrand;
using YooPoon.Core.Site;
using Zerg.Event.Models.Coupons;

namespace Zerg.Event.Controllers.OtherCoupon
{
    public class OtherCouponController : Controller
    {

        private readonly IProductBrandService _productBrandService;
        private readonly IWorkContext _workContext;
        private readonly IOtherCouponCategoryService _otherCouponCategoryService;
        private readonly IOtherCouponService _otherCouponService;
        private readonly IOtherCouponOwnerService _otherCouponOwnerService;

        public OtherCouponController(IOtherCouponCategoryService otherCouponCategoryService, IOtherCouponService otherCouponService, IProductBrandService productBrandService, IOtherCouponOwnerService otherCouponOwnerService, IWorkContext workContext)
        {
            _otherCouponCategoryService = otherCouponCategoryService;
            _otherCouponService = otherCouponService;
            _productBrandService = productBrandService;
            _otherCouponOwnerService = otherCouponOwnerService;
            _workContext = workContext;
        }
        public ActionResult OtherCoupons()
        {
            var ConConditon = new OtherCouponCategorySearchCondition
            {
                OrderBy = EnumOtherCouponCategorySearchOrderBy.OrderById
            };
            var list = _otherCouponCategoryService.GetCouponCategoriesByCondition(ConConditon).Select(p => new
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
            return View(list);
        }

        public ActionResult OtherCouponOwn(int id)
        {
            if (_workContext.CurrentUser == null)
            {
                return Redirect("http://www.iyookee.cn/#/user/login");
            }
            var couponCategory = _otherCouponCategoryService.GetCouponCategoryById(id);
            var condition = new OtherCouponSearchCondition
            {
                CouponCategoryId = id,
                Status = 0
            };
            var coupon = _otherCouponService.GetCouponByCondition(condition).FirstOrDefault();

                _otherCouponOwnerService.CreateRecord(_workContext.CurrentUser.Id, coupon.Id);
                coupon.Status = EnumOtherCouponStatus.Owned;
                _otherCouponService.Update(coupon);
                couponCategory.Count = couponCategory.Count - 1;
                _otherCouponCategoryService.UpdateCouponCategory(couponCategory);
                var brand = _productBrandService.GetProductBrandById(couponCategory.BrandId);
                var CouponOwn = new CouponCategoryModel
                {
                    Name = couponCategory.Name,
                    Number = coupon.Number,
                    BrandName = brand.Bname
                };
                return View(CouponOwn);

            //return RedirectToAction("coupons", "Coupons");
        }
    }
}