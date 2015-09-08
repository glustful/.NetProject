using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.Area;
using Community.Service.Area;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
	public class AreaController : ApiController
	{
		private readonly IAreaService _areaService;

		public AreaController(IAreaService areaService)
		{
			_areaService = areaService;
		}

		public AreaModel Get(int id)
		{
			var entity =_areaService.GetAreaById(id);
			var model = new AreaModel
			{
				Id = entity.Id,	
                Codeid = entity.CodeId,	
                Adddate = entity.AddDate,	
//                Parent = entity.Parent,	
                Name = entity.Name,	
            };
			return model;
		}

		public List<AreaModel> Get(AreaSearchCondition condition)
		{
			var model = _areaService.GetAreasByCondition(condition).Select(c=>new AreaModel
			{
				Id = c.Id,
				Codeid = c.CodeId,
				Adddate = c.AddDate,
//				Parent = c.Parent,
				Name = c.Name,
			}).ToList();
			return model;
		}

		public bool Post(AreaModel model)
		{
			var entity = new AreaEntity
			{
				CodeId = model.Codeid,
				AddDate = model.Adddate,
//				Parent = model.Parent,
				Name = model.Name,
			};
			if(_areaService.Create(entity).Id > 0)
			{
				return true;
			}
			return false;
		}

		public bool Put(AreaModel model)
		{
			var entity = _areaService.GetAreaById(model.Id);
			if(entity == null)
				return false;
			entity.CodeId = model.Codeid;
			entity.AddDate = model.Adddate;
//			entity.Parent = model.Parent;
			entity.Name = model.Name;
			if(_areaService.Update(entity) != null)
				return true;
			return false;
		}

		public bool Delete(int id)
		{
			var entity = _areaService.GetAreaById(id);
			if(entity == null)
				return false;
			if(_areaService.Delete(entity))
				return true;
			return false;
		}
	}
}