using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.ProductDetail;
using Community.Service.ProductDetail;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
	public class ProductDetailController : ApiController
	{
		private readonly IProductDetailService _productDetailService;

		public ProductDetailController(IProductDetailService productDetailService)
		{
			_productDetailService = productDetailService;
		}

		public ProductDetailModel Get(int id)
		{
			var entity =_productDetailService.GetProductDetailById(id);
			var model = new ProductDetailModel
			{
				Id = entity.Id,		
                Name = entity.Name,	
                Detail = entity.Detail,		
                Img = entity.Img,	
                Img1 = entity.Img1,		
                Img2 = entity.Img2,				
                Img3 = entity.Img3,				
                Img4 = entity.Img4,				
                SericeInstruction = entity.SericeInstruction,	
                AddUser = entity.AddUser,		
                AddTime = entity.AddTime,			
                UpdUser = entity.UpdUser,			
                UpdTime = entity.UpdTime,			
                Ad1 = entity.Ad1,		
                Ad2 = entity.Ad2,			
                Ad3 = entity.Ad3,		
            };
			return model;
		}

		public List<ProductDetailModel> Get(ProductDetailSearchCondition condition)
		{
			var model = _productDetailService.GetProductDetailsByCondition(condition).Select(c=>new ProductDetailModel
			{
				Id = c.Id,
				Name = c.Name,
				Detail = c.Detail,
				Img = c.Img,
				Img1 = c.Img1,
				Img2 = c.Img2,
				Img3 = c.Img3,
				Img4 = c.Img4,
				SericeInstruction = c.SericeInstruction,
				AddUser = c.AddUser,
				AddTime = c.AddTime,
				UpdUser = c.UpdUser,
				UpdTime = c.UpdTime,
				Ad1 = c.Ad1,
				Ad2 = c.Ad2,
				Ad3 = c.Ad3,
			}).ToList();
			return model;
		}

		public bool Post(ProductDetailModel model)
		{
			var entity = new ProductDetailEntity
			{
				Name = model.Name,
				Detail = model.Detail,
				Img = model.Img,
				Img1 = model.Img1,
				Img2 = model.Img2,
				Img3 = model.Img3,
				Img4 = model.Img4,
				SericeInstruction = model.SericeInstruction,
				AddUser = model.AddUser,
				AddTime = model.AddTime,
				UpdUser = model.UpdUser,
				UpdTime = model.UpdTime,
				Ad1 = model.Ad1,
				Ad2 = model.Ad2,
				Ad3 = model.Ad3,
			};
			if(_productDetailService.Create(entity).Id > 0)
			{
				return true;
			}
			return false;
		}

		public bool Put(ProductDetailModel model)
		{
			var entity = _productDetailService.GetProductDetailById(model.Id);
			if(entity == null)
				return false;
			entity.Name = model.Name;
			entity.Detail = model.Detail;
			entity.Img = model.Img;
			entity.Img1 = model.Img1;
			entity.Img2 = model.Img2;
			entity.Img3 = model.Img3;
			entity.Img4 = model.Img4;
			entity.SericeInstruction = model.SericeInstruction;
			entity.AddUser = model.AddUser;
			entity.AddTime = model.AddTime;
			entity.UpdUser = model.UpdUser;
			entity.UpdTime = model.UpdTime;
			entity.Ad1 = model.Ad1;
			entity.Ad2 = model.Ad2;
			entity.Ad3 = model.Ad3;
			if(_productDetailService.Update(entity) != null)
				return true;
			return false;
		}

		public bool Delete(int id)
		{
			var entity = _productDetailService.GetProductDetailById(id);
			if(entity == null)
				return false;
			if(_productDetailService.Delete(entity))
				return true;
			return false;
		}
	}
}