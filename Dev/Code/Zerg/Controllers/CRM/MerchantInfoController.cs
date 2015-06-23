using CRM.Entity.Model;
using CRM.Service.MerchantInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Zerg.Common;
using Zerg.Models.CRM;
using System.ComponentModel;

namespace Zerg.Controllers.CRM
{
    // 商家管理
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [Description("商家管理类")]
    public class MerchantInfoController : ApiController
    {
        private readonly IMerchantInfoService _merchantInfoService;
        public MerchantInfoController(IMerchantInfoService merchantInfoService)
        {
            _merchantInfoService = merchantInfoService;
        }

        #region 商家信息管理 黄秀宇 2015.04.30


        /// <summary>
        /// 传入商家参数，新增商家，返回新增商家结果状态信息
        /// </summary>
        /// <param name="merchantInfoModel">商家参数</param>
        /// <returns>新增商家结果状态信息</returns>
     
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddMerchantInfo(MerchantInfoModel merchantInfoModel)
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
                catch
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }


        /// <summary>
        /// 传入商家ID，撤掉商家，返回撤掉商家结果状态信息
        /// </summary>
        /// <param name="id">商家ID</param>
        /// <returns>撤掉商家结果状态信息</returns>
         
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
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }


        /// <summary>
        /// 传入商家名称，查询商家，返回新增商家列表
        /// </summary>
        /// <param name="Page">页码</param>
        /// <param name="PageCount">页面数量</param>
        /// <param name="isDescending">是否降序排列</param>
        /// <returns>商家列表</returns>
        
        [System.Web.Http.HttpGet]

        public HttpResponseMessage SearchMerchantInfo(string name = null, int page = 1, int pageSize = 10)
        {

            var merchantInfo = new MerchantInfoSearchCondition()
            {
                Page = page,
                PageCount = pageSize,
                Merchantname = name

            };
            var merchantList = _merchantInfoService.GetMerchantInfosByCondition(merchantInfo).ToList();
            var merchantListCount = _merchantInfoService.GetMerchantInfoCount(merchantInfo);
            return PageHelper.toJson(new { List = merchantList, Condition = merchantInfo, totalCount = merchantListCount });

        }




        /// <summary>
        /// 传入商家参数，修改商家，返回修改商家结果状态信息
        /// </summary>
        /// <param name="merchantInfoModel">商家参数</param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateMerchantInfo(MerchantInfoModel merchantInfoModel)
        {
            if (!string.IsNullOrEmpty(merchantInfoModel.Id) && PageHelper.ValidateNumber(merchantInfoModel.Id) && !string.IsNullOrEmpty(merchantInfoModel.Merchantname) && !string.IsNullOrEmpty(merchantInfoModel.Mail))
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



        /// <summary>
        /// 传入商家参数，注销商家，返回注销商家结果状态信息
        /// </summary>
        /// <param name="merchantInfoModel">商家参数</param>
        /// <returns>注销商家结果状态信息</returns>
        [System.Web.Http.HttpPost]
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
        /// 传入商家ID，查找商家详情，返回商家详细信息
        /// </summary>
        /// <param name="merchantInfoModel">商家ID</param>
        /// <returns>商家详细信息</returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetDetailMerchantInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
            };

            return PageHelper.toJson(_merchantInfoService.GetMerchantInfoById(Convert.ToInt32(id)));


        }

        #endregion
    }
}
