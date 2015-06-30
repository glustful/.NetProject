using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Event.Entity.Entity.Coupon;
using Event.Service.Coupon;
using Zerg.Common;

namespace Zerg.Event.API.Coupon
{
    public class CouponController : ApiController
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage ActiveCoupon([FromBody]string couponNum)
        {
            var entity = _couponService.GetCouponByNumber(couponNum);
            if (entity == null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "激活失败！无法找到该优惠券"));
            }
            else if(entity.Status ==EnumCouponStatus.Actived)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "激活失败！该优惠券已经激活"));
            }
            entity.Status = EnumCouponStatus.Actived;
            _couponService.Update(entity);
            return PageHelper.toJson(PageHelper.ReturnValue(true, "激活成功！"));
        }
    }
}
