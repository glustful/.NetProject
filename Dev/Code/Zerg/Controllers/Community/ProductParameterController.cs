using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Community.Entity.Model.ProductParameter;
using Community.Service.Parameter;
using Community.Service.ParameterValue;
using Community.Service.Product;
using Community.Service.ProductParameter;
using Zerg.Common;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
	public class ProductParameterController : ApiController
	{
		private readonly IProductParameterService _productParameterService;
	    private readonly IParameterService _parameterService;
	    private readonly IParameterValueService _parameterValueService;
	    private readonly IProductService _productService;

	    public ProductParameterController(IProductParameterService productParameterService,IParameterService parameterService,IParameterValueService parameterValueService,IProductService productService)
		{
			_productParameterService = productParameterService;
		    _parameterService = parameterService;
		    _parameterValueService = parameterValueService;
		    _productService = productService;
		}

		public ProductParameterModel Get(int id)
		{
			var entity =_productParameterService.GetProductParameterById(id);
			var model = new ProductParameterModel
			{
				Id = entity.Id,				
//                ParameterValue = entity.ParameterValue,			
//                Parameter = entity.Parameter,		
//                Product = entity.Product,		
                Sort = entity.Sort,		
                AddUser = entity.AddUser,	
                AddTime = entity.AddTime,	
                UpdUser = entity.UpdUser,	
                UpdTime = entity.UpdTime,		
            };
			return model;
		}

		public HttpResponseMessage Get(ProductParameterSearchCondition condition)
		{
			var model = _productParameterService.GetProductParametersByCondition(condition).Select(c=>new ProductParameterModel
			{
				Id = c.Id,

				Sort = c.Sort,
				AddUser = c.AddUser,
				AddTime = c.AddTime,
				UpdUser = c.UpdUser,
				UpdTime = c.UpdTime,
			}).ToList();
			return PageHelper.toJson(model);
		}

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
            if(_productParameterService.Create(entity).Id > 0)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true,"属性添加成功"));
            }
			return PageHelper.toJson(PageHelper.ReturnValue(false,"数据添加失败"));
		}

		public HttpResponseMessage Put(ProductParameterModel model)
		{
            var parameterValue = _parameterValueService.GetParameterValueById(model.ParameterValueId);
            var parameter = _parameterService.GetParameterById(model.ParameterId);
            var product = _productService.GetProductById(model.Id);
			var entity = _productParameterService.GetProductParameterById(model.Id);
			if(entity == null)
				return PageHelper.toJson(PageHelper.ReturnValue(false,"数据不存在"));
            entity.ParameterValue = parameterValue;
            entity.Parameter = parameter;
            entity.Product = product;
			entity.Sort = model.Sort;
			entity.AddUser = model.AddUser;
			entity.AddTime = model.AddTime;
			entity.UpdUser = model.UpdUser;
			entity.UpdTime = model.UpdTime;
			if(_productParameterService.Update(entity) != null)
				return PageHelper.toJson(PageHelper.ReturnValue(true,"数据更新成功"));
			return PageHelper.toJson(PageHelper.ReturnValue(false,"数据更新失败"));
		}

		public bool Delete(int id)
		{
			var entity = _productParameterService.GetProductParameterById(id);
			if(entity == null)
				return false;
			if(_productParameterService.Delete(entity))
				return true;
			return false;
		}
	}
}