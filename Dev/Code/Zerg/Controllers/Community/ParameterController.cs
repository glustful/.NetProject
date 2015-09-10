using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.Parameter;
using Community.Service.Parameter;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
	public class ParameterController : ApiController
	{
		private readonly IParameterService _parameterService;

		public ParameterController(IParameterService parameterService)
		{
			_parameterService = parameterService;
		}

		public ParameterModel Get(int id)
		{
			var entity =_parameterService.GetParameterById(id);
			var model = new ParameterModel
			{
				Id = entity.Id,
//                Category = entity.Category,	
                Name = entity.Name,			
                Sort = entity.Sort,			
                AddUser = entity.AddUser,		
                AddTime = entity.AddTime,			
                UpdUser = entity.UpdUser,				
                UpdTime = entity.UpdTime,		
            };
			return model;
		}

		public List<ParameterModel> Get(ParameterSearchCondition condition)
		{
			var model = _parameterService.GetParametersByCondition(condition).Select(c=>new ParameterModel
			{
				Id = c.Id,
//				Category = c.Category,
				Name = c.Name,
				Sort = c.Sort,
				AddUser = c.AddUser,
				AddTime = c.AddTime,
				UpdUser = c.UpdUser,
				UpdTime = c.UpdTime,
			}).ToList();
			return model;
		}

		public bool Post(ParameterModel model)
		{
			var entity = new ParameterEntity
			{
//				Category = model.Category,
				Name = model.Name,
				Sort = model.Sort,
				AddUser = model.AddUser,
				AddTime = model.AddTime,
				UpdUser = model.UpdUser,
				UpdTime = model.UpdTime,
			};
			if(_parameterService.Create(entity).Id > 0)
			{
				return true;
			}
			return false;
		}

		public bool Put(ParameterModel model)
		{
			var entity = _parameterService.GetParameterById(model.Id);
			if(entity == null)
				return false;
//			entity.Category = model.Category;
			entity.Name = model.Name;
			entity.Sort = model.Sort;
			entity.AddUser = model.AddUser;
			entity.AddTime = model.AddTime;
			entity.UpdUser = model.UpdUser;
			entity.UpdTime = model.UpdTime;
			if(_parameterService.Update(entity) != null)
				return true;
			return false;
		}

		public bool Delete(int id)
		{
			var entity = _parameterService.GetParameterById(id);
			if(entity == null)
				return false;
			if(_parameterService.Delete(entity))
				return true;
			return false;
		}
	}
}