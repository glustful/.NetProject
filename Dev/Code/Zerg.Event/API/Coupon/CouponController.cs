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
using Zerg.Event.Models.Coupons;

namespace Zerg.Event.API.Coupon
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
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
        [HttpGet]
        public HttpResponseMessage Index(int page, int pageSize)
        {
            var condition = new CouponSearchCondition
            {
                Page = page,
                PageCount = pageSize,
                OrderBy = EnumCouponSearchOrderBy.OrderById
            };
            var coupon = _couponService.GetCouponByCondition(condition).Select(p => new
            {
               p.Id,
               p.Number,
               p.Price,
               p.Status
            }).ToList();
            var count = _couponService.GetCouponCount(condition);
            return PageHelper.toJson(new { List = coupon, TotalCount = count, Condition = condition });
        }
        [HttpGet]
        public HttpResponseMessage Detailed(int id)
        {
            var coupon = _couponService.GetCouponById(id);
            return PageHelper.toJson(coupon);
        }
        /// <summary>
        /// 单个新建
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Create(CouponModel model)
        {
            Random random=new Random();
            int num = random.Next(10000000, 100000000);
            var coupon = new global::Event.Entity.Entity.Coupon.Coupon
            {
                Number = num.ToString(),
                Price = model.Price,
                Status = model.Status,
                CouponCategoryId = model.CouponCategoryId
            };
            if (_couponService.Create(coupon) != null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败"));
        }
        [HttpPost]
        public HttpResponseMessage BlukCreate(CouponModel model)
        {
            Random random = new Random();           
            List<global::Event.Entity.Entity.Coupon.Coupon> list = new List<global::Event.Entity.Entity.Coupon.Coupon>();
            for (int i = 0; i <model.Count; i++)
            {
                list.Add(new global::Event.Entity.Entity.Coupon.Coupon
                {
                    Number =DateTime.Now.ToString("HHmmss")+random.Next(10000000, 100000000),
                    Price = model.Price,
                    Status = model.Status,
                    CouponCategoryId = model.CouponCategoryId
                });               
            }
            if (_couponService.BulkCreate(list))
            {
                 return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功"));
            }            
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败"));
        }

        [HttpPost]
        public HttpResponseMessage Edit(CouponModel model)
        {
            var coupon = _couponService.GetCouponById(model.Id);
            coupon.Number = model.Number;
            coupon.Price = model.Price;
            coupon.Status = model.Status;
            coupon.CouponCategoryId = model.CouponCategoryId;
            if (_couponService.Update(coupon) != null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败"));
        }
        [HttpGet]
        public HttpResponseMessage Delete(int id)
        {
            var coupon = _couponService.GetCouponById(id);
            if (_couponService.Delete(coupon))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败"));
        }
    }
}
