﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Event.Entity.Entity.Coupon;
using Event.Entity.Entity.OtherCoupon;
using Event.Service.Coupon;
using Event.Service.OtherCoupon;
using Trading.Service.ProductBrand;
using Zerg.Common;
using Zerg.Event.Models.Coupons;

namespace Zerg.Event.API.OtherCoupon
{
    public class OtherCouponCategoryController : ApiController
    {
        private readonly IOtherCouponCategoryService _otherCouponCategoryService;
        private readonly IProductBrandService _productBrandService;

        public OtherCouponCategoryController(IOtherCouponCategoryService otherCouponCategoryService, IProductBrandService productBrandService)
        {
            _otherCouponCategoryService = otherCouponCategoryService;
            _productBrandService = productBrandService;         
        }
        #region 彭贵飞 获取优惠券种类列表
        /// <summary>
        /// 查询优惠券种类列表
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="pageSize">页面记录数</param>
        /// <param name="name">优惠券名称</param>
        /// <returns>list列表</returns>
        [HttpGet]
        public HttpResponseMessage Index(int page, int pageSize, string name = null)
        {
            var condition = new OtherCouponCategorySearchCondition
            {
                Name = name,
                Page = page,
                PageSize = pageSize,
                OrderBy = EnumOtherCouponCategorySearchOrderBy.OrderById
            };
            var couponCategory = _otherCouponCategoryService.GetCouponCategoriesByCondition(condition).Select(p => new
            {
                p.Id,
                p.Name,
                p.Count,
                p.Price,
                p.ReMark
            }).ToList();
            var count = _otherCouponCategoryService.GetCouponCategoriesCountByCondition(condition);
            return PageHelper.toJson(new { List = couponCategory, TotalCount = count, Condition = condition });
        }
        #endregion
        #region 彭贵飞 新建优惠券种类
        /// <summary>
        /// 新建优惠券种类
        /// </summary>
        /// <param name="model">优惠券种类参数</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Create(CategoryModel model)
        {
            var couponCategory = new OtherCouponCategory
            {
                Name = model.Name,
                Price = model.Price,
                BrandId = model.BrandId,
                Count = model.Count,
                ReMark = model.ReMark,
                ClassId = model.ClassId
            };
            if (_otherCouponCategoryService.CreateCouponCategory(couponCategory))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败"));
        }
        #endregion
        #region 彭贵飞 显示单个优惠券种类信息
        /// <summary>
        /// 查询单个优惠券种类信息
        /// </summary>
        /// <param name="id">优惠券种类Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Detailed(int id)
        {
            var couponCategory = _otherCouponCategoryService.GetCouponCategoryById(id);
            return PageHelper.toJson(couponCategory);
        }
        #endregion
        #region 彭贵飞 编辑优惠券种类
        /// <summary>
        /// 编辑优惠券种类
        /// </summary>
        /// <param name="model">优惠券参数</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Edit(CategoryModel model)
        {
            var couponCategory = _otherCouponCategoryService.GetCouponCategoryById(model.Id);
            couponCategory.Name = model.Name;
            couponCategory.Count = model.Count;
            couponCategory.Price = model.Price;
            couponCategory.ReMark = model.ReMark;
            couponCategory.BrandId = model.BrandId;
            if (_otherCouponCategoryService.UpdateCouponCategory(couponCategory))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据修改成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据修改失败"));
        }
        #endregion
        #region 彭贵飞 删除优惠券种类
        /// <summary>
        /// 删除优惠券种类
        /// </summary>
        /// <param name="id">种类Id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Delete(int id)
        {
            if (_otherCouponCategoryService.DeleteCouponCategory(id))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败"));
        }
        #endregion
        #region 彭贵飞 获取手机端显示的优惠券种类和对应品牌信息接口
        /// <summary>
        /// 获取优惠券种类
        /// </summary>
        /// <returns>list列表</returns>
        public HttpResponseMessage Coupons()
        {
            var Con = new OtherCouponCategorySearchCondition
            {
                OrderBy = EnumOtherCouponCategorySearchOrderBy.OrderById
            };
            var list = _otherCouponCategoryService.GetCouponCategoriesByCondition(Con).Select(p => new
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
        #endregion
    }
}
