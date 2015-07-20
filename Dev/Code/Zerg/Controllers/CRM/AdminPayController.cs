using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.BrokerWithdrawDetail;
using CRM.Service.BrokerWithdraw;
using CRM.Service.BrokeAccount;
using CRM.Service.BRECPay;
using CRM.Service.BrokerRECClient;
using CRM.Service.BLPay;
using CRM.Service.BrokerLeadClient;
using Zerg.Common;
using Zerg.Models.CRM;
using System.ComponentModel;
using YooPoon.WebFramework.User.Entity;
using CRM.Service.Broker;
using YooPoon.Core.Site;

//财务人员打款流程处理
namespace Zerg.Controllers.CRM
{
      [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [Description("财务人员打款流程处理类")]
    public class AdminPayController : ApiController
    {
        private IBrokeAccountService _brokerAcountService;
        private IBrokerWithdrawService _brokerwithdrawService;
        private readonly IBrokerWithdrawDetailService _brokerwithdrawDetailService;
        private readonly IBRECPayService _brecPayService;
        private readonly IBrokerRECClientService _brokerRecClientService;
        private readonly IBLPayService _blPayService;
        private readonly IBrokerLeadClientService _brokerLeadClientService;
        private readonly IBrokerService _brokerService;    
        private readonly IWorkContext _workContext;

        
        /// <summary>
        /// 财务人员打款管理初始化
        /// </summary>
        /// <param name="brecPayService">brecPayService</param>
        /// <param name="brokerRecClientService">brokerRecClientService</param>
        public AdminPayController(IBRECPayService brecPayService,
            IBrokerWithdrawService brokerwithdrawService,
            IBrokerWithdrawDetailService brokerwithdrawDetailService,
            IBrokerRECClientService brokerRecClientService,
            IBLPayService blPayService,
            IBrokerLeadClientService brokerLeadClientService,
            IWorkContext workContext,
            IBrokerService brokerService,
            IBrokeAccountService brokerAcountService
            )
        {
            _brokerwithdrawService = brokerwithdrawService;
            _brokerwithdrawDetailService = brokerwithdrawDetailService;
            _brecPayService = brecPayService;
            _brokerRecClientService = brokerRecClientService;
            _blPayService = blPayService;
            _brokerLeadClientService = brokerLeadClientService;
            _workContext = workContext;
            _brokerService = brokerService;
            _brokerAcountService = brokerAcountService;
        }

        #region 财务打款确认流程 杨定鹏 2015年5月19日10:24:34
        /// <summary>
        /// 传入财务管理员参数,财务管理员打款,返回打款结果状态信息,成功返回"添加成功"
        /// </summary>
        /// <param name="adminPayModel">财务管理员参数</param>
        /// <returns>财务管理员打款结果状态信息</returns>
        [HttpPost]
        [Description("财务管理员打款")]
        public HttpResponseMessage SetBREPay([FromBody]AdminPayModel adminPayModel)
        {
            if (string.IsNullOrEmpty(adminPayModel.Name) || adminPayModel.BankCard == 0 && adminPayModel.Amount == 0)
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不能为空"));

            var model = new BRECPayEntity
            {
                //BrokerRECClient = _brokerRecClientService.GetBrokerRECClientById(adminPayModel.Id),
                Name = adminPayModel.Name,
                Statusname = adminPayModel.Statusname,
                Describe = adminPayModel.Describe,
                Amount = adminPayModel.Amount,
                BankCard = adminPayModel.BankCard,

                Accountantid = adminPayModel.Accountantid,
                Adduser = adminPayModel.Adduser,
                Addtime = DateTime.Now,
                Upuser = adminPayModel.Upuser,
                Uptime = DateTime.Now
            };
            _brecPayService.Create(model);

            return PageHelper.toJson(PageHelper.ReturnValue(true, "添加成功"));
        }
        /// <summary>
        /// 传入财务管理员参数,财务管理员打款,返回打款结果状态信息,成功返回"添加成功"
        /// </summary>
        /// <param name="adminPayModel">财务管理员参数</param>
        /// <returns>财务管理员打款结果状态信息</returns>
        [HttpPost]
        [Description("财务管理员打款")]
        public HttpResponseMessage SetBLPay([FromBody]BrokerLeadClientPay leadClientPay)
        {
            if (string.IsNullOrEmpty(leadClientPay.Name) || leadClientPay.BankCard == 0 && leadClientPay.Amount == 0)
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不能为空"));

            var model = new BLPayEntity
            {
                BrokerLeadClient = _brokerLeadClientService.GetBrokerLeadClientById(leadClientPay.Id),
                Name = leadClientPay.Name,
                Statusname = leadClientPay.Statusname,
                Describe = leadClientPay.Describe,
                Amount = leadClientPay.Amount,
                BankCard = leadClientPay.BankCard,
                Accountantid = leadClientPay.Accountantid,
                Adduser = leadClientPay.Adduser,
                Addtime = DateTime.Now,
                Upuser = leadClientPay.Upuser,
                Uptime = DateTime.Now
            };
            _blPayService.Create(model);
            return PageHelper.toJson(PageHelper.ReturnValue(true, "添加成功"));
        }

        /// <summary>
        /// 传入财务管理员参数,修改打款流程,返回修改结果
        /// </summary>
        /// <param name="adminPayModel">财务管理员参数</param>
        /// <returns>打款流程修改结果状态信息</returns>
        [HttpPost]
        [Description("财务管理员打款流程修改")]
        public HttpResponseMessage ModifyBREPay([FromBody]AdminPayModel adminPayModel)
        {
            if (adminPayModel.Id == 0 && adminPayModel.Amount == 0)
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不能为空"));

            var model = _brecPayService.GetBRECPayById(adminPayModel.Id);
            model.BrokerRECClient = _brokerRecClientService.GetBrokerRECClientById(adminPayModel.Id);
            model.Name = adminPayModel.Name;
            model.Statusname = adminPayModel.Statusname;
            model.Describe = adminPayModel.Describe;
            model.Amount = adminPayModel.Amount;
            model.Accountantid = adminPayModel.Accountantid;
            model.Adduser = adminPayModel.Adduser;
            model.Addtime = DateTime.Now;
            model.Upuser = adminPayModel.Upuser;
            model.Uptime = adminPayModel.Uptime;

            _brecPayService.Create(model);

            return PageHelper.toJson(PageHelper.ReturnValue(true, "修改成功"));
        }


        /// <summary>
        /// 传入财务管理员参数,修改打款流程,返回修改结果
        /// </summary>
        /// <param name="leadClientPay">财务管理员参数</param>
        /// <returns>打款流程修改结果状态信息</returns>
        [HttpPost]
        [Description("财务管理员打款流程修改")]
        public HttpResponseMessage ModifyBLPay([FromBody]BrokerLeadClientPay leadClientPay)
        {
            if (leadClientPay.Id == 0 && leadClientPay.Amount == 0)
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不能为空"));
            var model = _blPayService.GetBLPayById(leadClientPay.Id);
            model.BrokerLeadClient = _brokerLeadClientService.GetBrokerLeadClientById(leadClientPay.Id);
            model.Name = leadClientPay.Name;
            model.Statusname = leadClientPay.Statusname;
            model.Describe = leadClientPay.Describe;
            model.Amount = leadClientPay.Amount;
            model.Accountantid = leadClientPay.Accountantid;
            model.Adduser = leadClientPay.Adduser;
            model.Addtime = DateTime.Now;
            model.Upuser = leadClientPay.Upuser;
            model.Uptime = leadClientPay.Uptime;
            _blPayService.Create(model);

            return PageHelper.toJson(PageHelper.ReturnValue(true, "修改成功"));
        }

        #endregion
        #region chenda  财务确认打款
        /// <summary>
          /// chenda  财务打款
          /// </summary>
          /// <param name="payModel"></param>
          /// <returns></returns>
        [HttpPost]
        [Description("财务管理员打款")]
        public HttpResponseMessage SetPay([FromBody]PayModel payModel)
        {
            var user = (UserBase) _workContext.CurrentUser;
            var broker = new BrokerEntity { };
            var BrokeAccount = new BrokeAccountEntity { };
            var BrokerWithdraw = new BrokerWithdrawEntity { };
            if (user != null)
            {
                broker = _brokerService.GetBrokerByUserId(user.Id); //获取当前经纪人
                if (broker == null)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
                }
            }

            if (string.IsNullOrEmpty(payModel.Id))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不能为空"));
            }
            BrokerWithdraw = _brokerwithdrawService.GetBrokerWithdrawById(Convert.ToInt32(payModel.Id));
            if (BrokerWithdraw.State == 1) 
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "财务已经打款"));
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////
            if (string.IsNullOrEmpty(payModel.Ids)) 
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不能为空"));
            }
            if (string.IsNullOrEmpty(payModel.BrokeAccountId))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false,"数据不能为空"));
            }
            string[] strBrokeAccountId = payModel.BrokeAccountId.Split(',');
            foreach (var BrokeAccountId in strBrokeAccountId)
            {
                if (string.IsNullOrEmpty(BrokeAccountId))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据错误"));
                }
                BrokeAccount = _brokerAcountService.GetBrokeAccountById(Convert.ToInt32(BrokeAccountId));
                if (BrokeAccount.State == 1) 
                {
                    break;
                }
            }
            string[] strIds = payModel.Ids.Split(',');
            foreach (var id in strIds)
            {
                if(string.IsNullOrEmpty(id))
                {
                    break;
                }
                var model = _brokerwithdrawDetailService.GetBrokerWithdrawDetailById(Convert.ToInt32(id));
                if (Convert.ToInt32(model.Type) == 0) 
                {
                    var blModel = new BLPayEntity
                    {
                        Name = payModel.Name,
                        Describe = payModel.Describe,
                        BankCard = Convert.ToInt32(model.BankCard.Num),
                        Accountantid = broker.Id,
                        Amount = model.Withdrawnum,
                        Adduser = broker.Id,
                        Upuser = broker.Id,
                        Addtime = DateTime.Now,
                        Uptime = DateTime.Now,
                    };
                    _blPayService.Create(blModel);
                }
                if (Convert.ToInt32(model.Type) == 1) 
                {
                    var breModel = new BRECPayEntity
                    {
                        Name = payModel.Name,
                        Describe = payModel.Describe,
                        BankCard = Convert.ToInt32(model.BankCard.Num),
                        Accountantid = broker.Id,
                        Amount = model.Withdrawnum,
                        Adduser = broker.Id,
                        Upuser = broker.Id,
                        Addtime = DateTime.Now,
                        Uptime = DateTime.Now, 
                    };
                    _brecPayService.Create(breModel);
                }
               
            }
            BrokerWithdraw.State = 1;
            BrokerWithdraw.AccAccountantId = broker;
            BrokerWithdraw.Uptime = DateTime.Now;
            BrokerWithdraw.Upuser = broker.Id;
            BrokerWithdraw.WithdrawDesc = payModel.Describe;
            _brokerwithdrawService.Update(BrokerWithdraw);
            BrokeAccount.State = 1;
            BrokeAccount.Uptime = DateTime.Now;
            BrokeAccount.Upuser = broker.Id;
            _brokerAcountService.Update(BrokeAccount);
            return PageHelper.toJson(PageHelper.ReturnValue(true, "打款成功"));
        }
        #endregion
    }
    /// <summary>
    /// 打款实体
    /// </summary>
      public class PayModel 
      {
          /// <summary>
          /// 提现ID
          /// </summary>
          public string Id { get; set; }
          /// <summary>
          /// 提现明细Id
          /// </summary>
          public string Ids { get; set; }
          /// <summary>
          /// 提现账户明细ID
          /// </summary>
          public string BrokeAccountId { get; set; }
          /// <summary>
          /// 描述
          /// </summary>
          public string Describe { get; set; }
          /// <summary>
          /// 款项名称
          /// </summary>
          public string Name { get; set; }
          /// <summary>
          /// 财务ID
          /// </summary>
          public string  Accountantid{get;set;}
          public string   Upuser{get;set;}
          public string Adduser { get; set; }
      }
}