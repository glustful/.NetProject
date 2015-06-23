using CRM.Entity.Model;
using CRM.Service.BankCard;
using CRM.Service.BrokeAccount;
using CRM.Service.Broker;
using CRM.Service.BrokerWithdrawDetail;
using System;
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
    /// <summary>
    /// 经纪人提现明细  李洪亮  2015-05-05
    /// </summary>
    [Description("经纪人提现明细")]
    public class BrokerWithdrawDetailController : ApiController
    {
        private IBrokerWithdrawDetailService _brokerwithdrawdetailService;
        private IBrokeAccountService _brokeaccountService;
        private IBrokerService _brokerService;
        private readonly IWorkContext _workContext;
        private readonly IBankCardService _bankcardService;

        /// <summary>
        /// 经纪人提现明细初始化
        /// </summary>
        /// <param name="brokeaccountService">brokeaccountService</param>
        /// <param name="bankcardService">bankcardService</param>
        /// <param name="workContext">workContext</param>
        /// <param name="brokerwithdrawdetailService">brokerwithdrawdetailService</param>
        /// <param name="brokerService">brokerService</param>
        public BrokerWithdrawDetailController(IBrokeAccountService brokeaccountService, IBankCardService bankcardService, IWorkContext workContext, IBrokerWithdrawDetailService brokerwithdrawdetailService, IBrokerService brokerService)
        {
            _brokeaccountService = brokeaccountService;
            _brokerwithdrawdetailService = brokerwithdrawdetailService;
            _brokerService = brokerService;
            _workContext = workContext;
            _bankcardService = bankcardService;
        }

        #region 经纪人提现明细详情

        /// <summary>
        /// 传入经纪人ID,查询经纪人提现详情,返回经纪人详情列表
        /// </summary>
        /// <param name="userId">经纪人Id</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面数量</param>
        /// <returns>经纪人详情列表</returns> 
        [Description("查询经纪人提现详情")]

        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetBrokerWithdrawDetailListByUserId(string userId = null, int page = 1, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据错误！"));
            }

            var brokerwithdrawdetailcon = new BrokerWithdrawDetailSearchCondition
            {
                Brokers = _brokerService.GetBrokerById(Convert.ToInt32(userId)),
                Page = Convert.ToInt32(page),
                PageCount = pageSize
            };
            var PointDetailList = _brokerwithdrawdetailService.GetBrokerWithdrawDetailsByCondition(brokerwithdrawdetailcon).Select(p => new
            {
                Id = p.Id,
                bankname = p.BankCard.Bank.Codeid,
                banknumber = p.BankCard.Num,
                p.Withdrawnum,
                Withdrawtime = p.Withdrawtime
            }).ToList().Select(p => new
            {
                Id = p.Id,
                bankname = p.bankname,
                banknumber = p.banknumber,
                p.Withdrawnum,
                Withdrawtime = p.Withdrawtime.ToString("yyyy-MM-dd")
            });
            var PointDetailListCount = _brokerwithdrawdetailService.GetBrokerWithdrawDetailCount(brokerwithdrawdetailcon);
            return PageHelper.toJson(new { List = PointDetailList, Condition = brokerwithdrawdetailcon, totalCount = PointDetailListCount });

        }



        /// <summary>
        /// 传入提现参数,增加提现详情,返回结果状态信息,成功提示"数据添加成功",失败提示"数据添加失败"
        /// </summary>
        /// <param name="MoneyEntity">提现参数</param>
        /// <returns>新增提现结果状态信息</returns>

        [Description("传入提现参数,增加提现详情")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddBrokerWithdrawDetail([FromBody] AddMoneyEntity MoneyEntity)
        {
            int bankId = 0;
            int withdrawMoney = 0;
            if (string.IsNullOrEmpty(MoneyEntity.Bank) || string.IsNullOrEmpty(MoneyEntity.Hidm) || string.IsNullOrEmpty(MoneyEntity.MobileYzm) || string.IsNullOrEmpty(MoneyEntity.Money))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误"));
            }
            if (!Int32.TryParse(MoneyEntity.Bank, out bankId))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误"));
            }
            if (!Int32.TryParse(MoneyEntity.Money, out withdrawMoney))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误"));
            }
            else
            {
                if (withdrawMoney <= 0)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "提现金额必须大于零"));
                }
            }


            #region 验证码判断 解密
            var strDes = EncrypHelper.Decrypt(MoneyEntity.Hidm, "Hos2xNLrgfaYFY2MKuFf3g==");//解密
            string[] str = strDes.Split('$');
            string source = str[0];//获取验证码
            DateTime date = Convert.ToDateTime(str[1]);//获取发送验证码的时间
            DateTime dateNow = Convert.ToDateTime(DateTime.Now.ToLongTimeString());//获取当前时间
            TimeSpan ts = dateNow.Subtract(date);
            double secMinu = ts.TotalMinutes;//得到发送时间与现在时间的时间间隔分钟数
            if (secMinu > 3) //发送时间与接受时间是否大于3分钟
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "你已超过时间验证，请重新发送验证码！"));
            }
            else
            {
                if (MoneyEntity.MobileYzm != source)//判断验证码是否一致
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "验证码错误，请重新发送！"));
                }
            }

            #endregion


            //非空验证

            var user = (UserBase)_workContext.CurrentUser;
            if (user != null)
            {
                var broker = _brokerService.GetBrokerByUserId(user.Id);//获取当前经纪人
                if (broker != null)
                {
                    decimal getMoney = Convert.ToDecimal(GetBrokerAmount());//计算得到的剩余总金额                   
                    decimal syMoney = 0;//剩余金额
                    // 提现金额逻辑判断(账户金额表 和提现表相减 跟经纪人表中‘提现金额’字段一致)

                    if (Convert.ToDecimal(MoneyEntity.Money) > getMoney)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "账户余额不足，不能提现"));
                    }
                    syMoney = getMoney - Convert.ToDecimal(MoneyEntity.Money);
                    broker.Amount = syMoney;//将剩余金额更新到经纪人表中金额字段
                    _brokerService.Update(broker);


                    var entity = new BrokerWithdrawDetailEntity
                    {
                        BankCard = _bankcardService.GetBankCardById(Convert.ToInt32(MoneyEntity.Bank)),
                        Withdrawnum = Convert.ToDecimal(MoneyEntity.Money),
                        Withdrawtime = DateTime.Now,
                        Broker = broker,
                        Uptime = DateTime.Now,
                        Addtime = DateTime.Now,
                        Adduser = broker.Id,
                        Upuser = broker.Id
                    };

                    try
                    {
                        if (_brokerwithdrawdetailService.Create(entity) != null)
                        {
                            return PageHelper.toJson(PageHelper.ReturnValue(true, entity.Id.ToString()));
                        }
                    }
                    catch
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
                    }
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
        }

        /// <summary>
        /// 计算经纪人的剩余账户金额 （账户金额表 和提现表相减）
        /// </summary>
        /// <returns>经纪人剩余账户</returns>
        /// 
        [Description("查询经纪人账户余额")]
        public string GetBrokerAmount()
        {
            var user = (UserBase)_workContext.CurrentUser;
            if (user != null)
            {
                var broker = _brokerService.GetBrokerByUserId(user.Id);//获取当前经纪人
                if (broker != null)
                {
                    BrokeAccountSearchCondition broconditon = new BrokeAccountSearchCondition
                    {
                        Brokers = broker
                    };
                    BrokerWithdrawDetailSearchCondition browithdetailcon = new BrokerWithdrawDetailSearchCondition
                    {
                        Brokers = broker
                    };
                    Decimal AddMoneys = _brokeaccountService.GetBrokeAccountsByCondition(broconditon).Sum(o => o.Balancenum);//新增的金额总和
                    decimal TxMoneys = _brokerwithdrawdetailService.GetBrokerWithdrawDetailsByCondition(browithdetailcon).Sum(o => o.Withdrawnum);//提现的总金额
                    return (AddMoneys - TxMoneys).ToString();
                }
            }
            return "";

        }



        #endregion



    }

    /// <summary>
    /// 添加到提现表映射实体类
    /// </summary>
    [Description("添加到提现表映射实体类")]
    public class AddMoneyEntity
    {


        /// <summary>
        /// 银行 卡号
        /// </summary>
        public string Bank { get; set; }

        /// <summary>
        /// 提现金额
        /// </summary>
        public string Money { get; set; }

        /// <summary>
        ///验证码
        /// </summary>
        public string MobileYzm { get; set; }

        /// <summary>
        /// 隐藏验证码
        /// </summary>
        public string Hidm { get; set; }
    }
}
