using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.ParameterValue;
using Community.Service.ParameterValue;
using Zerg.Models.Community;
using System.Net.Http;

namespace Zerg.Controllers.Community
{
	public class ParameterValueController : ApiController
	{
		private readonly IParameterValueService _parameterValueService;

		public ParameterValueController(IParameterValueService parameterValueService)
		{
			_parameterValueService = parameterValueService;
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

		public List<ParameterValueModel> Get(ParameterValueSearchCondition condition)
		{
			var model = _parameterValueService.GetParameterValuesByCondition(condition).Select(c=>new ParameterValueModel
			{
				Id = c.Id,
//				Parameter = c.Parameter,
//				ParameterValue = c.ParameterValue,
				Sort = c.Sort,
				AddUser = c.AddUser,
				AddTime = c.AddTime,
				UpdUser = c.UpdUser,
				UpdTime = c.UpdTime,
			}).ToList();
			return model;
		}

		public bool Post(ParameterValueModel model)
		{
			var entity = new ParameterValueEntity
			{
//				Parameter = model.Parameter,
//				ParameterValue = model.ParameterValue,
				Sort = model.Sort,
				AddUser = model.AddUser,
				AddTime = model.AddTime,
				UpdUser = model.UpdUser,
				UpdTime = model.UpdTime,
			};
			if(_parameterValueService.Create(entity).Id > 0)
			{
				return true;
			}
			return false;
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

		public bool Delete(int id)
		{
			var entity = _parameterValueService.GetParameterValueById(id);
			if(entity == null)
				return false;
			if(_parameterValueService.Delete(entity))
				return true;
			return false;
		}


     
	}
}