using CRM.Entity.Model;
using CRM.Service.BankCard;
using CRM.Service.BrokeAccount;
using CRM.Service.Broker;
using CRM.Service.BrokerWithdrawDetail;
using CRM.Service.BrokerWithdraw;
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
using System.Text;

namespace Zerg.Controllers.CRM
{
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [AllowAnonymous]
    /// <summary>
    /// 经纪人提现明细  李洪亮  2015-05-05
    /// </summary>
    [Description("经纪人提现明细")]
    public class BrokerWithdrawDetailController : ApiController
    {
        private IBrokerWithdrawService _brokerwithdrawService;
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
        public BrokerWithdrawDetailController(IBrokerWithdrawService brokerwithdrawService, IBrokeAccountService brokeaccountService, IBankCardService bankcardService, IWorkContext workContext, IBrokerWithdrawDetailService brokerwithdrawdetailService, IBrokerService brokerService)
        {
            _brokerwithdrawService = brokerwithdrawService;
            _brokeaccountService = brokeaccountService;
            _brokerwithdrawdetailService = brokerwithdrawdetailService;
            _brokerService = brokerService;
            _workContext = workContext;
            _bankcardService = bankcardService;
        }
        /// <summary>
        /// 根据提现ID查询提现明细信息  
        /// chen 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Description("根据经纪人查询提现明细")]
        //[HttpGet]
        public HttpResponseMessage GetBrokerWithdrawDetailByBrokerWithdrawId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据错误！"));
            }
            var seach = new BrokerWithdrawDetailSearchCondition
            {
                OrderBy = EnumBrokerWithdrawDetailSearchOrderBy.OrderById,
                BrokerWithdraw = _brokerwithdrawService.GetBrokerWithdrawById(Convert.ToInt32(id)),
            };
            var list = _brokerwithdrawdetailService.GetBrokerWithdrawDetailsByCondition(seach).Select(b => new
            {
                b.Id,
                b.Withdrawnum,
                b.BrokeAccount_Id,
                b.Withdrawtime,
                b.Type,
                b.BrokerWithdraw.WithdrawDesc,

            }).ToList().Select(a => new
            {
                a.Id,
                a.Withdrawnum,
                a.BrokeAccount_Id,
                a.Type,
                WithdrawDesc = a.WithdrawDesc,
                Withdrawtime = a.Withdrawtime.ToString("yyy-MM-dd"),
            });
            //取出所有提现明细的ID
            StringBuilder SB = new StringBuilder();
            foreach (var p in list)
            {
                SB.Append(p.Id.ToString() + ",");
            }
            ////////////取出账户明细ID
            StringBuilder stb = new StringBuilder();
            foreach (var b in list)
            {
                stb.Append(b.BrokeAccount_Id.Id.ToString() + ",");
            }
            return PageHelper.toJson(new { List = list, Ids = SB.ToString(), BrokeAccountId = stb.ToString() });
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
                Withdrawnum = p.Withdrawnum,
                Withdrawtime = p.Withdrawtime
            }).ToList().Select(p => new
            {
                Id = p.Id,
                bankname = p.bankname,
                banknumber = p.banknumber,
                //p.Withdrawnum,
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
            int bankId = 0;//银行Id
            decimal withdrawMoney = 0;//提现金额
            if (string.IsNullOrEmpty(MoneyEntity.Bank) || string.IsNullOrEmpty( MoneyEntity.Ids) || string.IsNullOrEmpty(MoneyEntity.Hidm) || string.IsNullOrEmpty(MoneyEntity.MobileYzm) || string.IsNullOrEmpty(MoneyEntity.Money))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误"));
            }
            if (!Int32.TryParse(MoneyEntity.Bank, out bankId))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误"));
            }

            //if (!Int32.TryParse(MoneyEntity.Money, out withdrawMoney))
            //{
            //    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误"));
            //}
            //else
            //{
            //    if (withdrawMoney <= 0)
            //    {
            //        return PageHelper.toJson(PageHelper.ReturnValue(false, "提现金额必须大于零"));
            //    }
            //}


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
                    //根据对应的经纪人账户明细Ids 添加到提现主表 附表中去 
                    if (!string.IsNullOrEmpty(MoneyEntity.Ids))
                    {
                        var bankCard = _bankcardService.GetBankCardById(Convert.ToInt32(MoneyEntity.Bank));
                        if(bankCard.Broker.Id!=broker.Id)
                        {
                            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误"));
                        }


                        List<BrokerWithdrawDetailEntity> listBrokerWithDetail = new List<BrokerWithdrawDetailEntity>(); //提现明细List
                        var brokerWithdraw = new BrokerWithdrawEntity //提现主表
                        {
                            Addtime = DateTime.Now,
                            Adduser = broker.Id,
                            BankCard = bankCard,
                            BankSn = "",
                            Broker = broker,
                            State = 0,
                            Uptime = DateTime.Now,
                            Upuser = broker.Id,
                            WithdrawDesc = "",
                            WithdrawTime = DateTime.Now, 
                        };

                        try
                        {
                            foreach (var p in MoneyEntity.Ids.Split(','))
                            {
                                if(string.IsNullOrEmpty(p))
                                {
                                    continue;
                                }
                                var broaccount = _brokeaccountService.GetBrokeAccountById(Convert.ToInt32(p));//获取该笔账户
                                if (broaccount.Broker.Id != broker.Id)//判断该笔账户金额是否是当前这个经纪人
                                {
                                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误"));
                                }
                                else
                                {
                                    withdrawMoney += broaccount.Balancenum;//提现总金额

                                    var bwithdrawDetail = new BrokerWithdrawDetailEntity
                                    {
                                        BankCard = bankCard,
                                        Withdrawnum = Convert.ToDecimal(broaccount.Balancenum),
                                        Withdrawtime = DateTime.Now,
                                        Broker = broker,
                                        Uptime = DateTime.Now,
                                        Addtime = DateTime.Now,
                                        Adduser = broker.Id,
                                        Upuser = broker.Id,
                                        Type =broaccount.Type.ToString(),
                                        BrokeAccount_Id =broaccount                                          
                                    };
                                    listBrokerWithDetail.Add(bwithdrawDetail);
                                }
                            }
                            //更新提现总金额
                            brokerWithdraw.WithdrawTotalNum = withdrawMoney;

                           brokerWithdraw= _brokerwithdrawService.Create(brokerWithdraw);//添加到提现主表

                            foreach(var browithdetail in listBrokerWithDetail)//添加到提现附表
                            {


                                browithdetail.BrokerWithdraw = brokerWithdraw;

                                _brokerwithdrawdetailService.Create(browithdetail);

                                //更改账户表中 状态
                                var brokeraccount = browithdetail.BrokeAccount_Id;
                                brokeraccount.State = -1;
                                _brokeaccountService.Update(brokeraccount);
                            }
                            //更新到经纪人表中 可用金额
                            broker.Amount = Convert.ToDecimal(GetBrokerAmount());
                            _brokerService.Update(broker);

                            return PageHelper.toJson(PageHelper.ReturnValue(true, "提现申请成功！"));
                        }
                        catch
                        {
                            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误"));
                        }

                    }
                    else
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误"));
                    }



                    #region 以前的逻辑
                    //decimal getMoney = Convert.ToDecimal(GetBrokerAmount());//计算得到的剩余总金额                   
                    //decimal syMoney = 0;//剩余金额
                    //// 提现金额逻辑判断(账户金额表 和提现表相减 跟经纪人表中‘提现金额’字段一致)

                    //if (Convert.ToDecimal(MoneyEntity.Money) > getMoney)
                    //{
                    //    return PageHelper.toJson(PageHelper.ReturnValue(false, "账户余额不足，不能提现"));
                    //}
                    //syMoney = getMoney - Convert.ToDecimal(MoneyEntity.Money);

                    ////将剩余金额更新到经纪人表中金额字段
                    ////broker.Amount = syMoney;
                    ////_brokerService.Update(broker);


                    ////更新到提现表中
                    //var entity = new BrokerWithdrawDetailEntity
                    //{
                    //    BankCard = _bankcardService.GetBankCardById(Convert.ToInt32(MoneyEntity.Bank)),
                    //    Withdrawnum = Convert.ToDecimal(MoneyEntity.Money),
                    //    Withdrawtime = DateTime.Now,
                    //    Broker = broker,
                    //    Uptime = DateTime.Now,
                    //    Addtime = DateTime.Now,
                    //    Adduser = broker.Id,
                    //    Upuser = broker.Id,
                    //    Type = "0"
                    //};

                    //try
                    //{
                    //    if (_brokerwithdrawdetailService.Create(entity) != null)
                    //    {
                    //        return PageHelper.toJson(PageHelper.ReturnValue(true, entity.Id.ToString()));
                    //    }
                    //}
                    //catch
                    //{
                    //    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
                    //}

                    #endregion


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
                        Brokers = broker,
                        State=0
                    };
                    //BrokerWithdrawDetailSearchCondition browithdetailcon = new BrokerWithdrawDetailSearchCondition
                    //{
                    //    Brokers = broker,
                    //    Type = "1"

                    //};
                    //decimal AddMoneys = _brokeaccountService.GetBrokeAccountsByCondition(broconditon).Count() > 0 ? _brokeaccountService.GetBrokeAccountsByCondition(broconditon).Sum(o => o.Balancenum) : 0;//新增的金额总和
                    //decimal TxMoneys = _brokerwithdrawdetailService.GetBrokerWithdrawDetailsByCondition(browithdetailcon).Count() > 0 ? _brokerwithdrawdetailService.GetBrokerWithdrawDetailsByCondition(browithdetailcon).Sum(o => o.Withdrawnum) : 0;//提现的总金额
                    //return (AddMoneys - TxMoneys).ToString();

                    return _brokeaccountService.GetBrokeAccountsByCondition(broconditon).Count() > 0 ? _brokeaccountService.GetBrokeAccountsByCondition(broconditon).Sum(o => o.Balancenum).ToString() : "0";//金额总和
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

        /// <summary>
        /// 账户明细id列表
        /// </summary>
        public string Ids { get; set; }
    }
}
