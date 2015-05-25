using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.BRECPay;
using CRM.Service.BrokerRECClient;
using Zerg.Common;
using Zerg.Models.CRM;

//财务人员打款流程处理
namespace Zerg.Controllers.CRM
{
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    public class AdminPayController : ApiController
    {
        private readonly IBRECPayService _brecPayService;
        private readonly IBrokerRECClientService _brokerRecClientService;
        public AdminPayController(IBRECPayService brecPayService,
            IBrokerRECClientService brokerRecClientService
            )
        {
            _brecPayService = brecPayService;
            _brokerRecClientService = brokerRecClientService;
        }

        #region 财务打款确认流程 杨定鹏 2015年5月19日10:24:34
        /// <summary>
        /// 确认打款
        /// </summary>
        /// <param name="adminPayModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SetPay([FromBody]AdminPayModel adminPayModel)
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
        /// 修改打款流程
        /// </summary>
        /// <param name="adminPayModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage ModifyPay([FromBody]AdminPayModel adminPayModel)
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

        #endregion
    }
}
