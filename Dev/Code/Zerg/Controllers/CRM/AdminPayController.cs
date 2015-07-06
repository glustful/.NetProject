using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.BRECPay;
using CRM.Service.BrokerRECClient;
using CRM.Service.BLPay;
using CRM.Service.BrokerLeadClient;
using Zerg.Common;
using Zerg.Models.CRM;
using System.ComponentModel;

//财务人员打款流程处理
namespace Zerg.Controllers.CRM
{
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [Description("财务人员打款流程处理类")]
    public class AdminPayController : ApiController
    {
        private readonly IBRECPayService _brecPayService;
        private readonly IBrokerRECClientService _brokerRecClientService;
        private readonly IBLPayService _blPayService;
        private readonly IBrokerLeadClientService _brokerLeadClientService;
        /// <summary>
        /// 财务人员打款管理初始化
        /// </summary>
        /// <param name="brecPayService">brecPayService</param>
        /// <param name="brokerRecClientService">brokerRecClientService</param>
        public AdminPayController(IBRECPayService brecPayService,
            IBrokerRECClientService brokerRecClientService,
            IBLPayService blPayService,
            IBrokerLeadClientService brokerLeadClientService
            )
        {
            _brecPayService = brecPayService;
            _brokerRecClientService = brokerRecClientService;
            _blPayService = blPayService;
            _brokerLeadClientService = brokerLeadClientService;
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
                BrokerRECClient = _brokerRecClientService.GetBrokerRECClientById(adminPayModel.BrokerRECClientId),
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
                BrokerLeadClient = _brokerLeadClientService.GetBrokerLeadClientById(leadClientPay.BrokerLeadClientId),
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
            model.BrokerRECClient = _brokerRecClientService.GetBrokerRECClientById(adminPayModel.BrokerRECClientId);
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
            model.BrokerLeadClient = _brokerLeadClientService.GetBrokerLeadClientById(leadClientPay.BrokerLeadClientId);
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
    }
}
