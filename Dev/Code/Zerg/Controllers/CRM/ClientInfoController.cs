using System.Globalization;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.BrokerRECClient;
using CRM.Service.ClientInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Zerg.Common;
using Zerg.Models.CRM;
using System.Data;

namespace Zerg.Controllers.CRM
{
     [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// 客户信息  李洪亮 2015-05-06
    /// </summary>
    public class ClientInfoController : ApiController
    {
        private IClientInfoService _clientInfoService;
        private IBrokerService _brokerService;
        private IBrokerRECClientService _brokerRecClientService;
        public ClientInfoController(
            IClientInfoService clientInfoService, 
            IBrokerService brokerService,
            IBrokerRECClientService brokerRecClientService
            )
        {
            _clientInfoService = clientInfoService;
            _brokerService = brokerService;
            _brokerRecClientService = brokerRecClientService;
        }

        #region 客户信息

        /// <summary>
        /// 查询推荐经纪人表中的所有数据
        /// </summary>
        /// <param name="status"></param>
        /// <param name="clientName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetClientInfoList(EnumBRECCType status, string clientName, int page, int pageSize)
        {
            var condition = new BrokerRECClientSearchCondition
            {
                OrderBy = EnumBrokerRECClientSearchOrderBy.OrderById,
                Page = page,
                PageCount = pageSize,
                Status = status,
                Brokername = clientName
            };

            var list = _brokerRecClientService.GetBrokerRECClientsByCondition(condition).Select(a => new
            {
                a.Id,
                a.Clientname,
                a.ClientInfo.Phone,
                a.Brokername,
                a.Uptime

            }).ToList();

            var totalCont = _brokerRecClientService.GetBrokerRECClientCount(condition);

            return PageHelper.toJson(new { list1 = list, condition1 = condition, totalCont1 = totalCont });
        }

        /// <summary>
        /// 获取客户详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage ClientInfo(int id)
        {
            var condition = new BrokerRECClientSearchCondition
            {
                Id = id
            };


            var model = _brokerRecClientService.GetBrokerRECClientsByCondition(condition).ToList();
            try {
            var clientModel =model .Select(p=>new {
            
                Clientname =p.ClientInfo.Clientname,
                Phone =p.ClientInfo.Phone,
                Housetype = p.ClientInfo.Housetype,
                Houses = p.ClientInfo.Houses,
                Note = p.ClientInfo.Note,
                Uptime = p.Uptime.ToString(CultureInfo.InvariantCulture)
            });
            var brokerModel =model .Select(p=>new {
          
                Brokername = p.Brokername,
                Brokerlevel = p.Brokerlevel,
                Phone = p.Broker.Phone,
                Qq = p.Broker.Qq,
                RegTime = p.Broker.Regtime.ToString(CultureInfo.InvariantCulture),
                Projectname =p.Projectname
            });
            return PageHelper.toJson(new { clientModel, brokerModel });
            }
            catch { }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
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
