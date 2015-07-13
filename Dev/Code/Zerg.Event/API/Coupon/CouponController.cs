using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Event.Entity.Entity.Coupon;
using Event.Service.Coupon;
using Trading.Service.ProductBrand;
using YooPoon.WebFramework.User.Services;
using Zerg.Common;
using Zerg.Event.Models.Coupons;

namespace Zerg.Event.API.Coupon
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    public class CouponController : ApiController
    {
        private readonly ICouponService _couponService;
        private readonly IUserService _userService;
        private readonly ICouponOwnerService _couponownerService;
        private readonly ICouponCategoryService _couponCategoryService;
        private readonly IProductBrandService _productBrandService;


        public CouponController(ICouponService couponService, IUserService userService, ICouponOwnerService couponownerService,ICouponCategoryService couponCategoryService, IProductBrandService productBrandService)
        {
            _couponService = couponService;
            _userService = userService;
            _couponownerService = couponownerService;
            _couponCategoryService = couponCategoryService;
            _productBrandService = productBrandService;
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
            else if (entity.Status == EnumCouponStatus.Actived)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "激活失败！该优惠券已经激活"));
            }
            entity.Status = EnumCouponStatus.Actived;
            _couponService.Update(entity);
            return PageHelper.toJson(PageHelper.ReturnValue(true, "激活成功！"));
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
            var condition = new CouponSearchCondition
            {
                Number = number,
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
            var coupon = _couponService.GetCouponById(id);
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
        public HttpResponseMessage Create(CouponModel model)
        {
            Random random = new Random();
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
        #endregion
        #region 彭贵飞 批量插入优惠券
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="model">优惠券参数</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage BlukCreate(CouponModel model)
        {
            Random random = new Random();
            List<global::Event.Entity.Entity.Coupon.Coupon> list = new List<global::Event.Entity.Entity.Coupon.Coupon>();
            for (int i = 0; i < model.Count; i++)
            {
                list.Add(new global::Event.Entity.Entity.Coupon.Coupon
                {
                    Number = DateTime.Now.ToString("HHmmss") + random.Next(10000000, 100000000),
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
        #endregion
        #region 彭贵飞 编辑优惠券
        /// <summary>
        /// 编辑优惠券
        /// </summary>
        /// <param name="model">优惠券参数</param>
        /// <returns></returns>
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
            var coupon = _couponService.GetCouponById(id);
            if (_couponService.Delete(coupon))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败"));
        }
        #endregion
        #region  获取该用户所有的优惠劵
        /// <summary>
        /// 获取该用户所有的优惠劵
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>list</returns>
        [HttpGet]
        public HttpResponseMessage GetUserAllCoupon(string username)
        {
            YooPoon.WebFramework.User.Entity.UserBase us = _userService.GetUserByName(username);
            if (us == null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "用户不存在！"));
            }
            else
            {
                //获取该用户Id对应的优惠卷的CouponId  
                var co = _couponownerService.GetCouponByUserId(us.Id);

                //var co = _couponownerService.GetCouponByUserId(us.Id).Select(p => new
                //{
                //    list = _couponService.GetCouponById(p.CouponId)
                //});

                List<global::Event.Entity.Entity.Coupon.Coupon> lists = new List<global::Event.Entity.Entity.Coupon.Coupon>();
                foreach (var p in co)
                {

                    lists.Add(_couponService.GetCouponById(p.CouponId));

                }
                List<ReturnCoupon> listReCoupon = new List<ReturnCoupon>();

                foreach (var z in lists)
                {
                    listReCoupon.Add(new ReturnCoupon
                    {
                        ComponCategoryName = _couponCategoryService.GetCouponCategoryById(z.CouponCategoryId).Name,
                        Number = z.Number,
                        Brandname = _productBrandService.GetProductBrandById(_couponCategoryService.GetCouponCategoryById(z.CouponCategoryId).BrandId).Bname,
                        Price = z.Price.ToString()
                    });
                }

                if (listReCoupon.Count <= 0)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "该用户没有优惠卷！"));
                }

                //if(co!=null)
                //{
                //    foreach(var p in co)
                //    {
                //    var m=  _couponService.GetCouponById(p.CouponId);

                //    }
                //}
                return PageHelper.toJson(new { list = listReCoupon });
            }
        }
        #endregion

    }

    public class ReturnCoupon
    {
        public string ComponCategoryName { get; set; }

        public string Price { get; set; }

        public string Brandname { get; set; }

        public string Number { get; set; }
    }

}
