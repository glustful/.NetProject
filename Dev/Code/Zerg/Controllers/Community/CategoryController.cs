using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.Category;
using Community.Service.Category;
using Zerg.Models.Community;
using System;
using System.Net.Http;
using Zerg.Common;
using System.EnterpriseServices;
using System.Text.RegularExpressions;
using System.Web.Http.Cors;

namespace Zerg.Controllers.Community
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
	public class CategoryController : ApiController
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}
        #region 商品分类管理 2015.9.9 黄秀宇
        /// <summary>
        /// 根据id查找商品分类
        /// </summary>
        /// <param name="id">商品分类id</param>
        /// <returns></returns>
        [Description("根据id查找商品分类")]
        public HttpResponseMessage Get(int id)
		{
			var entity =_categoryService.GetCategoryById(id);
          if(entity!=null)
          { 
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
			return PageHelper.toJson(model);
          }
          return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不存在！"));
		}
        /// <summary>
        /// 根据条件查找商品类别
        /// </summary>
        /// <param name="condition">Name商品类别名称，Sorts排序类型</param>
        /// <returns></returns>
        [Description("根据条件查找商品类别")]
      public HttpResponseMessage Get([FromUri]CategorySearchCondition condition)
		{
            //验证是否有非法字符
            Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");

            if (!string.IsNullOrEmpty(condition.Name))
            {
                var m = reg.IsMatch(condition .Name);
                if (!m)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "搜索输入存在非法字符！"));
                }
            }
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
            return PageHelper.toJson(model);
		}
        /// <summary>
        /// 添加商品分类
        /// </summary>
        /// <param name="model">Name商品分类名称，Sort分类排序</param>
        /// <returns></returns>
        [Description("添加商品分类")]
        public HttpResponseMessage Post(CategoryModel model)
		{
			var entity = new CategoryEntity
			{
//				Father = model.Father,
				Name = model.Name,
				Sort = model.Sort,
				AddUser = model.AddUser,
                AddTime = DateTime.Now,
				UpdUser = model.UpdUser,
                UpdTime = DateTime.Now,
			};
			if(_categoryService.Create(entity).Id > 0)
			{
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！"));
			}
            return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加失败！"));
		}
        /// <summary>
        /// 修改商品分类
        /// </summary>
        /// <param name="model">Name商品分类名称，Sort分类排序</param>
        /// <returns></returns>
        [Description("修改商品分类")]
		public HttpResponseMessage Put(CategoryModel model)
		{
			var entity = _categoryService.GetCategoryById(model.Id);
			if(entity == null)
				return PageHelper .toJson (PageHelper .ReturnValue(false ,"不存在该数据！修改失败"));
//			entity.Father = model.Father;
			entity.Name = model.Name;
			entity.Sort = model.Sort;
			entity.AddUser = model.AddUser;
			//entity.AddTime = model.AddTime;
			entity.UpdUser = model.UpdUser;
			entity.UpdTime = DateTime.Now;
            if (_categoryService.Update(entity) != null)
            { return PageHelper.toJson(PageHelper.ReturnValue(true, "修改成功")); }
			return PageHelper .toJson (PageHelper .ReturnValue (false ,"修改失败"));
		}
        /// <summary>
        /// 删除商品分类
        /// </summary>
        /// <param name="id">商品分类id</param>
        /// <returns></returns>
        [Description("根据id删除商品分类")]
		public HttpResponseMessage Delete(int id)
		{
			var entity = _categoryService.GetCategoryById(id);
			if(entity == null)
				return PageHelper .toJson (PageHelper .ReturnValue (false ,"删除失败"));
			if(_categoryService.Delete(entity))
                return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));
            return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败"));
        }
        #endregion
    }
}