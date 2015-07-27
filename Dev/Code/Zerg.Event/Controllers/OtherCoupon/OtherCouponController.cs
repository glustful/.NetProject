using System.Linq;
using System.Web.Mvc;
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

        public OtherCouponController(
            IOtherCouponCategoryService otherCouponCategoryService,
            IOtherCouponService otherCouponService, 
            IProductBrandService productBrandService, 
            IOtherCouponOwnerService otherCouponOwnerService, 
            IWorkContext workContext
            )
        {
            _otherCouponCategoryService = otherCouponCategoryService;
            _otherCouponService = otherCouponService;
            _productBrandService = productBrandService;
            _otherCouponOwnerService = otherCouponOwnerService;
            _workContext = workContext;
        }

        /// <summary>
        /// 优惠劵显示列表
        /// </summary>
        /// <returns></returns>
        public ActionResult OtherCoupons()
        {
            var conConditon = new OtherCouponCategorySearchCondition
            {
               // ClassId = classId,
                OrderBy = EnumOtherCouponCategorySearchOrderBy.OrderById
            };
            var list = _otherCouponCategoryService.GetCouponCategoriesByCondition(conConditon).Select(p => new
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

        /// <summary>
        /// 获取优惠券
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

            if (coupon != null)
            {
                _otherCouponOwnerService.CreateRecord(_workContext.CurrentUser.Id, coupon.Id);
                coupon.Status = EnumOtherCouponStatus.Owned;
                _otherCouponService.Update(coupon);
                couponCategory.Count = couponCategory.Count - 1;
                _otherCouponCategoryService.UpdateCouponCategory(couponCategory);
                var brand = _productBrandService.GetProductBrandById(couponCategory.BrandId);
                var couponOwn = new CouponCategoryModel
                {
                    Name = couponCategory.Name,
                    Number = coupon.Number,
                    BrandName = brand.Bname
                };
                return View(couponOwn);
            }
            return RedirectToAction("OtherCoupons", "OtherCoupons");
        }
    }
}