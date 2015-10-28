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
        #region ��Ʒ������� 2015.9.9 ������

        /// <summary>
        /// ����id������Ʒ������Ϣ
        /// </summary>
        /// <param name="id">��Ʒ����id</param>
        /// <returns></returns>
        [Description("����id������Ʒ������Ϣ")]
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
          return PageHelper.toJson(PageHelper.ReturnValue(false, "���ݲ����ڣ�"));
		}

        /// <summary>
        /// ���ݸ�����Ʒ����id��������Ʒ���࣬����0ʱ�򲻴�ֵ�ǻ�ȡһ������
        /// </summary>
        /// <param name="id">��Ʒ����id</param>
        /// <returns></returns>
        [Description("���ݸ�����Ʒ����id��������Ʒ����")]
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
            return PageHelper.toJson(PageHelper.ReturnValue(false, "���ݲ����ڣ�"));
        }
        /// <summary>
        /// ��������������Ʒ���
        /// </summary>
        /// <param name="condition">Name��Ʒ������ƣ�Sorts��������</param>
        /// <returns></returns>
        [Description("��������������Ʒ���")]
      public HttpResponseMessage Get(CategorySearchCondition condition)
		{
            //��֤�Ƿ��зǷ��ַ�
            Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");

            if (!string.IsNullOrEmpty(condition.Name))
            {
                var m = reg.IsMatch(condition .Name);
                if (!m)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "����������ڷǷ��ַ���"));
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
        /// �����Ʒ����
        /// </summary>
        /// <param name="model">Name��Ʒ�������ƣ�Sort��������</param>
        /// <returns></returns>
        [Description("�����Ʒ����")]
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
            //    return PageHelper.toJson(PageHelper.ReturnValue(true, "������ӳɹ���"));
            //}
            //return PageHelper.toJson(PageHelper.ReturnValue(true, "�������ʧ�ܣ�"));
            Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
            var m = reg.IsMatch(model.Name);
            if (!m)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "���ڷǷ��ַ���"));
            }
            CategoryEntity superCe = _categoryService.GetCategoryById(model.Id);
            int sort = 0;
            if (superCe != null)//���ϼ�������μ������1��
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
                return PageHelper.toJson(PageHelper.ReturnValue(true, "��ӷ���ɹ���"));
            }
            catch (Exception error)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "��ӷ���ʧ�ܣ�")); ;
            }
		}
        /// <summary>
        /// �޸���Ʒ����
        /// </summary>
        /// <param name="model">Name��Ʒ�������ƣ�Sort��������</param>
        /// <returns></returns>
        [Description("�޸���Ʒ����")]
		public HttpResponseMessage Put(CategoryModel model)
		{
			var entity = _categoryService.GetCategoryById(model.Id);
			if(entity == null)
				return PageHelper .toJson (PageHelper .ReturnValue(false ,"�����ڸ����ݣ��޸�ʧ��"));
			entity.Name = model.Name;
			entity.Sort = model.Sort;
            //entity.UpdUser = _workContent.CurrentUser.Id;
            entity.UpdUser = 1;
			entity.UpdTime = DateTime.Now;
            if (_categoryService.Update(entity) != null)
            { return PageHelper.toJson(PageHelper.ReturnValue(true, "�޸ĳɹ�")); }
			return PageHelper .toJson (PageHelper .ReturnValue (false ,"�޸�ʧ��"));
		}
        /// <summary>
        /// ɾ����Ʒ����
        /// </summary>
        /// <param name="id">��Ʒ����id</param>
        /// <returns></returns>
        [Description("����idɾ����Ʒ����")]
		public HttpResponseMessage Delete(int id)
		{
            try
            {
                if (_categoryService.Delete(_categoryService.GetCategoryById(id)))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "ɾ���ɹ���"));
                }
                else
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "�����ӷ����������ɾ����"));
                }

            }
            catch (Exception e)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "ɾ��ʧ�ܣ�"));
            }
        }
        /// <summary>
        /// ��ȡ���з���(ʹ��Angular��Tree�����ݸ�ʽ)��
        /// </summary>
        /// <param name="pageindex">��ǰ��ҳҳ��</param>
        /// <returns>��ѯ���</returns>

        [Description("��ȡ���з���(ʹ��Angular��Tree�����ݸ�ʽ)")]
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
        #region ��ȡ֧��angularjs��treeģ���б�
        /// <summary>
        /// ֧��angularjs��tree�����ģ�ͣ�
        /// </summary>
        [Description("֧��angularjs��tree�����ģ��")]
        public class TreeJsonModel
        {
            public string label { set; get; }
            public List<TreeJsonModel> children { set; get; }
            public int Id { set; get; }
            /// <summary>
            /// ������
            /// </summary>
            public virtual CategoryEntity Father { get; set; }
            public List<Product> product { get; set; }
        }
        /// <summary>
        /// ��Ʒ��
        /// </summary>
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string MainImg { get; set; }
        }

        /// <summary>
        /// ���϶��»�ȡ��״���ڵ��б�
        /// </summary>
        /// <returns>��״���ڵ��б�</returns>
        [Description("���϶��»�ȡ��״���ڵ��б�")]
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
                    ceListBuffer.Add(ce);//���ҵ�һ����
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
                if(ifid==0)//���ifidΪ0����ôֱ�����ҵ����һ�����࣬����ֻ�鵽��һ��
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
        /// ���ҷ����6����Ʒ��
        /// </summary>
        /// <returns>��״���ڵ��б�</returns>
        [Description("���ҷ����6����Ʒ")]
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
                    ceListBuffer.Add(ce);//���ҵ�һ����
                }
            }
            foreach (var ce in ceListBuffer)
            {
                List<CategoryEntity> ceList1 = _categoryService.GetCategorysBySuperFather(ce.Id).ToList();//�ҳ��ü����Ӽ���
                TreeJsonModel TJM1=null;
                foreach (var ce1 in ceList1)
                {
                     TJM1 = new TreeJsonModel()
                    {
                        label = ce1.Name,
                        Id = ce1.Id,
                        //Father = ce1.Father
                    };
                     TJM1.children = GetJsonModel(ce1.Id);//��ȡ����������������µ�ǰ6����Ʒ
                     treeJsonModelBuffer.Add(TJM1);

                }
                    
                
            }
            return treeJsonModelBuffer;
        }
        /// <summary>
        /// ����������������������µ�������Ʒ��
        /// </summary>
        /// <param name="nodeId">�ڵ�ID</param>
        /// <returns>�������ӽڵ�</returns>
        [Description("����������������������µ�������Ʒ")]
        public List<TreeJsonModel> GetJsonModel(int nodeId)
        {
            CategorySearchCondition csc = new CategorySearchCondition()
            {
                OrderBy = EnumCategorySearchOrderBy.OrderById
            };
            List<TreeJsonModel> datalist = new List<TreeJsonModel>();
            List<CategoryEntity> ceList = _categoryService.GetCategorysBySuperFather(nodeId).ToList();//�ҳ��ü����Ӽ���
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
                TJM.product = GetSixPro(TJM.Id);//�Ե���;

            }
            if (ceList.Count == 0)//��������ĩ�ˣ���
            {
                return null;
            }
            else
            {
                return datalist;
            }
        }
        /// <summary>
        /// ��ȡ6����Ʒ
        /// </summary>
        /// <param name="id">����id</param>
        /// <returns>��Ʒ�б�</returns>
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
        /// �Ե�����ȡ�������ӽڵ㣻
        /// </summary>
        /// <param name="nodeId">�ڵ�ID</param>
        /// <returns>�������ӽڵ�</returns>
        [Description("�Ե�����ȡ�������ӽڵ㣻")]
        public List<TreeJsonModel> GetJsonFromTreeModel(int nodeId,int ifid=0)
        {
            CategorySearchCondition csc = new CategorySearchCondition()
            {
                OrderBy = EnumCategorySearchOrderBy.OrderById
            };
            List<TreeJsonModel> datalist = new List<TreeJsonModel>();
            List<CategoryEntity> ceList = _categoryService.GetCategorysBySuperFather(nodeId).ToList();//�ҳ��ü����Ӽ���
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
                TJM.children = GetJsonFromTreeModel(TJM.Id);//�Ե���;

            }
            if (ceList.Count == 0)//��������ĩ�ˣ���
            {
                return null;
            }
            else
            {
                return datalist;
            }
        }
        #endregion

        #region ��ȡ��״json��ʽ�ķ������
        ///// <summary>
        ///// ��ȡ��״json��ʽ�ķ������
        ///// </summary>
        ///// <param name="classifyId">����ID</param>
        ///// <returns>��״json��ʽ�ķ������</returns>
        //[Description("��ȡ��״json��ʽ�ķ������")]
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
        /// ����ģ��
        /// </summary>
        [Description("����ģ��")]
        public class ParameterTreeModel
        {
            public string Name { set; get; }//�������ƣ�
            public int Id { set; get; }//����Id��
            public List<ParameterValueTreeModel> ValueList { set; get; }//�������ֵ��
        }
        /// <summary>
        /// ����ֵģ��
        /// </summary>
        [Description("����ֵģ��")]
        public class ParameterValueTreeModel
        {
            public string Value { set; get; }//����ֵ���ƣ�
            public int Id { set; get; }//����ֵId��
        }
        #endregion

        #endregion
    }
}