using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.BrokerRECClient;
using Zerg.Common;
using Zerg.Models.CRM;

namespace Zerg.Controllers.CRM
{
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// admin的推荐至平台流程处理
    /// </summary>
    public class AdminRecomController : ApiController
    {
        private readonly IBrokerRECClientService _brokerRecClientService;
        private readonly IBrokerService _brokerService;

        public AdminRecomController(IBrokerRECClientService brokerRecClientService,
            IBrokerService brokerService
            )
        {
            _brokerRecClientService = brokerRecClientService;
            _brokerService = brokerService;
        }

        #region 经济人列表 杨定鹏 2015年5月4日14:29:24

        /// <summary>
        /// 经纪人列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage BrokerList(EnumBRECCType status, string brokername,int page, int pageSize)
        {
           
            var condition = new BrokerRECClientSearchCondition
            {
                OrderBy = EnumBrokerRECClientSearchOrderBy.OrderById,
                Page = page,
                PageCount = pageSize,
                Status = status,
                Brokername=brokername
                    
            };
            var list = _brokerRecClientService.GetBrokerRECClientsByCondition(condition).Select(a => new
            {
                a.Id,
                a.Brokername,
                a.Brokerlevel,
                a.ClientInfo.Phone,
                a.Projectname,
                a.Addtime,

                a.Clientname,
                SecretaryName=a.SecretaryId.Brokername,
                a.SecretaryPhone,
                Waiter=a.WriterId.Brokername,
                a.WriterPhone,
                a.Uptime

            }).ToList();

            var totalCont = _brokerRecClientService.GetBrokerRECClientCount(condition);

            return PageHelper.toJson(new { list1 = list, condition1 = condition, totalCont1 = totalCont });
        }
        #endregion

        #region 待审核业务处理 杨定鹏 2015年5月5日16:28:30
        /// <summary>
        /// 审核状态变更
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAuditDetail(int id)
        {
            var model = _brokerRecClientService.GetBrokerRECClientById(id);
            var newModel = new BrokerRECClientModel
            {
                Id = model.Id,
                Broker = model.Broker.Id,
                NickName = model.Broker.Nickname,
                Brokername = model.Brokername,
                Brokerlevel = model.Brokerlevel,
                Sex = model.Broker.Sexy,
                RegTime = model.Broker.Regtime.ToString(CultureInfo.InvariantCulture),

                Clientname = model.Clientname,
                HouseType = model.ClientInfo.Housetype,
                Houses = model.ClientInfo.Houses,
                Note = model.ClientInfo.Note,
                Phone = model.Phone

            };

            return PageHelper.toJson(newModel);
        }

        /// <summary>
        /// 确认审核
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage PassAudit([FromBody]BrokerRECClientModel brokerRecClientModel)
        {
            if (brokerRecClientModel.Id==0)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "Id不能为空"));
            }

            var model = _brokerRecClientService.GetBrokerRECClientById(brokerRecClientModel.Id);
            model.Status = brokerRecClientModel.Status;
            model.Uptime = DateTime.Now;

            _brokerRecClientService.Update(model);
            return PageHelper.toJson(PageHelper.ReturnValue(true,"确认成功"));
        }

        #region 选择带客人 杨定鹏 2015年5月5日19:45:14
        /// <summary>
        /// 带客人列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage WaiterList()
        {
            var condition = new BrokerSearchCondition
            {
                OrderBy = EnumBrokerSearchOrderBy.OrderById,
                UserType = EnumUserType.带客人员
            };
            var list = _brokerService.GetBrokersByCondition(condition).Select(a => new
            {
                a.Id,
                a.Brokername, 
                a.Phone
            }).ToList();
            return PageHelper.toJson(list);
        }

        #endregion

        #region 场秘管理 杨定鹏 2015年5月5日19:45:40
        /// <summary>
        /// 场秘列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage SecretaryList()
        {
            var condition = new BrokerSearchCondition
            {
                OrderBy = EnumBrokerSearchOrderBy.OrderById,
                UserType = EnumUserType.场秘
            };
            var list = _brokerService.GetBrokersByCondition(condition).Select(a => new
            {
                a.Id,
                a.Brokername,
                a.Phone
            }).ToList();
            return PageHelper.toJson(list);
        }

        #endregion

        /// <summary>
        /// 确认成功/失败
        /// </summary>
        /// <param name="brokerRecClientModel"></param>
        /// <returns></returns>
        public HttpResponseMessage Access([FromBody]BrokerRECClientModel brokerRecClientModel)
        {
            if (brokerRecClientModel == null) throw new ArgumentNullException("brokerRecClientModel");
            var model = new BrokerRECClientEntity
            {
                Id=brokerRecClientModel.Id,
                Status = brokerRecClientModel.Status
            };
            _brokerRecClientService.Update(model);
            return PageHelper.toJson(PageHelper.ReturnValue(true, "提交成功"));
        }

        #endregion
    }
}
