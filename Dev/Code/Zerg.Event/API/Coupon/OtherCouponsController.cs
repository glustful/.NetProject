using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Service.Broker;
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
    public class OtherCouponsController : ApiController
    {
        private readonly ICouponCategoryService _couponCategoryService;
        private readonly IProductBrandService _productBrandService;
        private readonly ICouponOwnerService _couponOwnerService;
        private readonly IWorkContext _workContext;
        private readonly ICouponService _couponService;
        private readonly IBrokerService _brokerService;

        public OtherCouponsController(
            ICouponCategoryService couponCategoryService, 
            ICouponService couponService, 
            IProductBrandService productBrandService, 
            ICouponOwnerService couponOwnerService, 
            IWorkContext workContext,
            IBrokerService brokerService
            )
        {
            _couponCategoryService = couponCategoryService;
            _couponService = couponService;
            _productBrandService = productBrandService;
            _couponOwnerService = couponOwnerService;
            _workContext = workContext;
            _brokerService = brokerService;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetList()
        {
            var con = new CouponCategorySearchCondition
            {
                OrderBy = EnumCouponCategorySearchOrderBy.OrderById
            };
            var list = _couponCategoryService.GetCouponCategoriesByCondition(con).Select(p => new
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

            return PageHelper.toJson(list);
        }

        /// <summary>
        /// 获取优惠券
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage couponOwn(int id)
        {
            if (_workContext.CurrentUser == null)
            {
                return PageHelper.toJson("error");
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
            var phone = _brokerService.GetBrokerByUserId(_workContext.CurrentUser.Id).Phone;

            //短信发送
            SMSHelper.Sending(phone, "优惠券为：" + brand.Bname + "，券号为：" + coupon.Number + " 【优客惠】");

            return PageHelper.toJson(CouponOwn);
        }
    }
}
