using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Event.Entity.Entity.Coupon;
using Event.Service.Coupon;
using Trading.Service.ProductBrand;
using YooPoon.Core.Site;
using Zerg.Event.Models.Coupons;

namespace Zerg.Event.Controllers.Coupons
{
    public class CouponsController : Controller
    {
        private readonly ICouponCategoryService _couponCategoryService;
        private readonly IProductBrandService _productBrandService;
        private readonly ICouponOwnerService _couponOwnerService;
        private readonly IWorkContext _workContext;
        private readonly ICouponService _couponService;

        public CouponsController(ICouponCategoryService couponCategoryService, ICouponService couponService, IProductBrandService productBrandService, ICouponOwnerService couponOwnerService, IWorkContext workContext)
        {
            _couponCategoryService = couponCategoryService;
            _couponService = couponService;
            _productBrandService = productBrandService;
            _couponOwnerService = couponOwnerService;
            _workContext = workContext;
        }
        #region 彭贵飞 获取手机端显示的优惠券种类和对应品牌信息
        /// <summary>
        /// 获取优惠券种类
        /// </summary>
        /// <returns>list列表</returns>
        public ActionResult Coupons()
        {
            var Con = new CouponCategorySearchCondition
            {
                OrderBy = EnumCouponCategorySearchOrderBy.OrderById
            };
            var list = _couponCategoryService.GetCouponCategoriesByCondition(Con).Select(p => new
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
                BrandId = pp.BrandId,
                BrandImg = _productBrandService.GetProductBrandById(pp.BrandId).Bimg,
                SubTitle = _productBrandService.GetProductBrandById(pp.BrandId).SubTitle,
                ProductParamater = _productBrandService.GetProductBrandById(pp.BrandId).ParameterEntities.ToDictionary(k => k.Parametername, v => v.Parametervaule)
            });
            return View(list);
        }
        #endregion
        #region 彭贵飞 抢优惠券
        /// <summary>
        /// 抢优惠券
        /// </summary>
        /// <param name="id">优惠券种类Id</param>
        /// <returns></returns>

        public ActionResult couponOwn(int id)
        {
            if (_workContext.CurrentUser == null)
            {
                return Redirect("http://www.iyookee.cn/#/user/login");
            }
            var couponCategory = _couponCategoryService.GetCouponCategoryById(id);
            var condition = new CouponSearchCondition
            {
                CouponCategoryId = id,
                Status = 0
            };
            var coupon = _couponService.GetCouponByCondition(condition).FirstOrDefault();
            _couponOwnerService.CreateRecord(_workContext.CurrentUser.Id, coupon.Id);
            coupon.Status = EnumCouponStatus.Owned;
            _couponService.Update(coupon);
            couponCategory.Count = couponCategory.Count - 1;
            _couponCategoryService.UpdateCouponCategory(couponCategory);
            var brand = _productBrandService.GetProductBrandById(couponCategory.BrandId);
            var CouponOwn = new CouponCategoryModel
            {
                Name = couponCategory.Name,
                Number = coupon.Number,
                BrandName = brand.Bname
            };
            return View(CouponOwn);
        }
        #endregion
    }
}