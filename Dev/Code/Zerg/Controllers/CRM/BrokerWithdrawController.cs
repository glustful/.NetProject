using System;
using CRM.Entity.Model;
using CRM.Service.BankCard;
using CRM.Service.Broker;
using CRM.Service.BrokerWithdraw;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using YooPoon.Common.Encryption;
using YooPoon.Core.Site;
using YooPoon.WebFramework.User.Entity;
using Zerg.Common;
using System.ComponentModel;

namespace Zerg.Controllers.CRM
{
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [AllowAnonymous]
    /// <summary>
    /// 经纪人提现  chen  2015-07-09
    /// </summary>
    [Description("经纪人提现明细")]
    public class BrokerWithdrawController : ApiController
    {
        private IBrokerService _brokerService;
        private IBrokerWithdrawService _brokerwithdrawService;
        private readonly IWorkContext _workContext;
        private readonly IBankCardService _bankcardService;
     
        /// <summary>
        /// 经纪人提现初始化
        /// </summary>
        /// <param name="bankcardService">bankcardService</param>
        /// <param name="workContext">workContext</param>
        /// <param name="brokerwithdrawService">brokerwithdrawdetailService</param>
        /// <param name="brokerService">brokerService</param>
        public BrokerWithdrawController(IBankCardService bankcardService, IWorkContext workContext, IBrokerWithdrawService brokerwithdrawService, IBrokerService brokerService)
        {
            _brokerwithdrawService = brokerwithdrawService;
            _brokerService = brokerService;
            _workContext = workContext;
            _bankcardService = bankcardService;
        }
       
        
        #region   查询所有提现信息


        /// <summary>
        /// 根据经纪人ID查询经纪人提现信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Description("查询所有提现信息")]
        [HttpGet]
        public HttpResponseMessage GetBrokerWithdraw(int page = 1, int pageSize = 10)
        {
            var condition = new BrokerWithdrawSearchCondition
            {
                OrderBy = EnumBrokerWithdrawSearchOrderBy.State,
                Page = Convert.ToInt32(page),
                PageCount = pageSize
            };
            var list = _brokerwithdrawService.GetBrokerWithdrawsByCondition(condition).Select(p => new
            {
                Id = p.Id,
                bankname = p.BankCard.Bank.Codeid,
                banknumber = p.BankCard.Num,
                brokername = p.Broker.Brokername,
                withdrawnum = p.WithdrawTotalNum,
                state = p.State,
                accacount = p.AccAccountantId.Brokername,
                WithdrawTime = p.WithdrawTime
            }).ToList().Select(p => new
            {
                Id = p.Id,
                bankname = p.bankname,
                banknumber = p.banknumber,
                withdrawnum = p.withdrawnum,
                brokername = p.brokername,
                state = p.state,
                accacount = p.accacount,
                WithdrawTime = p.WithdrawTime.ToString("yyyy-MM-dd")
            });
            var Count = _brokerwithdrawService.GetBrokerWithdrawCount(condition);
            return PageHelper.toJson(new { List = list, Condition = condition, totalCount = Count });
        }
        #endregion
        
        
        [Description("根据ID查询")]
        [HttpGet]
        public HttpResponseMessage GetBrokerWithdrawById(int id)
        {
            if (id == 0)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据错误"));
            }
            var model = _brokerwithdrawService.GetBrokerWithdrawById(id);
            var NewModel = new ReturnModel
            {
                ID = model.Id.ToString(),
                Withdrawnum = model.WithdrawTotalNum,
                BankType = model.BankCard.Type,
                BankNum = model.BankCard.Num,
                BrokerName = model.Broker.Brokername,
            };
            return PageHelper.toJson(NewModel);
        }


        /// <summary>
        /// 查询用户所有的提现记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAllBrokerWithdrawByUser()
        {
              var user = (UserBase)_workContext.CurrentUser;
              if (user != null)
              {
                  var broker = _brokerService.GetBrokerByUserId(user.Id);//获取当前经纪人
                  if (broker == null)
                  {
                      return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
                  }
                    
            var condition = new BrokerWithdrawSearchCondition
            {             
                Brokers=broker
            };
             object list = null;
             if(_brokerwithdrawService.GetBrokerWithdrawCount(condition)>0)
             {
                 list = _brokerwithdrawService.GetBrokerWithdrawsByCondition(condition).Select(p => new
                 {
                     Id = p.Id,
                     bankname = p.BankCard.Bank.Codeid,
                     withdrawnum = p.WithdrawTotalNum,
                     state = p.State == 0 ? "处理中" : p.State == 1 ? "已打款" : "",
                     WithdrawTime = p.WithdrawTime
                 }).ToList();

             }            
                return PageHelper.toJson(new { List = list });

              }
              return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));     
        }


    }
    /// <summary>
    /// 返回实体
    /// </summary>
    public class ReturnModel
    {
        public virtual string ID { get; set; }
        public virtual string BankType { get; set; }
        public virtual string BankNum { get; set; }
        public virtual decimal Withdrawnum { get; set; }
        public virtual string BrokerName { get; set; }
    }
}