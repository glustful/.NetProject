﻿using CRM.Entity.Model;
using CRM.Service.MerchantInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Zerg.Common;
using Zerg.Models.CRM;

namespace Zerg.Controllers.CRM
{
    /// <summary>
    /// 商家管理
    /// </summary>
    public class MerchantInfoController : ApiController
    {
        IMerchantInfoService _merchantInfoService;
        public MerchantInfoController(MerchantInfoService merchantInfoService)
        {
            _merchantInfoService = merchantInfoService;
        }
        #region 商家信息管理 黄秀宇 2015.04.30
        /// <summary>
        /// 新增商家
        /// </summary>
        /// <param name="merchantInfoModel"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddMerchantInfo(MerchantInfoModel merchantInfoModel )
        {
            if (string.IsNullOrEmpty(merchantInfoModel.Merchantname) && string.IsNullOrEmpty(merchantInfoModel.Mail))
            {
                if (!PageHelper.IsEmail(merchantInfoModel.Mail))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "Email格式验证错误！"));
                }
                var merchantInfoEntity = new MerchantInfoEntity()
                {
                    Merchantname = merchantInfoModel.Merchantname,
                    Mail = merchantInfoModel.Mail
                };
                try
                {
                    if (_merchantInfoService.Create(merchantInfoEntity) != null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！"));
                    }
                }
                catch {
                    return PageHelper .toJson (PageHelper .ReturnValue (false ,"数据添加失败！"));
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }
        /// <summary>
        /// 撤掉商家
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DeleteMerchantInfo(string id)
        {
            if (string.IsNullOrEmpty(id) && PageHelper.ValidateNumber(id))
            {
                if (_merchantInfoService.Delete(_merchantInfoService.GetMerchantInfoById(Convert.ToInt32(id))))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据成功删除！"));
                }
                else
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败！"));
                }
            }
            return  PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }
        /// <summary>
        /// 查找商家
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="PageCount"></param>
        /// <param name="isDescending"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]

        public HttpResponseMessage SearchMerchantInfo(MerchantInfoModel merchantInfoModel)
        {

            var merchantInfo = new MerchantInfoSearchCondition()
            {
                Page = merchantInfoModel.Page,
                PageCount = merchantInfoModel.PageCount,
                isDescending = merchantInfoModel.isDescending

            };
          
            return PageHelper.toJson(_merchantInfoService.GetMerchantInfosByCondition(merchantInfo).ToList ());

        }
        /// <summary>
        /// 修改商家信息
        /// </summary>
        /// <param name="merchantInfoModel"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateMerchantInfo(MerchantInfoModel merchantInfoModel)
        {
            if (!string.IsNullOrEmpty(merchantInfoModel.Id) && PageHelper.ValidateNumber(merchantInfoModel.Id) && !string.IsNullOrEmpty(merchantInfoModel.Merchantname) && !string.IsNullOrEmpty( merchantInfoModel.Mail))
            {
                if (!PageHelper.IsEmail(merchantInfoModel.Mail))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "Email格式验证错误！"));
                }
                var merchantInfo = _merchantInfoService.GetMerchantInfoById(Convert.ToInt32(merchantInfoModel.Id));
                merchantInfo.Uptime = DateTime.Now;
                merchantInfo.Merchantname = merchantInfoModel.Merchantname;
                merchantInfo.Mail = merchantInfoModel.Mail;

                try
                {
                    if (_merchantInfoService.Update(merchantInfo) != null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功！"));
                    }
                }
                catch
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败！"));
                }


            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }
        #endregion
        #region 注销、查找商家详情 黄秀宇 2015.05.04
        /// <summary>
        /// 注销商家
        /// </summary>
        /// <param name="merchantInfoModel"></param>
        /// <returns></returns>
        [System.Web .Http .HttpPost ]
        public HttpResponseMessage LogoffMerchantInfo(MerchantInfoModel merchantInfoModel)
        {
            if (!string.IsNullOrEmpty(merchantInfoModel.Id))
            {
                var mInfoModel = _merchantInfoService.GetMerchantInfoById(Convert.ToInt32(merchantInfoModel.Id));
                mInfoModel.Id = Convert.ToInt32(merchantInfoModel.Id);
                mInfoModel.State = false;
                try
                {
                    if (_merchantInfoService.Update(mInfoModel) != null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "注销成功！"));
                    }
                }
                catch
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "注销失败！"));
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }
        /// <summary>
        /// 查找商家详情
        /// </summary>
        /// <param name="merchantInfoModel"></param>
        /// <returns></returns>
        [System .Web .Http .HttpPost ]
        public HttpResponseMessage  SearchDetailMerchantInfo(MerchantInfoModel merchantInfoModel)
        {
            if(!string .IsNullOrEmpty(merchantInfoModel .Id  )){
                var mInfoCondition=new MerchantInfoSearchCondition(){
                   OrderBy =EnumMerchantInfoSearchOrderBy.OrderById
                };
           
            return PageHelper.toJson(_merchantInfoService.GetMerchantInfosByCondition(mInfoCondition).Where(p=>p.Id==Convert.ToInt32(merchantInfoModel.Id)).First());
        }
          return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }
        #endregion
    }
}