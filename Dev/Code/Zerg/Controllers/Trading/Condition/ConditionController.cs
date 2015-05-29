using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Trading.Service.Parameter;
using Trading.Entity.Entity.Area;
using Trading.Service.Area;
using Trading.Entity.Model;
using Trading.Service.ParameterValue;
using Zerg.Common;

namespace Zerg.Controllers.Trading.Type
{
    public class TypeController : ApiController
    {
        private readonly IParameterService _parameterService;
        private readonly IAreaService _areaService;
        private readonly IParameterValueService _parameterValueService;


        public TypeController(IParameterService parameterService,IAreaService areaService,IParameterValueService parameterValueService)
        {
            _parameterService = parameterService;
            _areaService = areaService;
            _parameterValueService = parameterValueService;
        }

        public HttpResponseMessage GetCondition(int parentId=0)
        {           
            var areaCon = new AreaSearchCondition
            {
                ParentId = parentId
            };
            var areaList = _areaService.GetAreaByCondition(areaCon).Select(a=>new
            {             
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
