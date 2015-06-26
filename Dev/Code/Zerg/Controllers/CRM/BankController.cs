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
using System.ComponentModel;

namespace Zerg.Controllers.CRM
{

    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// 银行管理  李洪亮  2015-05-05
    /// </summary>
    [Description("银行管理类")]
    public class BankController : ApiController
    {
        private readonly IBankService _bankService;
        private readonly IBrokerService _brokerService;//经纪人
        /// <summary>
        /// 银行管理初始化
        /// </summary>
        /// <param name="bankService">bankService</param>
        /// <param name="brokerService">brokerService</param>
        public BankController(IBankService bankService, IBrokerService brokerService)
        {
            _bankService = bankService;
            _brokerService = brokerService;
        }

        #region 银行设置

        /// <summary>
        /// 传入页面设置参数,检索银行,返回银行列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面数量</param>
        /// <returns>银行列表</returns>
        [System.Web.Http.HttpGet]
        [Description("获取银行列表")]
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
        ///获取所有银行,返回银行列表
        /// </summary>
        /// <returns>银行列表</returns>
        [System.Web.Http.HttpGet]
        [Description("获取银行列表")]
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
        /// 检索银行信息,返回银行详细信息
        /// </summary>
        /// <param name="id">银行ID</param>
        /// <returns>银行详细信息</returns>
        [HttpGet]
        [Description("获取一条银行详细信息")]
        public HttpResponseMessage GetBank(string id)
        {
            return PageHelper.toJson(_bankService.GetBankById(Convert.ToInt32(id)));
        }




        /// <summary>
        /// 传入银行参数,添加一家银行,返回状态信息,成功返回"数据添加成功",失败返回"数据添加失败"
        /// </summary>
        /// <param name="bankcard">银行参数</param>
        /// <returns>添加银行结果状态信息</returns>
        [System.Web.Http.HttpPost]
        [Description("传入参数,添加一家银行")]
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
        /// 传入银行信息,编辑修改银行信息,返回编辑修改结果状态信息,成功返回"数据更新成功",失败提示"数据更新失败"
        /// </summary>
        /// <param name="bank">银行参数</param>
        /// <returns>银行信息更新结果状态信息</returns>

        [System.Web.Http.HttpPost]
        [Description("更新银行信息")]
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
