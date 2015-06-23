using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.PointDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Zerg.Common;
using System.ComponentModel;

namespace Zerg.Controllers.CRM
{
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    /// <summary>
    /// 积分详情管理  李洪亮  2015-05-05
    /// </summary>
    [Description("积分管理类")]
    public class PointDetailController : ApiController
    {

        private IPointDetailService _pointdetailService;
        private readonly IBrokerService _brokerService;
        /// <summary>
        /// 积分管理初始化
        /// </summary>
        /// <param name="pointdetailService">pointdetailService</param>
        /// <param name="brokerService">brokerService</param>
        public PointDetailController(IPointDetailService pointdetailService, IBrokerService brokerService)
        {
            _pointdetailService = pointdetailService;
            _brokerService = brokerService;
        }

        #region 积分详情

        /// <summary>
        /// 传入经纪人ID，查询经纪人积分详情，返回经纪人积分想详情
        /// </summary>
        /// <param name="userId">经纪人ID</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面数量</param>
        /// <returns>经纪人积分详情</returns>
        [Description("查询返回经纪人积分详情")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetPointDetailByUserId(string userId = null, int page = 1, int pageSize = 10)
        {
            var pointdetailCon = new PointDetailSearchCondition
            {
                Brokers = _brokerService.GetBrokerById(Convert.ToInt32(userId)),
                Page = Convert.ToInt32(page),
                PageCount = pageSize
            };

            var PointDetailList = _pointdetailService.GetPointDetailsByCondition(pointdetailCon).Select(p => new
            {
                Id = p.Id,
                p.Addpoints,
                p.Pointsds,
                p.Addtime

            }).ToList().Select(p => new
            {
                Id = p.Id,
                p.Addpoints,
                p.Pointsds,
                Addtime = p.Addtime.ToString("yyyy-MM-dd")

            });
            var PointDetailListCount = _pointdetailService.GetPointDetailCount(pointdetailCon);
            return PageHelper.toJson(new { List = PointDetailList, Condition = pointdetailCon, totalCount = PointDetailListCount });

        }

        /// <summary>
        /// 传入积分参数，新增积分详情，返回新增积分详情结果状态信息
        /// </summary>
        /// <param name="pointDetail">积分参数</param>
        /// <returns>新增结果状态信息</returns>
        [Description("查询返回经纪人积分详情")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddPointDetail([FromBody] PointDetailEntity pointDetail)
        {

            if (pointDetail != null)
            {
                var entity = new PointDetailEntity
                {
                    Addpoints = pointDetail.Addpoints,
                    Broker = null,
                    Pointsds = pointDetail.Pointsds,
                    Totalpoints = pointDetail.Totalpoints,
                    Uptime = DateTime.Now,
                    Addtime = DateTime.Now,

                };

                try
                {
                    if (_pointdetailService.Create(entity) != null)
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
