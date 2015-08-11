using CRM.Entity.Model;
using CRM.Service.Bank;
using CRM.Service.BankCard;
using CRM.Service.Broker;
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
using CRM.Service.BrokerWithdraw;
namespace Zerg.Controllers.CRM
{

    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// 银行卡管理  李洪亮  2015-05-05
    /// </summary>
    [Description("银行卡管理类")]
    public class BankCardController : ApiController
    {
        private readonly IBankCardService _bankcardService;
        private readonly IBrokerService _brokerService;//经纪人
        private readonly IWorkContext _workContext;
        private readonly IBankService _bankService;
        private readonly IBrokerWithdrawService _brokerwithdrawService;
        /// <summary>
        /// 银行卡管理初始化
        /// </summary>
        /// <param name="bankService">bankService</param>
        /// <param name="workContext">workContext</param>
        /// <param name="bankcardService">bankcardService</param>
        /// <param name="brokerService">brokerService</param>
        public BankCardController(IBankService bankService, IWorkContext workContext, IBankCardService bankcardService, IBrokerService brokerService, IBrokerWithdrawService brokerwithdrawService)
        {
            _bankcardService = bankcardService;
            _brokerService = brokerService;
            _workContext = workContext;
            _bankService = bankService;
            _brokerwithdrawService=brokerwithdrawService;
        }

        #region 银行卡管理

        /// <summary>
        /// 传入经纪人ID 查询该经纪人的银行卡列表,返回经纪人银行卡列表
        /// </summary>
        /// <param name="userId">经纪人ID</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面数量</param>
        /// <returns>经纪人银行卡列表</returns>
        [System.Web.Http.HttpGet]
        [Description("传入经纪人ID,返回经纪人关联的银行卡列表")]
        public HttpResponseMessage SearchBankCardsByUserID(string userId = null, int page = 1, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(userId) || !PageHelper.ValidateNumber(userId))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
            }

            var bankcardSearchCon = new BankCardSearchCondition
            {
                Brokers = _brokerService.GetBrokerById(Convert.ToInt32(userId)),
                Page = Convert.ToInt32(page),
                PageCount = pageSize
            };

