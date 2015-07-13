using CRM.Entity.Model;
using CRM.Service.BrokeAccount;
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

    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// 账户明细管理  李洪亮  2015-05-05
    /// </summary>
    [Description("账户明细管理类")]
    public class BrokeAccountController : ApiController
    {
        private IBrokeAccountService _brokeaccountService;
        private  IBrokerService _brokerService;
        /// <summary>
        /// 账户明细管理初始化
        /// </summary>
        /// <param name="brokeaccountService">brokeaccountService</param>
        /// <param name="brokerService">brokerService</param>
        [Description("账户明细管理初始化构造器")]
        public BrokeAccountController(IBrokeAccountService brokeaccountService, IBrokerService brokerService)
        {
            _brokeaccountService = brokeaccountService;
            _brokerService =brokerService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Description("获取当前经纪人账户信息")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetBrokeAccountByUserId(string userId = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据错误！"));
            }

            var brokeaccountcon = new BrokeAccountSearchCondition
            {
                Brokers = _brokerService.GetBrokerById(Convert.ToInt32(userId)),
                State = 0
               
            };
            var PointDetailList = _brokeaccountService.GetBrokeAccountsByCondition(brokeaccountcon).Select(p => new
            {
                Id = p.Id,
                p.Balancenum,
                p.Type,
                p.Addtime

            }).ToList().Select(p => new
            {
                Id = p.Id,
                p.Balancenum,
                p.Type,
                Addtime = p.Addtime.ToString("yyyy-MM-dd")
            });
            return PageHelper.toJson(new { List = PointDetailList, Condition = brokeaccountcon});
        }

        #region 经纪人账户明细详情

        /// <summary>
        /// 传入用户id,页码,压面数量设置,检索经纪人账户明细,返回经纪人账户明细
        /// </summary>
        /// <param name="userId">经纪人ID</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面数量</param>
        /// <returns>经纪人账户详细信息</returns>
        [Description("获取经纪人账户详细信息")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetPointDetailListByUserId(string userId=null, int page = 1, int pageSize = 10)
        {
            if(string.IsNullOrEmpty(userId))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据错误！"));
            }

            var brokeaccountcon = new BrokeAccountSearchCondition
            {
               Brokers =_brokerService.GetBrokerById(Convert.ToInt32(userId)),
               Page = Convert.ToInt32(page),
               PageCount = pageSize
            };
            var PointDetailList = _brokeaccountService.GetBrokeAccountsByCondition(brokeaccountcon).Select(p => new
            {
                Id = p.Id,
                p.Balancenum,
                p.MoneyDesc,
                brokername = p.Broker.Brokername,
                p.Addtime

            }).ToList().Select(p => new {
                Id = p.Id,
                p.Balancenum,
                p.MoneyDesc,
                brokername = p.brokername,
               Addtime= p.Addtime.ToString ("yyyy-MM-dd")
            });
            var PointDetailListCount = _brokeaccountService.GetBrokeAccountCount(brokeaccountcon);
            return PageHelper.toJson(new { List = PointDetailList, Condition = brokeaccountcon, totalCount = PointDetailListCount });       
        }

        /// <summary>
        /// 传入经纪人账户信息参数,新增经纪人账户信息,返回新增经纪人账户信息结果状态,成功则返回"数据添加成功",失败返回"数据添加失败"
        /// </summary>
        /// <param name="brokeAccount">经纪人账户参数</param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [Description("新增经纪人账户信息")]
        
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
