using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Trading.Entity.Entity.Area;
using Trading.Service.Area;
using Zerg.Common;

namespace Zerg.Controllers.Trading.Area
{
    public class AreaController : ApiController
    {
        private readonly IAreaService _areaService;

        public AreaController(IAreaService areaService)
        {
            _areaService = areaService;
        }

        public HttpResponseMessage GetArea(int parentId=0)
        {
            var areaCon = new AreaSearchCondition
            {
                ParentId = parentId
            };
            var areaList = _areaService.GetAreaByCondition(areaCon).Select(a=>new
            {
                Id=a.Id,
                AreaName=a.AreaName
            }).ToList();
            return PageHelper.toJson(areaList);
        }
    }
}