            var bankList = _bankcardService.GetBankCardsByCondition(bankcardSearchCon).Select(p => new
            {
                Id = p.Id,
                bankName = p.Bank.Codeid,
                p.Num,
                p.Addtime

            }).ToList();
            var bankListCount = _bankcardService.GetBankCardCount(bankcardSearchCon);
            return PageHelper.toJson(new { List = bankList, Condition = bankcardSearchCon, totalCount1 = bankListCount });

        }


        /// <summary>
        ///查询当前经纪人的所有银行,返回经纪人银行列表
        /// </summary>
        /// <returns>经纪人银行列表</returns>
        [System.Web.Http.HttpGet]
        [Description("查询当前经纪人的所有银行,返回经纪人银行列表")]
        public HttpResponseMessage SearchAllBankByUser()
        {
            var user = (UserBase)_workContext.CurrentUser;
            if (user != null)
            {
                var broker = _brokerService.GetBrokerByUserId(user.Id);//获取当前经纪人
                if (broker == null)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
                }
                else
                {
                    var bankcardSearchCon = new BankCardSearchCondition
                    {
                        Brokers = broker
                    };
                    var bankList = _bankcardService.GetBankCardsByCondition(bankcardSearchCon).Select(p => new
                    {
                        Id = p.Id,
                        bankName = p.Bank.Codeid,
                        Num = p.Num.ToString().Substring(p.Num.ToString().Length - 4, 4)
                    }).ToList();

                    return PageHelper.toJson(new { List = bankList, AmountMoney = broker.Amount });
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));

        }


        /// <summary>
        /// 传入银行卡参数,创建银行卡关联,返回结果状态信息,成功返回"数据添加成功",失败返回"数据添加失败"
        /// </summary>
        /// <param name="bankcard">银行卡参数</param>
        /// <returns>创建银行卡关联结果状态信息</returns>
        [System.Web.Http.HttpPost]
        [Description("传入银行卡参数,创建银行卡关联")]
        public HttpResponseMessage AddBankCard([FromBody] AddBankCardEntity bankcard)
        {
            #region 验证码判断 解密
            var strDes = EncrypHelper.Decrypt(bankcard.Hidm, "Hos2xNLrgfaYFY2MKuFf3g==");//解密
            string[] str = strDes.Split('$');
            string source = str[0];//获取验证码
            DateTime date = Convert.ToDateTime(str[1]);//获取发送验证码的时间
            DateTime dateNow = Convert.ToDateTime(DateTime.Now.ToLongTimeString());//获取当前时间
            TimeSpan ts = dateNow.Subtract(date);
            double secMinu = ts.TotalMinutes;//得到发送时间与现在时间的时间间隔分钟数
            if (secMinu > 30) //发送时间与接受时间是否大于3分钟
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "你已超过时间验证，请重新发送验证码！"));
            }
            else
            {
                if (bankcard.MobileYzm != source)//判断验证码是否一致
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "验证码错误，请重新发送！"));
                }
            }

            #endregion


            var user = (UserBase)_workContext.CurrentUser;
            if (user != null)
            {
                var broker = _brokerService.GetBrokerByUserId(user.Id);//获取当前经纪人
                if (broker != null)
                {
                    var entity = new BankCardEntity
                    {
                        Addtime = DateTime.Now,
                        Uptime = DateTime.Now,
                        Address = bankcard.Address,
                        Adduser = broker.Id,
                        Type = "储蓄卡",
                        Upuser = broker.Id,
                        Bank = _bankService.GetBankById(Convert.ToInt32(bankcard.Bank)),
                        Broker = broker,
                        Num = bankcard.Num,
                        Deadline = Convert.ToDateTime("2000-01-01 00:00:00")
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
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));

        }

        /// <summary>
        ///查询当前经济人的所有银行 以下拉列表方式返回
        /// </summary>
        /// <returns>下拉列表形式经纪人所有关联银行</returns>
        [System.Web.Http.HttpGet]
        [Description("获取经纪人所有关联银行(下拉列表形式)")]
        public HttpResponseMessage SearchAllBankByUserToSelect()
        {
            var user = (UserBase)_workContext.CurrentUser;
            if (user != null)
            {
                var broker = _brokerService.GetBrokerByUserId(user.Id);//获取当前经纪人
                if (broker == null)
                {

                    return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
                }
                else
                {
                    var bankcardSearchCon = new BankCardSearchCondition
                    {
                        Brokers = broker
                    };
                    var bankList = _bankcardService.GetBankCardsByCondition(bankcardSearchCon).Select(p => new
                    {
                        Id = p.Id,
                        Name = p.Bank.Codeid + "[***" + p.Num.ToString().Substring(p.Num.ToString().Length - 4, 4) + "]"
                    }).ToList();

                    return PageHelper.toJson(new { List = bankList, AmountMoney = broker.Amount });
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));

        }


        /// <summary>
        /// 删除一个银行卡
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DeleteBankCard([FromBody] string id)
        {
            if (!string.IsNullOrEmpty(id) && id!="0")
           {
             int Id;
             if (!Int32.TryParse(id, out Id))
             {
                 return PageHelper.toJson(PageHelper.ReturnValue(false, "数据转换异常"));
             }
             var user = (UserBase)_workContext.CurrentUser;
             if (user != null)
             {
                 var broker = _brokerService.GetBrokerByUserId(user.Id);//获取当前经纪人
                 if (broker == null)
                 {

                     return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
                 }
                 else
                 {
                      
                     
                     int [] ids={Id};                 
                     var bankcardSearchCon = new BankCardSearchCondition
                     {
                         Brokers = broker,
                         Ids=ids
                     };
                    if( _bankcardService.GetBankCardCount(bankcardSearchCon)>0)//判断是否是自己的卡
                    {
                        var Bank=_bankcardService.GetBankCardById(Id);
                        if (Bank==null)
                        {
                          return PageHelper.toJson(PageHelper.ReturnValue(false, "此卡不存在"));
                        }
                        BankCardEntity[] banks={Bank};
                      BrokerWithdrawSearchCondition bwithSearchCon=new BrokerWithdrawSearchCondition{
                           Brokers=broker,
                           BankCards=banks
                      };
                      if (_brokerwithdrawService.GetBrokerWithdrawCount(bwithSearchCon) > 0)//判断用户提现表是否使用此卡
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "有该银行相关提现数据不能删除"));
                    }
                      _bankcardService.Delete(Bank);
                      return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));

                    }
                    else
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "此卡不是您的，不能删除！"));
                    }
                                     
                 }
             }
             else
             {
                 return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
             }
           
           }
           return PageHelper.toJson(PageHelper.ReturnValue(false, "数据异常"));
        }
        #endregion
    }

    /// <summary>
    /// 添加银行卡映射实体类
    /// </summary>
    [Description(" 添加银行卡映射实体类")]
    public class AddBankCardEntity
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string Num { get; set; }

        /// <summary>
        /// 类型  存储卡
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        public string Bank { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

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
