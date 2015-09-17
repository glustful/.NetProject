using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.Parameter;
using Community.Service.Parameter;
using Zerg.Models.Community;
using System;
using System.Net.Http;
using Zerg.Common;
using System.EnterpriseServices;
using System.Text.RegularExpressions;
using System.Web.Http.Cors;
using Community.Service.Category;
namespace Zerg.Controllers.Community
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
	public class ParameterController : ApiController
	{
		private readonly IParameterService _parameterService;
        private readonly ICategoryService _categoryService;

        public ParameterController(IParameterService parameterService, ICategoryService categoryService)
		{
			_parameterService = parameterService;
            _categoryService = categoryService;
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

        public HttpResponseMessage Get(string CategoryId)
        {
            var condition = new ParameterSearchCondition
            {
                Category = _categoryService.GetCategoryById(Convert.ToInt32(CategoryId))
            };
            var model = _parameterService.GetParametersByCondition(condition).Select(c => new
            {
                Id = c.Id,
                CategoryName = c.Category.Name,
                Name = c.Name,
                Sort = c.Sort,
                AddTime = c.AddTime
            }).ToList();
            return PageHelper.toJson(model);
        }

        public HttpResponseMessage Get(ParameterSearchCondition condition)
		{      
			var model = _parameterService.GetParametersByCondition(condition).Select(c=>new
			{
				Id = c.Id,
				CategoryName = c.Category.Name,
				Name = c.Name,
				Sort = c.Sort,			
				AddTime = c.AddTime				
			}).ToList();
            return PageHelper.toJson(model);
		}

        /// <summary>
        /// ��Ӳ���
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
		public HttpResponseMessage Post(ParameterModel model)
		{
            if(String.IsNullOrEmpty(model.Name) || model.Id==0)
            {
               return PageHelper.toJson(PageHelper.ReturnValue(false, "�����쳣��")); 
            }
            
			var entity = new ParameterEntity
			{
                Category = _categoryService.GetCategoryById(model.Id),//���ʱ�� �����Id�����Ƿ���ID         
				Name = model.Name,
				Sort = model.Sort,				
				AddTime =DateTime.Now,
                UpdTime =DateTime.Now,
			};
			if(_parameterService.Create(entity).Id > 0)
			{
                return PageHelper.toJson(PageHelper.ReturnValue(true, "��ӳɹ���"));
			}
            return PageHelper.toJson(PageHelper.ReturnValue(false, "���ʧ�ܣ�")); 
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

		

        [HttpGet]
        public HttpResponseMessage Delete(int id)
        {
            var entity = _parameterService.GetParameterById(id);
            if (entity == null)
                return PageHelper.toJson(PageHelper.ReturnValue(false, "ɾ��ʧ�ܣ�"));
            if (_parameterService.Delete(entity))
                return PageHelper.toJson(PageHelper.ReturnValue(true, "ɾ��ʧ�ܣ�"));
            return PageHelper.toJson(PageHelper.ReturnValue(false, "ɾ��ʧ�ܣ�"));
        }
        
	}
}