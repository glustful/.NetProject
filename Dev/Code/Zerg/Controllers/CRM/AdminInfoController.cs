using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.Broker;
using Zerg.Common;
using Zerg.Models.CRM;
using System.ComponentModel;

namespace Zerg.Controllers.CRM
{
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// CRM 个人信息管理/admin管理
    /// </summary>

    [Description("CRM个人信息管理/Admin管理")]
    public class AdminInfoController : ApiController
    {
        private readonly IBrokerService _brokerService;
        /// <summary>
        /// 个人信息管理初始化
        /// </summary>
        /// <param name="brokerService"></param>
        [Description("个人信息管理初始化构造器")]
        public AdminInfoController(IBrokerService brokerService
            )
        {
            _brokerService = brokerService;
        }

        #region 管理员明细 杨定鹏2015年5月5日18:45:52
        /// <summary>
        /// 传入管理员参数,查询管理员,返回管理员列表
        /// </summary>
        /// <param name="brokerSearchModel">经纪人参数</param>
        /// <returns>管理员列表</returns>
        /// 
        [Description("检索管理员,返回管理员列表")]
        public HttpResponseMessage GetAdminList([FromBody]BrokerSearchModel brokerSearchModel)
        {
            var condition = new BrokerSearchCondition
            {
                OrderBy = EnumBrokerSearchOrderBy.OrderById
            };
            return PageHelper.toJson(_brokerService.GetBrokersByCondition(condition).ToList());
        }

        /// <summary>
        /// 传入经纪人参数,获取经纪人,返回经纪人信息
        /// </summary>
        /// <param name="brokerModel">经纪人参数</param>
        /// <returns>经纪人信息</returns>
        [Description("检索经纪人信息")]
        public HttpResponseMessage GetAdmin([FromBody] BrokerModel brokerModel)
        {
            return PageHelper.toJson(_brokerService.GetBrokerById(brokerModel.Id));
        }

        /// <summary>
        /// 传入经纪人参数,添加或者修改经纪人信息,返回添加或者修改结果状态信息,成功添加返回"添加成功",添加失败返回"添加失败",成功修改返回"修改成功",修改失败返回"修改失败"
        /// </summary>
        /// <param name="brokerModel">经纪人参数</param>
        /// <returns>添加或修改经纪人结果状态信息</returns>
        [Description("添加或者修改经纪人信息")]
        public HttpResponseMessage AddAdmin([FromBody] BrokerModel brokerModel)
        {
            var model = new BrokerEntity
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
        /// 传入管理员ID,删除管理员,返回删除结果状态信息
        /// </summary>
        /// <param name="id">管理员Id</param>
        /// <returns>删除结果状态信息</returns>
        /// 
        [Description("管理员删除")]
        public HttpResponseMessage DelAdmin([FromBody]int id)
        {
            try
            {
                var model = new BrokerEntity
                {
                    Id = id,
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
        /// 传入管理员Id,注销管理员,返回注销结果状态信息
        /// </summary>
        /// <param name="id">管理员Id</param>
        /// <returns>管理员注销结果状态信息</returns>
        [Description("管理员注销")]
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
