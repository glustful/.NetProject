using CRM.Entity.Model;
using CRM.Service.Broker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Zerg.Common;

namespace Zerg.Controllers.CRM
{
    /// <summary>
    /// 经纪人管理  李洪亮  2015-05-04
    /// </summary>
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
        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetBroker([FromBody] string id)
        {
            if (!string.IsNullOrEmpty(id) && PageHelper.ValidateNumber(id))
            {
                return PageHelper.toJson(_brokerService.GetBrokerById(Convert.ToInt32(id)));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }



        [System.Web.Http.HttpGet]
        public HttpResponseMessage SearchBrokers([FromBody] string pageindex)
        {
            var brokerSearchCondition = new BrokerSearchCondition
            {
                OrderBy = EnumBrokerSearchOrderBy.OrderById,
                Page=Convert.ToInt32(pageindex),
                PageCount=10
            };
            return PageHelper.toJson(_brokerService.GetBrokersByCondition(brokerSearchCondition).ToList());
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
            if (broker != null && !string.IsNullOrEmpty(broker.Id.ToString()) && PageHelper.ValidateNumber(broker.Id.ToString()) && !string.IsNullOrEmpty(broker.Brokername))
            {
                var brokerModel = _brokerService.GetBrokerById(broker.Id);
                brokerModel.Uptime = DateTime.Now;
                brokerModel.Brokername = broker.Brokername;
                brokerModel.Address = broker.Address;
                brokerModel.Nickname = broker.Nickname;

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





        #endregion


    }
}
