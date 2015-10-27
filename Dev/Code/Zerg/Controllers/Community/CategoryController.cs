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
using YooPoon.Core.Site;
using Community.Entity.Model.Product;
using Community.Service.Product;


namespace Zerg.Controllers.Community
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
	public class CategoryController : ApiController
	{
		private readonly ICategoryService _categoryService;
        private readonly IWorkContext _workContent;
        private readonly IProductService _productService;
        public CategoryController(ICategoryService categoryService, IWorkContext workContent,IProductService productService)
		{
			_categoryService = categoryService;
            _workContent = workContent;
            _productService = productService;
		}
        #region 商品分类管理 2015.9.9 黄秀宇

        /// <summary>
        /// 根据id查找商品分类信息
        /// </summary>
        /// <param name="id">商品分类id</param>
        /// <returns></returns>
        [Description("根据id查找商品分类信息")]
        public HttpResponseMessage Get(int id)
		{
			var entity =_categoryService.GetCategoryById(id);
          if(entity!=null)
          { 
			var model = new CategoryModel
			{
				Id = entity.Id,
                Father = entity.Father,
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
        /// 根据父级商品分类id查找子商品分类，传递0时或不传值是获取一级分类
        /// </summary>
        /// <param name="id">商品分类id</param>
        /// <returns></returns>
        [Description("根据父级商品分类id查找子商品分类")]
        public HttpResponseMessage GetChildByFatherId(int id=0)
        {
            var entity = _categoryService.GetCategorysBySuperFather(id);
            if (entity != null)
            {
              var model=  entity.Select(q => new CategoryModel
                {
                    Id = q.Id,
                    //Father = entity.Father,
                    Name = q.Name,
                    Sort = q.Sort,
                    AddUser = q.AddUser,
                    AddTime = q.AddTime,
                    UpdUser = q.UpdUser,
                    UpdTime = q.UpdTime
                });
              
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
               // AddUser = _workContent.CurrentUser.Id,
               AddUser =1,
                UpdTime = DateTime.Now,
               // UpdUser = _workContent.CurrentUser.Id
               UpdUser =1

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
			entity.Name = model.Name;
			entity.Sort = model.Sort;
            //entity.UpdUser = _workContent.CurrentUser.Id;
            entity.UpdUser = 1;
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
            /// <summary>
            /// 父分类
            /// </summary>
            public virtual CategoryEntity Father { get; set; }
            public List<Product> product { get; set; }
        }
        /// <summary>
        /// 商品类
        /// </summary>
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string MainImg { get; set; }
        }

        /// <summary>
        /// 自上而下获取树状根节点列表；
        /// </summary>
        /// <returns>树状根节点列表</returns>
        [Description("自上而下获取树状根节点列表")]
        public List<TreeJsonModel> GetAllTree(int ifid=0)
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
                if(ifid==0)//如果ifid为0，那么直到查找到最后一级子类，否则只查到下一级
                {
                TJM.children = GetJsonFromTreeModel(TJM.Id);
                }
                else
                {
                    TJM.children = GetJsonFromTreeModel(TJM.Id,1);
                }
            }
            return treeJsonModelBuffer;
        }

        /// <summary>
        /// 查找分类跟6个商品；
        /// </summary>
        /// <returns>树状根节点列表</returns>
        [Description("查找分类跟6个商品")]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        [HttpGet]
        public List<TreeJsonModel> GetCateANDPro()
        {
            CategorySearchCondition csc = new CategorySearchCondition()
            {
                OrderBy = EnumCategorySearchOrderBy.OrderById,
                father ="NULL"
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
                List<CategoryEntity> ceList1 = _categoryService.GetCategorysBySuperFather(ce.Id).ToList();//找出该级的子集；
                TreeJsonModel TJM1=null;
                foreach (var ce1 in ceList1)
                {
                     TJM1 = new TreeJsonModel()
                    {
                        label = ce1.Name,
                        Id = ce1.Id,
                        //Father = ce1.Father
                    };
                     TJM1.children = GetJsonModel(ce1.Id);//获取第三级分类跟分类下的前6个商品
                     treeJsonModelBuffer.Add(TJM1);

                }
                    
                
            }
            return treeJsonModelBuffer;
        }
        /// <summary>
        /// 第三级分类跟第三集分类下的六个商品；
        /// </summary>
        /// <param name="nodeId">节点ID</param>
        /// <returns>所有树子节点</returns>
        [Description("第三级分类跟第三集分类下的六个商品")]
        public List<TreeJsonModel> GetJsonModel(int nodeId)
        {
            CategorySearchCondition csc = new CategorySearchCondition()
            {
                OrderBy = EnumCategorySearchOrderBy.OrderById
            };
            List<TreeJsonModel> datalist = new List<TreeJsonModel>();
            List<CategoryEntity> ceList = _categoryService.GetCategorysBySuperFather(nodeId).ToList();//找出该级的子集；
            int i = 0;
            foreach (var ce in ceList)
            {
                TreeJsonModel TJM = new TreeJsonModel()
                {
                    label = ce.Name,
                    Id = ce.Id,
                    // Father  =ce.Father
                };
                datalist.Add(TJM);
                TJM.product = GetSixPro(TJM.Id);//自迭代;

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
        /// <summary>
        /// 获取6个商品
        /// </summary>
        /// <param name="id">分类id</param>
        /// <returns>商品列表</returns>
        public List <Product> GetSixPro(int id)
        {
            var con = new ProductSearchCondition
            {
                Page =1,
                PageCount =6,
               // Name = condition.Name,
                IsDescending = true,
               // OrderBy = condition.OrderBy,
               // PriceBegin = condition.PriceBegin,
               //PriceEnd = condition.PriceEnd,
               // CategoryName = condition.CategoryName,
                CategoryId=id
            };
            List<Product> data = new List<Product>();
            var model = _productService.GetProductsByCondition(con).Select(c => new ProductModel
            {
                Id = c.Id,
                Name = c.Name,
                MainImg = c.MainImg,
            }).ToList();
            foreach (var pro in model)
            {
                Product produce = new Product
                {
                    Id = pro.Id,
                    Name =pro.Name ,
                    MainImg =pro.MainImg 
                };
                data.Add(produce);
            }
            return data;
        }
        /// <summary>
        /// 自迭代获取所有树子节点；
        /// </summary>
        /// <param name="nodeId">节点ID</param>
        /// <returns>所有树子节点</returns>
        [Description("自迭代获取所有树子节点；")]
        public List<TreeJsonModel> GetJsonFromTreeModel(int nodeId,int ifid=0)
        {
            CategorySearchCondition csc = new CategorySearchCondition()
            {
                OrderBy = EnumCategorySearchOrderBy.OrderById
            };
            List<TreeJsonModel> datalist = new List<TreeJsonModel>();
            List<CategoryEntity> ceList = _categoryService.GetCategorysBySuperFather(nodeId).ToList();//找出该级的子集；
            int i = 0;
            foreach (var ce in ceList)
            {
                TreeJsonModel TJM = new TreeJsonModel()
                {
                    label = ce.Name,
                    Id = ce.Id,
                   // Father  =ce.Father
                };
                datalist.Add(TJM);
                if(ifid==0)
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