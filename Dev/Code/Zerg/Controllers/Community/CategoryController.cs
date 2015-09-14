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

        //public System.Web.Mvc.JsonResult Get()
        //{
        //     CategorySearchCondition  csc = new CategorySearchCondition()
        //    {
        //        OrderBy = EnumCategorySearchOrderBy.OrderById,                
        //    };
        //    List<CategoryEntity> listCategofyOne = _categoryService.GetCategorysByCondition(csc).Where(o=>o.Father==null).ToList();
        //    foreach (var p in listCategofyOne)
        //    {
        //        if (p.Father == null)
        //        {
        //           //查找第一级；
        //        }
        //    }

        //    return System.Web.Mvc.Json("", System.Web.Mvc.JsonRequestBehavior.AllowGet);
        //}


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
      public HttpResponseMessage Get(CategorySearchCondition condition)
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
			//	Father = c.Father,
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
            //var entity = new CategoryEntity
            //{
            // //   FatherId = model.FatherId,
            //    Name = model.Name,
            //    Sort = model.Sort,
            //    AddUser = model.AddUser,
            //    AddTime = DateTime.Now,
            //    UpdUser = model.UpdUser,
            //    UpdTime = DateTime.Now,
            //};
            //if(_categoryService.Create(entity).Id > 0)
            //{
            //    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！"));
            //}
            //return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加失败！"));
            Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
            var m = reg.IsMatch(model.Name);
            if (!m)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "存在非法字符！"));
            }
            CategoryEntity superCe = _categoryService.GetCategoryById(model.Id);
            int sort = 0;
            if (superCe != null)//有上级分类则次级排序加1；
            {
                sort = superCe.Sort + 1;
            }
            CategoryEntity ce = new CategoryEntity()
            {
                Father = superCe,
                Name = model.Name,
                Sort = sort,
                AddTime = DateTime.Now,
                // Adduser = classify.Adduser,
                //AddUser = _workContext.CurrentUser.Id.ToString(),
                AddUser =1,
                UpdTime = DateTime.Now,
                //UpdUser = _workContext.CurrentUser.Id.ToString()
                  UpdUser = 1
                //Upduser = classify.Upduser

            };
            try
            {
                _categoryService.Create(ce);
                return PageHelper.toJson(PageHelper.ReturnValue(true, "添加分类成功！"));
            }
            catch (Exception error)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "添加分类失败！")); ;
            }
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
            //var entity = _categoryService.GetCategoryById(id);
            //if(entity == null)
            //    return PageHelper .toJson (PageHelper .ReturnValue (false ,"删除失败"));
            //if(_categoryService.Delete(entity))
            //    return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));
            //return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败"));
            try
            {
                if (_categoryService.Delete(_categoryService.GetCategoryById(id)))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功！"));
                }
                else
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "存在子分类关联不能删除！"));
                }

            }
            catch (Exception e)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败！"));
            }
        }
        /// <summary>
        /// 获取所有分类(使用Angular中Tree的数据格式)；
        /// </summary>
        /// <param name="pageindex">当前翻页页数</param>
        /// <returns>查询结果</returns>

        [Description("获取所有分类(使用Angular中Tree的数据格式)")]
        [System.Web.Http.HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage GetAllClassify()
        {
            CategorySearchCondition csc = new CategorySearchCondition()
            {
                OrderBy = EnumCategorySearchOrderBy.OrderById
            };
            return PageHelper.toJson(GetAllTree().ToList());
        }
        #region 获取支持angularjs的tree模型列表；
        /// <summary>
        /// 支持angularjs中tree插件的模型；
        /// </summary>
        [Description("支持angularjs中tree插件的模型")]
        public class TreeJsonModel
        {
            public string label { set; get; }
            public List<TreeJsonModel> children { set; get; }
            public int Id { set; get; }
        }


        /// <summary>
        /// 自上而下获取树状根节点列表；
        /// </summary>
        /// <returns>树状根节点列表</returns>
        [Description("自上而下获取树状根节点列表")]
        public List<TreeJsonModel> GetAllTree()
        {
            CategorySearchCondition  csc = new CategorySearchCondition()
            {
                OrderBy = EnumCategorySearchOrderBy.OrderById
            };
            List<CategoryEntity> ceListBuffer = new List<CategoryEntity>();
            List<TreeJsonModel> treeJsonModelBuffer = new List<TreeJsonModel>();
            List<CategoryEntity> ceList = _categoryService.GetCategorysByCondition(csc).ToList();
            foreach (var ce in ceList)
            {
                if (ce.Father == null)
                {
                    ceListBuffer.Add(ce);//查找第一级；
                }
            }
            foreach (var ce in ceListBuffer)
            {
                TreeJsonModel TJM = new TreeJsonModel()
                {
                    label = ce.Name,
                    Id = ce.Id
                };
                treeJsonModelBuffer.Add(TJM);
                TJM.children = GetJsonFromTreeModel(TJM.Id);
            }
            return treeJsonModelBuffer;
        }

        /// <summary>
        /// 自迭代获取所有树子节点；
        /// </summary>
        /// <param name="nodeId">节点ID</param>
        /// <returns>所有树子节点</returns>
        [Description("自迭代获取所有树子节点；")]
        public List<TreeJsonModel> GetJsonFromTreeModel(int nodeId)
        {
            CategorySearchCondition csc = new CategorySearchCondition()
            {
                OrderBy = EnumCategorySearchOrderBy.OrderById
            };
            List<TreeJsonModel> datalist = new List<TreeJsonModel>();
            List<CategoryEntity> ceList = _categoryService.GetCategorysBySuperFather(nodeId).ToList();//找出该级的子集；
            foreach (var ce in ceList)
            {
                TreeJsonModel TJM = new TreeJsonModel()
                {
                    label = ce.Name,
                    Id = ce.Id
                };
                datalist.Add(TJM);
                TJM.children = GetJsonFromTreeModel(TJM.Id);//自迭代;

            }
            if (ceList.Count == 0)//若遍历到末端，则：
            {
                return null;
            }
            else
            {
                return datalist;
            }
        }
        #endregion

        #region 获取树状json格式的分类参数
        ///// <summary>
        ///// 获取树状json格式的分类参数
        ///// </summary>
        ///// <param name="classifyId">分类ID</param>
        ///// <returns>树状json格式的分类参数</returns>
        //[Description("获取树状json格式的分类参数")]
        //[System.Web.Http.HttpGet]
        //[EnableCors("*", "*", "*", SupportsCredentials = true)]
        //public HttpResponseMessage GetParameterTreeData(int classifyId)
        //{

        //    List<ParameterTreeModel> PTMList = new List<ParameterTreeModel>();
        //    var PList = _parameterService.GetParameterEntitysByClassifyId(classifyId);
        //    foreach (var p in PList)
        //    {
        //        ParameterTreeModel PT = new ParameterTreeModel()
        //        {
        //            Name = p.Name,
        //            Id = p.Id
        //        };
        //        List<ParameterValueEntity> PVList = _parameterValueService.GetParameterValuesByParameter(p.Id).ToList();
        //        List<ParameterValueTreeModel> PVTMList = new List<ParameterValueTreeModel>();
        //        foreach (var pv in PVList)
        //        {
        //            ParameterValueTreeModel PVTM = new ParameterValueTreeModel()
        //            {
        //                Value = pv.Parametervalue,
        //                Id = pv.Id
        //            };
        //            PVTMList.Add(PVTM);
        //        };
        //        PT.ValueList = PVTMList;
        //        PTMList.Add(PT);
        //    }
        //    return PageHelper.toJson(PTMList.ToList());

        //}
        /// <summary>
        /// 分类模型
        /// </summary>
        [Description("分类模型")]
        public class ParameterTreeModel
        {
            public string Name { set; get; }//分类名称；
            public int Id { set; get; }//分类Id；
            public List<ParameterValueTreeModel> ValueList { set; get; }//分类参数值；
        }
        /// <summary>
        /// 分类值模型
        /// </summary>
        [Description("分类值模型")]
        public class ParameterValueTreeModel
        {
            public string Value { set; get; }//分类值名称；
            public int Id { set; get; }//分类值Id；
        }
        #endregion

        #endregion
    }
}