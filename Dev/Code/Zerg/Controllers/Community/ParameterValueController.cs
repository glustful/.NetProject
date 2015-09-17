using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.ParameterValue;
using Community.Service.ParameterValue;
using Zerg.Models.Community;
using System.Net.Http;

using System;
using Community.Service.Parameter;
using Zerg.Common;
using System.Web.Http.Cors;

namespace Zerg.Controllers.Community
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
	public class ParameterValueController : ApiController
	{
		private readonly IParameterValueService _parameterValueService;
        private readonly IParameterService _parameterService;

        public ParameterValueController(IParameterValueService parameterValueService, IParameterService parameterService)
		{
			_parameterValueService = parameterValueService;
            _parameterService = parameterService;
		}

		public ParameterValueModel Get(int id)
		{
			var entity =_parameterValueService.GetParameterValueById(id);
			var model = new ParameterValueModel
			{
				Id = entity.Id,
//                Parameter = entity.Parameter,
//                ParameterValue = entity.ParameterValue,	
                Sort = entity.Sort,			
                AddUser = entity.AddUser,	
                AddTime = entity.AddTime,	
                UpdUser = entity.UpdUser,	
                UpdTime = entity.UpdTime,		
            };
			return model;
		}

        public HttpResponseMessage Get(string ParameterId)
        {
            var condition = new ParameterValueSearchCondition
            {
                Parameters = _parameterService.GetParameterById(Convert.ToInt32(ParameterId))
            };
            var model = _parameterValueService.GetParameterValuesByCondition(condition).Select(c => new
            {
                Id = c.Id,                
                ParameterName = c.Parameter.Name,
                Value = c.Value,
                Sort = c.Sort,
                AddTime = c.AddTime
            }).ToList();
            return PageHelper.toJson(model);
        } 


	

		public  HttpResponseMessage Post(ParameterValueModel model)
		{

              if(String.IsNullOrEmpty(model.ParameterValue) || model.Id==0)
            {
               return PageHelper.toJson(PageHelper.ReturnValue(false, "数据异常！")); 
            }

              var entity = new ParameterValueEntity
			{
                Parameter =_parameterService.GetParameterById(model.Id),//添加时候 这里的Id传的是参数ID         
                Value = model.ParameterValue,
				Sort = model.Sort,				
				AddTime =DateTime.Now,
                UpdTime =DateTime.Now,
			};
              if (_parameterValueService.Create(entity).Id > 0)
			{
                return PageHelper.toJson(PageHelper.ReturnValue(true, "添加成功！"));
			}
            return PageHelper.toJson(PageHelper.ReturnValue(false, "添加失败！")); 


		
		}

		public bool Put(ParameterValueModel model)
		{
			var entity = _parameterValueService.GetParameterValueById(model.Id);
			if(entity == null)
				return false;
//			entity.Parameter = model.Parameter;
//			entity.ParameterValue = model.ParameterValue;
			entity.Sort = model.Sort;
			entity.AddUser = model.AddUser;
			entity.AddTime = model.AddTime;
			entity.UpdUser = model.UpdUser;
			entity.UpdTime = model.UpdTime;
			if(_parameterValueService.Update(entity) != null)
				return true;
			return false;
		}

        [HttpGet]
        public HttpResponseMessage Delete(int id)
		{
			var entity = _parameterValueService.GetParameterValueById(id);
			if(entity == null)
		    return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败！"));
			if(_parameterValueService.Delete(entity))
                return PageHelper.toJson(PageHelper.ReturnValue(true, "删除失败！"));
            return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败！"));
		}


     
	}
}