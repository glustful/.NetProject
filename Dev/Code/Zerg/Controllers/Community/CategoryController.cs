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
      public HttpResponseMessage Get([FromUri]CategorySearchCondition condition)
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
        /// �����Ʒ����
        /// </summary>
        /// <param name="model">Name��Ʒ�������ƣ�Sort��������</param>
        /// <returns></returns>
        [Description("�����Ʒ����")]
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
                return PageHelper.toJson(PageHelper.ReturnValue(true, "������ӳɹ���"));
			}
            return PageHelper.toJson(PageHelper.ReturnValue(true, "�������ʧ�ܣ�"));
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
			var entity = _categoryService.GetCategoryById(id);
			if(entity == null)
				return PageHelper .toJson (PageHelper .ReturnValue (false ,"ɾ��ʧ��"));
			if(_categoryService.Delete(entity))
                return PageHelper.toJson(PageHelper.ReturnValue(true, "ɾ���ɹ�"));
            return PageHelper.toJson(PageHelper.ReturnValue(false, "ɾ��ʧ��"));
        }
        #endregion
    }
}