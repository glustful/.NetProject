using CRM.Entity.Model;
using CRM.Service.BankCard;
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
    /// 银行卡管理  李洪亮  2015-05-05
    /// </summary>
    public class BankCardController : ApiController
    {
        private readonly IBankCardService _bankcardService;
        private readonly IBrokerService _brokerService;//经纪人
        public BankCardController(IBankCardService bankcardService, IBrokerService brokerService)
        {
             _bankcardService=bankcardService;
             _brokerService = brokerService;
        }

        #region 银行卡管理

        /// <summary>
        /// 通过经纪人ID 查询该经纪人的银行卡列表
        /// </summary>
        /// <param name="userID">经纪人ID</param>
        /// <returns></returns>
        public HttpResponseMessage SearchBankCardsByUserID(string userID)
        {
            if(string.IsNullOrEmpty(userID) || !PageHelper.ValidateNumber(userID))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！")); 
            }

           var bankcardSearchCon=new BankCardSearchCondition
           {
                 Brokers=_brokerService.GetBrokerById(Convert.ToInt32(userID))
           };
           return PageHelper.toJson(
           _bankcardService.GetBankCardsByCondition(bankcardSearchCon).ToList());
        }


        /// <summary>
        /// 添加银行卡
        /// </summary>
        /// <param name="bankcard"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddBankCard([FromBody] BankCardEntity  bankcard)
        {
            var entity=new BankCardEntity{
                 Addtime=DateTime.Now,
                 Uptime=DateTime.Now,
                 Bank=null,
                 Broker=null,
                 Num=bankcard.Num,
                 Deadline=bankcard.Deadline
            };

              try
                {
                    if (_bankcardService.Create(entity) != null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！"));
                    }
                }
                catch
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
                }
              return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));

        }
        #endregion
    }
}
