using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Community.Entity.Model.Parameter;
using Community.Entity.Model.ParameterValue;
using Community.Entity.Model.ProductParameter;
using Community.Service.Category;
using Community.Service.Parameter;
using Community.Service.ParameterValue;
using Community.Service.Product;
using Community.Service.ProductParameter;
using YooPoon.Core.Site;
using Zerg.Common;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    public class ProductParameterController : ApiController
    {
        private readonly IProductParameterService _productParameterService;
        private readonly IParameterService _parameterService;
        private readonly IParameterValueService _parameterValueService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWorkContext _workContext;

        public ProductParameterController(IProductParameterService productParameterService, IParameterService parameterService,
            IParameterValueService parameterValueService, IProductService productService, ICategoryService categoryService,IWorkContext workContext)
        {
            _productParameterService = productParameterService;
            _parameterService = parameterService;
            _parameterValueService = parameterValueService;
            _productService = productService;
            _categoryService = categoryService;
            _workContext = workContext;
        }

        //        public ProductParameterModel Get(int id)
        //        {
        //            var entity =_productParameterService.GetProductParameterById(id);
        //            var model = new ProductParameterModel
        //            {
        //                Id = entity.Id,				
        ////                ParameterValue = entity.ParameterValue,			
        ////                Parameter = entity.Parameter,		
        ////                Product = entity.Product,		
        //                Sort = entity.Sort,		
        //                AddUser = entity.AddUser,	
        //                AddTime = entity.AddTime,	
        //                UpdUser = entity.UpdUser,	
        //                UpdTime = entity.UpdTime,		
        //            };
        //            return model;
        //        }

        //public HttpResponseMessage Get(ProductParameterSearchCondition condition)
        //{
        //    var model = _productParameterService.GetProductParametersByCondition(condition).Select(c=>new ProductParameterModel
        //    {
        //        Id = c.Id,

        //        Sort = c.Sort,
        //        AddUser = c.AddUser,
        //        AddTime = c.AddTime,
        //        UpdUser = c.UpdUser,
        //        UpdTime = c.UpdTime,
        //    }).ToList();
        //    return PageHelper.toJson(model);
        //}

        public HttpResponseMessage Post(ProductParameterModel model)
        {
            var parameterValue = _parameterValueService.GetParameterValueById(model.ParameterValueId);
            var parameter = _parameterService.GetParameterById(model.ParameterId);
            var product = _productService.GetProductById(model.Id);
            var entity = new ProductParameterEntity
            {
                ParameterValue = parameterValue,
                Parameter = parameter,
                Product = product,
                AddUser = model.AddUser,
                AddTime = model.AddTime,
                UpdUser = model.UpdUser,
                UpdTime = model.UpdTime,
            };
            if (_productParameterService.Create(entity).Id > 0)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "������ӳɹ�"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "�������ʧ��"));
        }

        public HttpResponseMessage Put(ProductParameterModel model)
        {
            var product = _productService.GetProductById(model.ProductId);
            if (product == null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "��Ʒ������"));
            }
            //TODO:�жϿ�ֵ
            var parameterValue = _parameterValueService.GetParameterValuesByCondition(new ParameterValueSearchCondition { Ids = model.ValueIds }).ToList().Select(pv => new ProductParameterEntity
            {
                Product = product,
                AddTime = DateTime.Now,
                AddUser = _workContext.CurrentUser.Id,//TODO:�޸�Ϊ��ǰ�û�
                Parameter = pv.Parameter,
                ParameterValue = pv,
                Sort = 0,
                UpdTime = DateTime.Now,
                UpdUser = _workContext.CurrentUser.Id //TODO:�޸�Ϊ��ǰ�û�
            }).ToList();            
            _productParameterService.BulkCreate(parameterValue);
            return PageHelper.toJson(PageHelper.ReturnValue(true, "���ݸ��³ɹ�"));                             
        }

        public bool Delete(int id)
        {
            var entity = _productParameterService.GetProductParameterById(id);
            if (entity == null)
                return false;
            if (_productParameterService.Delete(entity))
                return true;
            return false;
        }

        /// <summary>
        /// ͨ������ID��ȡ��������Ӧ�Ĳ���ֵ�б�
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(int categoryId)
        {
            #region ���ݲ���ֵ���ѯ
            //var category = _categoryService.GetCategoryById(categoryId);
            //var parvalueSecon = new ParameterValueSearchCondition
            //{
            //    OrderBy = EnumParameterValueSearchOrderBy.OrderById            
            //};
            //var value = _parameterValueService.GetParameterValuesByCondition(parvalueSecon).Where(p=>p.Parameter.Category.Id==categoryId).Select(p => new 
            //{
            //    Id = p.Id,
            //    ParameterName=p.Parameter.Name,
            //    ParameterId=p.Parameter.Id,
            //    ParameterValue = p.Value,              
            //}).ToList();
            #endregion

            #region ���ݲ������ѯ

            var conParameter = new ParameterSearchCondition
            {
                OrderBy = EnumParameterSearchOrderBy.OrderById
            };

            var value = _parameterService.GetParametersByCondition(conParameter).Where(o => o.Category.Id == categoryId).Select(p => new
            {
                Id = p.Id,
                ParameterName = p.Name,
                ParameterValues = p.Values.Select(o => new { id = o.Id, name = o.Value }).ToList(),
                value = ""
            }).ToList();
            #endregion

            return PageHelper.toJson(value);
        }
    }
}