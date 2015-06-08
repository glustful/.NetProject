using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using CRM.Entity.Model;
using CRM.Service.Broker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Zerg.Common;
using Zerg.Models.CRM;

namespace Zerg.Controllers.CRM
{
    [AllowAnonymous ]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// 经纪人管理  李洪亮  2015-05-04
    /// </summary>
    [Description("经纪人管理")]
    public class BrokerInfoController : ApiController
    {
        private readonly IBrokerService _brokerService;

        public BrokerInfoController(IBrokerService brokerService)
        {
            _brokerService = brokerService;
        }


        #region 经纪人管理 
       
        
        
        /// <summary>
        /// 获取经纪人
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetBroker(string id)
        {
            if (string.IsNullOrEmpty(id) || !PageHelper.ValidateNumber(id))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
            }
            var brokerlist = _brokerService.GetBrokerById(Convert.ToInt32(id));
            return PageHelper.toJson(new { List = brokerlist });
        }

        /// <summary>
        /// 通过User查找经纪人
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetBrokerByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId) || !PageHelper.ValidateNumber(userId))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
            }

            var model = _brokerService.GetBrokerByUserId(Convert.ToInt32(userId));

            if (model == null) return PageHelper.toJson(PageHelper.ReturnValue(false, "该用户不存在！"));

            var brokerInfo = new BrokerModel
            {
                Id = model.Id,
                Brokername = model.Brokername,
                Realname = model.Realname,
                Nickname = model.Nickname,
                Sexy = model.Sexy,
                Sfz = model.Sfz,
                Email = model.Email,
                Phone=model.Phone,
                Headphoto=model.Headphoto
            };

            return PageHelper.toJson(brokerInfo);
        }


        /// <summary>
        /// 会员列表查询操作
        /// </summary>
        /// <param name="userType"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
       [System.Web.Http.HttpGet]
        public HttpResponseMessage SearchBrokers(EnumUserType userType,int? phone, string name = null,  int page = 1, int pageSize = 10)
        {
            //var phones = new int[1];

            var brokerSearchCondition = new BrokerSearchCondition
            {
                Brokername=name,
                //Phones=phones,
                OrderBy = EnumBrokerSearchOrderBy.OrderById,
                Page=Convert.ToInt32(page),
                PageCount=10,
                UserType = userType
            };
           
            var brokersList = _brokerService.GetBrokersByCondition(brokerSearchCondition).Select(p => new
            {
                p.Id,
                p.Nickname,
                p.Brokername,
                p.Realname,
                p.Phone,
                p.Sfz,
                p.Amount,
                p.Agentlevel,
                p.Regtime,
                p.Headphoto
            }).ToList().Select(b => new
            {
                b.Id,
                b.Nickname,
                b.Brokername,
                b.Realname,
                b.Phone,
                b.Sfz,
                b.Amount,
                b.Agentlevel,
                Regtime = b.Regtime.ToString("yyyy-MM-dd"),
                b.Headphoto
            });
            var brokerListCount = _brokerService.GetBrokerCount(brokerSearchCondition);
            return PageHelper.toJson(new { List = brokersList, Condition = brokerSearchCondition, totalCount = brokerListCount });
        }

        /// <summary>
        /// 新增经纪人
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddBroker([FromBody] BrokerEntity broker)
        {

            if (!string.IsNullOrEmpty(broker.Brokername) )
            {
                var brokerEntity = new BrokerEntity
                {
                    Brokername=broker.Brokername,
                    Uptime = DateTime.Now,
                    Addtime = DateTime.Now,

                };

                try
                {
                    if (_brokerService.Create(brokerEntity) != null)
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


        /// <summary>
        /// 修改经纪人
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateBroker([FromBody] BrokerEntity broker)
        {
            if (broker != null && !string.IsNullOrEmpty(broker.Id.ToString()) && PageHelper.ValidateNumber(broker.Id.ToString()) )
            {
                var brokerModel = _brokerService.GetBrokerById(broker.Id);
                brokerModel.Headphoto = broker.Headphoto;
                brokerModel.Brokername = broker.Brokername;
                brokerModel.Phone = broker.Phone;
                brokerModel.Sfz = broker.Sfz;
                brokerModel.Email = broker.Email;
                brokerModel.Realname = broker.Realname;
                brokerModel.Sexy = broker.Sexy;

                try
                {
                    if (_brokerService.Update(brokerModel) != null)
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


        /// <summary>
        /// 删除经纪人(标为删除状态 不可恢复)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DeleteBroker([FromBody] string id)
        {
            if (!string.IsNullOrEmpty(id) && PageHelper.ValidateNumber(id))
            {
                if (_brokerService.Delete(_brokerService.GetBrokerById(Convert.ToInt32(id))))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据成功删除！"));
                }
                else
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败！"));
                }
            }

            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }


        /// <summary>
        /// 注销经纪人(以后能够恢复)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage CancelBroker([FromBody] string id)
        {
            if (!string.IsNullOrEmpty(id) && PageHelper.ValidateNumber(id))
            {
                if (_brokerService.Delete(_brokerService.GetBrokerById(Convert.ToInt32(id))))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据成功删除！"));
                }
                else
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败！"));
                }
            }

            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }


        /// <summary>
        /// 经纪人排行 返回前10条
        /// </summary>
        /// <returns></returns>

        [System.Web.Http.HttpGet]
        public HttpResponseMessage  OrderByBrokerList()
        {
          

            var brokersList = _brokerService.OrderbyBrokersList().Select(p => new
            {
                p.Id,
                p.Brokername,
                p.Agentlevel,
                p.Amount

            }).ToList();
            
            return PageHelper.toJson(new { List = brokersList});
        }


        /// <summary>
        /// 经纪人排行 返回前3条
        /// </summary>
        /// <returns></returns>

        [System.Web.Http.HttpGet]
        public HttpResponseMessage OrderByBrokerTopThree()
        {


            var brokersList = _brokerService.OrderbyBrokersList().Select(p => new
            {
                p.Id,
                p.Brokername,
                p.Agentlevel,
                p.Amount

            }).Take(3).ToList();

            return PageHelper.toJson(new { List = brokersList });
        }

        [HttpPost]

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage SendSMS([FromBody] string  mobile)
        {
            return PageHelper.toJson(  SMSHelper.Sending(mobile, "[创富宝]短信接口测试，发送时间："+DateTime.Now.ToString()));

        }

        #endregion


    }
}
