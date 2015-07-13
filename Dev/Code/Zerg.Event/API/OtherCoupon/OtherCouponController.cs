using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Event.Entity.Entity.OtherCoupon;
using Event.Service.OtherCoupon;
using Zerg.Common;
using Zerg.Event.Models.Coupons;

namespace Zerg.Event.API.OtherCoupon
{
    public class OtherCouponController : ApiController
    {
        private readonly IOtherCouponService _otherCouponService;     

        public OtherCouponController(IOtherCouponService otherCouponService)
        {
            _otherCouponService = otherCouponService;            
        }
        #region 彭贵飞 获取优惠券列表
        /// <summary>
        /// 查询优惠券列表
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="pageSize">页面记录数</param>
        /// <param name="number">优惠券验证码</param>
        /// <returns>list列表</returns>
        [HttpGet]
        public HttpResponseMessage Index(int page, int pageSize, string number)
        {
            var condition = new OtherCouponSearchCondition
            {
                Number = number,
                Page = page,
                PageCount = pageSize,
                OrderBy = EnumOtherCouponSearchOrderBy.OrderById
            };
            var coupon = _otherCouponService.GetCouponByCondition(condition).Select(p => new
            {
                p.Id,
                p.Number,
                p.Price,
                p.Status
            }).ToList();
            var count = _otherCouponService.GetCouponCount(condition);
            return PageHelper.toJson(new { List = coupon, TotalCount = count, Condition = condition });
        }
        #endregion
        #region 彭贵飞 显示优惠券信息
        /// <summary>
        /// 显示单个优惠券信息
        /// </summary>
        /// <param name="id">优惠券Id</param>
        /// <returns>优惠券对象</returns>
        [HttpGet]
        public HttpResponseMessage Detailed(int id)
        {
            var coupon = _otherCouponService.GetCouponById(id);
            return PageHelper.toJson(coupon);
        }
        #endregion
        #region 彭贵飞 单个插入优惠券
        /// <summary>
        /// 单个新建
        /// </summary>
        /// <param name="model">优惠券参数</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Create(OtherCouponModel model)
        {
            Random random = new Random();
            int num = random.Next(10000000, 100000000);
            var coupon = new global::Event.Entity.Entity.OtherCoupon.OtherCoupon
            {
                Number = num.ToString(),
                Price = model.Price,
                Status = model.Status,
                CouponCategoryId = model.CouponCategoryId
            };
            if (_otherCouponService.Create(coupon) != null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败"));
        }
        #endregion
        #region 彭贵飞 批量插入优惠券
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="model">优惠券参数</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage BlukCreate(OtherCouponModel model)
        {
            Random random = new Random();
            List<global::Event.Entity.Entity.OtherCoupon.OtherCoupon> list = new List<global::Event.Entity.Entity.OtherCoupon.OtherCoupon>();
            for (int i = 0; i < model.Count; i++)
            {
                list.Add(new global::Event.Entity.Entity.OtherCoupon.OtherCoupon
                {
                    Number = DateTime.Now.ToString("HHmmss") + random.Next(10000000, 100000000),
                    Price = model.Price,
                    Status = model.Status,
                    CouponCategoryId = model.CouponCategoryId
                });
            }
            if (_otherCouponService.BulkCreate(list))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败"));
        }
        #endregion
        #region 彭贵飞 编辑优惠券
        /// <summary>
        /// 编辑优惠券
        /// </summary>
        /// <param name="model">优惠券参数</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Edit(OtherCouponModel model)
        {
            var coupon = _otherCouponService.GetCouponById(model.Id);
            coupon.Number = model.Number;
            coupon.Price = model.Price;
            coupon.Status = model.Status;
            coupon.CouponCategoryId = model.CouponCategoryId;
            if (_otherCouponService.Update(coupon) != null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败"));
        }
        #endregion
        #region 彭贵飞 删除优惠券
        /// <summary>
        /// 删除优惠券
        /// </summary>
        /// <param name="id">优惠券Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Delete(int id)
        {
            var coupon = _otherCouponService.GetCouponById(id);
            if (_otherCouponService.Delete(coupon))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败"));
        }
        #endregion
    }
}
