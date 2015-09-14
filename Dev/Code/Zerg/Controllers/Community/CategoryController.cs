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
        #region ��Ʒ������� 2015.9.9 ������

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
        //           //���ҵ�һ����
        //        }
        //    }

        //    return System.Web.Mvc.Json("", System.Web.Mvc.JsonRequestBehavior.AllowGet);
        //}


        /// <summary>
        /// ����id������Ʒ����
        /// </summary>
        /// <param name="id">��Ʒ����id</param>
        /// <returns></returns>
        [Description("����id������Ʒ����")]
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
//			entity.Father = model.Father;
			entity.Name = model.Name;
			entity.Sort = model.Sort;
			entity.AddUser = model.AddUser;
			//entity.AddTime = model.AddTime;
			entity.UpdUser = model.UpdUser;
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
            //var entity = _categoryService.GetCategoryById(id);
            //if(entity == null)
            //    return PageHelper .toJson (PageHelper .ReturnValue (false ,"ɾ��ʧ��"));
            //if(_categoryService.Delete(entity))
            //    return PageHelper.toJson(PageHelper.ReturnValue(true, "ɾ���ɹ�"));
            //return PageHelper.toJson(PageHelper.ReturnValue(false, "ɾ��ʧ��"));
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
        }


        /// <summary>
        /// ���϶��»�ȡ��״���ڵ��б�
        /// </summary>
        /// <returns>��״���ڵ��б�</returns>
        [Description("���϶��»�ȡ��״���ڵ��б�")]
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
                TJM.children = GetJsonFromTreeModel(TJM.Id);
            }
            return treeJsonModelBuffer;
        }

        /// <summary>
        /// �Ե�����ȡ�������ӽڵ㣻
        /// </summary>
        /// <param name="nodeId">�ڵ�ID</param>
        /// <returns>�������ӽڵ�</returns>
        [Description("�Ե�����ȡ�������ӽڵ㣻")]
        public List<TreeJsonModel> GetJsonFromTreeModel(int nodeId)
        {
            CategorySearchCondition csc = new CategorySearchCondition()
            {
                OrderBy = EnumCategorySearchOrderBy.OrderById
            };
            List<TreeJsonModel> datalist = new List<TreeJsonModel>();
            List<CategoryEntity> ceList = _categoryService.GetCategorysBySuperFather(nodeId).ToList();//�ҳ��ü����Ӽ���
            foreach (var ce in ceList)
            {
                TreeJsonModel TJM = new TreeJsonModel()
                {
                    label = ce.Name,
                    Id = ce.Id
                };
                datalist.Add(TJM);
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