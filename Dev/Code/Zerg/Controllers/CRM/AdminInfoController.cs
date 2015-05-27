using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.Broker;
using Zerg.Common;
using Zerg.Models.CRM;

namespace Zerg.Controllers.CRM
{
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// CRM 个人信息管理/admin管理
    /// </summary>
    public class AdminInfoController : ApiController
    {
        private readonly IBrokerService _brokerService;

        public AdminInfoController(IBrokerService brokerService
            )
        {
            _brokerService = brokerService;
        }

        #region 管理员明细 杨定鹏2015年5月5日18:45:52
        /// <summary>
        /// 管理员列表
        /// </summary>
        /// <param name="brokerSearchModel"></param>
        /// <returns></returns>
        public HttpResponseMessage GetAdminList([FromBody]BrokerSearchModel brokerSearchModel)
        {
            var condition = new BrokerSearchCondition
            {
                OrderBy = EnumBrokerSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_brokerService.GetBrokersByCondition(condition).ToList());
        }

        /// <summary>
        /// 获取管理员
        /// </summary>
        /// <param name="brokerModel"></param>
        /// <returns></returns>
        public HttpResponseMessage GetAdmin([FromBody] BrokerModel brokerModel)
        {
            return PageHelper.toJson(_brokerService.GetBrokerById(brokerModel.Id));
        }

        /// <summary>
        /// 添加/修改管理员
        /// </summary>
        /// <param name="brokerModel"></param>
        /// <returns></returns>
        public HttpResponseMessage AddAdmin([FromBody] BrokerModel brokerModel)
        {
            var model = new BrokerEntity()
            {
                Id = brokerModel.Id,
            };
            if (brokerModel.Type == "add")
            {
                try
                {
                    if (_brokerService.Create(model) != null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "添加成功"));
                    }
                }
                catch (Exception)
                {

                    return PageHelper.toJson(PageHelper.ReturnValue(false, "添加失败"));
                }
            }
            if (brokerModel.Type == "edit")
            {
                try
                {
                    if (_brokerService.Update(_brokerService.GetBrokerById(model.Id)) != null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "修改成功"));
                    }

                }
                catch (Exception)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "修改失败"));
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证失败"));
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage DelAdmin([FromBody]int id)
        {
            try
            {
                var model = new BrokerEntity()
                {
                    Id=id,
                    State = 0,
                    Usertype = EnumUserType.管理员
                };
                _brokerService.Update(model);
                return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));
            }
            catch (Exception)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败"));
            }
        }

        /// <summary>
        /// 注销管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HttpResponseMessage LogoutAdmin([FromBody] int id)
        {
            try
            {
                var model = new BrokerEntity()
                {
                    Id = id,
                    State = -1,
                };
                _brokerService.Update(model);
                return PageHelper.toJson(PageHelper.ReturnValue(true, "注销成功"));
            }
            catch (Exception)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "注销失败"));
            }
        }

        #endregion
    }
}
