﻿using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.BrokerWithdrawDetail;
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
    /// 经纪人提现明细  李洪亮  2015-05-05
    /// </summary>
    public class BrokerWithdrawDetailController : ApiController
    {
        private IBrokerWithdrawDetailService _brokerwithdrawdetailService;
        private  IBrokerService _brokerService;
        public BrokerWithdrawDetailController(IBrokerWithdrawDetailService brokerwithdrawdetailService, IBrokerService brokerService)
        {
            _brokerwithdrawdetailService =brokerwithdrawdetailService;
            _brokerService =brokerService;
        }

        #region 经纪人提现明细详情

      /// <summary>
      /// 查询经纪人的提现明细详情List
      /// </summary>
      /// <param name="userId">经纪人ID</param>
      /// <returns></returns>

        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetBrokerWithdrawDetailListByUserId(string userId)
        {
            var brokerwithdrawdetailcon = new BrokerWithdrawDetailSearchCondition
            {
               Brokers=_brokerService.GetBrokerById(Convert.ToInt32(userId))
            };
            return PageHelper.toJson(_brokerwithdrawdetailService.GetBrokerWithdrawDetailsByCondition(brokerwithdrawdetailcon).ToList());
        }

        /// <summary>
        /// 新增提现详情
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddBrokerWithdrawDetail([FromBody] BrokerWithdrawDetailEntity brokerwithDetail)
        {
            if (brokerwithDetail != null)
            {
                var entity = new BrokerWithdrawDetailEntity
                {
                    BankCard=null,
                    Withdrawnum=brokerwithDetail.Withdrawnum,
                    Withdrawtime=DateTime.Now,
                    Broker=null,
                    Uptime = DateTime.Now,
                    Addtime = DateTime.Now,
                };

                try
                {
                    if (_brokerwithdrawdetailService.Create(entity) != null)
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