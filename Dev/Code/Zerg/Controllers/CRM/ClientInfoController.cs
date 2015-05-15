using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.ClientInfo;
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
    /// 客户信息  李洪亮 2015-05-06
    /// </summary>
    public class ClientInfoController : ApiController
    {
        private IClientInfoService _clientInfoService;
        private IBrokerService _brokerService;
        public ClientInfoController(IClientInfoService clientInfoService, IBrokerService brokerService)
        {
            _clientInfoService = clientInfoService;
            _brokerService = brokerService;
        }

        #region 客户信息

        /// <summary>
        /// 查询推荐经纪人表中的所有数据
        /// </summary>
        /// <param name="userId">经纪人ID</param>
        /// <returns></returns>

        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetClientInfoList()
        {
            var clientinfosearchcon = new ClientInfoSearchCondition
            {
                PageCount = 10
            };
            return PageHelper.toJson(_clientInfoService.GetClientInfosByCondition(clientinfosearchcon).ToList());
        }






        /// <summary>
        /// 新增 客户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddClientInfo([FromBody] ClientInfoEntity clientinfo)
        {
            if (clientinfo != null)
            {
                var entity = new ClientInfoEntity
                {
                    Clientname = clientinfo.Clientname,
                    Houses = clientinfo.Houses,
                    Housetype = clientinfo.Housetype,
                    Note = clientinfo.Note,
                    Phone = "",                  
                    Uptime = DateTime.Now,
                    Addtime = DateTime.Now,
                };

                try
                {
                    if (_clientInfoService.Create(entity) != null)
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
