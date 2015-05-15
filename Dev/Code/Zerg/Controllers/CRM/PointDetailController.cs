﻿using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.PointDetail;
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
    /// 积分详情管理  李洪亮  2015-05-05
    /// </summary>
    public class PointDetailController : ApiController
    {

        private IPointDetailService _pointdetailService;
        private readonly IBrokerService _brokerService;
        public PointDetailController(IPointDetailService pointdetailService, IBrokerService brokerService)
        {
            _pointdetailService =pointdetailService;
            _brokerService =brokerService;
        }

        #region 积分详情

      /// <summary>
      /// 查询经纪人的积分详情
      /// </summary>
      /// <param name="userId">经纪人ID</param>
      /// <returns></returns>

        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetPointDetailByUserId(string userId)
        {
            var pointdetailCon = new PointDetailSearchCondition
            {
               Brokers=_brokerService.GetBrokerById(Convert.ToInt32(userId))
            };
            return PageHelper.toJson(_pointdetailService.GetPointDetailsByCondition(pointdetailCon).ToList());
        }

        /// <summary>
        /// 新增积分详情
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddPointDetail([FromBody] PointDetailEntity pointDetail)
        {

            if (pointDetail!=null)
            {
                var entity = new PointDetailEntity
                {
                    Addpoints=pointDetail.Addpoints,
                    Broker=null,
                    Pointsds=pointDetail.Pointsds,
                    Totalpoints=pointDetail.Totalpoints,
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