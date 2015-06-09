using CRM.Entity.Model;
using CRM.Service.Bank;
using CRM.Service.Broker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Zerg.Common;

namespace Zerg.Controllers.CRM
{

    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// 银行管理  李洪亮  2015-05-05
    /// </summary>
    public class BankController : ApiController
    {
        private readonly IBankService _bankService;
        private readonly IBrokerService _brokerService;//经纪人
        public BankController(IBankService bankService, IBrokerService brokerService)
        {
            _bankService = bankService;
            _brokerService = brokerService;
        }

        #region 银行设置

        /// <summary>
        ///获取所有银行
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage SearchBanks(int page = 1, int pageSize = 10)
        {


            var bankSearchCon = new BankSearchCondition
            {

                Page = Convert.ToInt32(page),
                PageCount = pageSize
            };

            var bankList = _bankService.GetBanksByCondition(bankSearchCon).Select(p => new
            {
                Id = p.Id,
                p.Codeid,
                p.Addtime

            }).ToList();
            var bankListCount = _bankService.GetBankCount(bankSearchCon);
            return PageHelper.toJson(new { List = bankList, Condition = bankSearchCon, totalCount = bankListCount });

        }



        /// <summary>
        ///获取所有银行
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage SearchAllBank()
        {
           var bankSearchCon = new BankSearchCondition
            {            
            };
            var bankList = _bankService.GetBanksByCondition(bankSearchCon).Select(p => new
            {
                Id = p.Id,
                p.Codeid,
                p.Addtime

            }).ToList();
            var bankListCount = _bankService.GetBankCount(bankSearchCon);
            return PageHelper.toJson(new { List = bankList });
        }





        /// <summary>
        /// 获取一条银行信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetBank(string id)
        {
            return PageHelper.toJson(_bankService.GetBankById(Convert.ToInt32(id)));
        }




        /// <summary>
        /// 添加银行
        /// </summary>
        /// <param name="bankcard"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddBank([FromBody] BankEntity bank)
        {
            var entity = new BankEntity
            {
                Addtime = DateTime.Now,
                Codeid = bank.Codeid
            };

            try
            {
                if (_bankService.Create(entity) != null)
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


        /// <summary>
        /// 更新
        /// </summary>

        [System.Web.Http.HttpPost]
        public HttpResponseMessage Update(BankEntity bank)
        {
            if (PageHelper.ValidateNumber(bank.Id.ToString()) && !string.IsNullOrEmpty(bank.Codeid))
            {

                var BankEntity = _bankService.GetBankById(Convert.ToInt32(bank.Id));

                BankEntity.Codeid = bank.Codeid;

                try
                {
                    if (_bankService.Update(BankEntity) != null)
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
    }
}
