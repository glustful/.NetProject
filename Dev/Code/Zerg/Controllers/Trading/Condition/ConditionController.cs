using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Trading.Service.Parameter;
using Trading.Entity.Entity.Area;
using Trading.Entity.Model;
using Trading.Service.ParameterValue;
using Zerg.Common;
using Trading.Service.Area;
using System.ComponentModel;

namespace Zerg.Controllers.Trading.Condition
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)] 
    [Description("状态管理类")]
    public class ConditionController : ApiController
    {
        private readonly IParameterService _parameterService;
        private readonly IAreaService _areaService;
        private readonly IParameterValueService _parameterValueService;

        /// <summary>
        /// 状态管理初始化
        /// </summary>
        /// <param name="parameterService">parameterService</param>
        /// <param name="areaService">areaService</param>
        /// <param name="parameterValueService">parameterValueService</param>
        public ConditionController(IParameterService parameterService, IAreaService areaService, IParameterValueService parameterValueService)
        {
            _parameterService = parameterService;
            _areaService = areaService;
            _parameterValueService = parameterValueService;
        }
        /// <summary>
        /// 传入参数，获取状态信息
        /// </summary>
        /// <param name="parentId">ID</param>
        /// <returns>状态信息</returns>
        [Description("获取状态")]
        [HttpGet]
        public HttpResponseMessage GetCondition(int parentId=0)
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
            var typeCon = new ParameterSearchCondition
            {
                Name="户型"
            };
            var typeList = _parameterService.GetParametersByCondition(typeCon).SelectMany(p => p.Values).Select(v=>new
            {
                TypeId = v.Id,
                TypeName = v.Parametervalue
            }).ToList();
            return PageHelper.toJson(new { AreaList = areaList, TypeList = typeList });
        }
    }
}
