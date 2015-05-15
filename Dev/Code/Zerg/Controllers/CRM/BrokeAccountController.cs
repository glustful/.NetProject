﻿using CRM.Entity.Model;
using CRM.Service.BrokeAccount;
using CRM.Service.Broker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Zerg.Common;

namespace Zerg.Controllers.CRM
{
    /// <summary>
    /// 账户明细管理  李洪亮  2015-05-05
    /// </summary>
    public class BrokeAccountController : ApiController
    {
        private IBrokeAccountService _brokeaccountService;
        private  IBrokerService _brokerService;
        public BrokeAccountController(IBrokeAccountService brokeaccountService, IBrokerService brokerService)
        {
            _brokeaccountService = brokeaccountService;
            _brokerService =brokerService;
        }

        #region 经纪人账户明细详情

      /// <summary>
      /// 查询经纪人的积分详情
      /// </summary>
      /// <param name="userId">经纪人ID</param>
      /// <returns></returns>

        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetPointDetailByUserId(string userId)
        {
            var brokeaccountcon = new BrokeAccountSearchCondition
            {
               Brokers=_brokerService.GetBrokerById(Convert.ToInt32(userId))
            };
            return PageHelper.toJson(_brokeaccountService.GetBrokeAccountsByCondition(brokeaccountcon).ToList());
        }

        /// <summary>
        /// 新增积分详情
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddBrokeAccount([FromBody] BrokeAccountEntity brokeAccount)
        {

            if (brokeAccount != null)
            {
                var entity = new BrokeAccountEntity
                {
                    MoneyDesc=brokeAccount.MoneyDesc,
                    Balancenum=brokeAccount.Balancenum,
                    Broker=null,
                  
                    Uptime = DateTime.Now,
                    Addtime = DateTime.Now,

                };

                try
                {
                    if (_brokeaccountService.Create(entity) != null)
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



        #endregion


    }
}