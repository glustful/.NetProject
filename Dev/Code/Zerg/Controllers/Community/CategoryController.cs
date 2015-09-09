using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.Category;
using Community.Service.Category;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
	public class CategoryController : ApiController
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		public CategoryModel Get(int id)
		{
			var entity =_categoryService.GetCategoryById(id);
			var model = new CategoryModel
			{
				Id = entity.Id,
//                Father = entity.Father,
                Name = entity.Name,
                Sort = entity.Sort,
                AddUser = entity.AddUser,
                AddTime = entity.AddTime,
                UpdUser = entity.UpdUser,
                UpdTime = entity.UpdTime
            };
			return model;
		}

		public List<CategoryModel> Get(CategorySearchCondition condition)
		{
			var model = _categoryService.GetCategorysByCondition(condition).Select(c=>new CategoryModel
			{
				Id = c.Id,
//				Father = c.Father,
				Name = c.Name,
				Sort = c.Sort,
				AddUser = c.AddUser,
				AddTime = c.AddTime,
				UpdUser = c.UpdUser,
				UpdTime = c.UpdTime,
			}).ToList();
			return model;
		}

		public bool Post(CategoryModel model)
		{
			var entity = new CategoryEntity
			{
//				Father = model.Father,
				Name = model.Name,
				Sort = model.Sort,
				AddUser = model.AddUser,
				AddTime = model.AddTime,
				UpdUser = model.UpdUser,
				UpdTime = model.UpdTime,
			};
			if(_categoryService.Create(entity).Id > 0)
			{
				return true;
			}
			return false;
		}

		public bool Put(CategoryModel model)
		{
			var entity = _categoryService.GetCategoryById(model.Id);
			if(entity == null)
				return false;
//			entity.Father = model.Father;
			entity.Name = model.Name;
			entity.Sort = model.Sort;
			entity.AddUser = model.AddUser;
			entity.AddTime = model.AddTime;
			entity.UpdUser = model.UpdUser;
			entity.UpdTime = model.UpdTime;
			if(_categoryService.Update(entity) != null)
				return true;
			return false;
		}

		public bool Delete(int id)
		{
			var entity = _categoryService.GetCategoryById(id);
			if(entity == null)
				return false;
			if(_categoryService.Delete(entity))
				return true;
			return false;
		}
	}
}