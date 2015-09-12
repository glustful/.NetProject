using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Community.Entity.Model.Product;
using Community.Entity.Model.ProductDetail;
using Community.Service.Category;
using Community.Service.Product;
using Community.Service.ProductDetail;
using Zerg.Common;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
	public class CommunityProductController : ApiController
	{
		private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IProductDetailService _productDetailService;

        public CommunityProductController(IProductService productService,ICategoryService categoryService,IProductDetailService productDetailService)
		{
			_productService = productService;
		    _categoryService = categoryService;
            _productDetailService = productDetailService;
		}
        /// <summary>
        /// ��ȡ��Ʒ����
        /// </summary>
        /// <param name="id">��ƷId</param>
        /// <returns>��Ʒʵ��</returns>
        public HttpResponseMessage Get(int id)
		{
			var entity =_productService.GetProductById(id);
            var comment = entity.Comments;           
            List<ProductCommentModel> commentList;
            if (comment == null)
            {
                commentList = new List<ProductCommentModel>();
            }
            else
            {
                commentList = (from c in comment
                        select new ProductCommentModel
                        {
                            Id = c.Id,
                            Content = c.Content,
                            AddTime = c.AddTime,
                            AddUser = c.AddUser
                        }).ToList();
            }
			var model = new ProductModel
			{
				Id = entity.Id,	
                CategoryId = entity.Category.Id,	
                BussnessId= entity.BussnessId,	
                BussnessName = entity.BussnessName,	
                Price = entity.Price,		
                Name = entity.Name,			
                Status = entity.Status,		
                MainImg = entity.MainImg,
		        Img = entity.Detail.Img,
                Img1 = entity.Detail.Img1,
                Img2 = entity.Detail.Img2,
                Img3 = entity.Detail.Img3,
                Img4 = entity.Detail.Img4,
                IsRecommend = entity.IsRecommend,
                Sort = entity.Sort,				
                Stock = entity.Stock,		            	
                Subtitte = entity.Subtitte,				
                Contactphone = entity.Contactphone,
                SericeInstruction = entity.Detail.SericeInstruction,
                Type = entity.Type,
			    NewPrice = entity.NewPrice,
                Owner = entity.Owner,
                Detail = entity.Detail.Detail,
		        Ad1 = entity.Detail.Ad1,
                Ad2 = entity.Detail.Ad2,
                Ad3 = entity.Detail.Ad3,
                //Comments = entity.Comments,		
                ParameterValue =entity.Parameters.Select(c => new ProductParameterValueModel { ParameterId = c.Parameter.Id, ParameterString = c.Parameter.Name, ValueId = c.ParameterValue.Id, Value = c.ParameterValue.Value}).ToArray(),
            };
            var product=new ProductComment
            {
                ProductModel = model,
                Comments = commentList
            };
			return PageHelper.toJson(product);
		}
        /// <summary>
        /// ��ȡ��Ʒ�б�
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <returns>��Ʒ�б�</returns>
        public HttpResponseMessage Get([FromUri]ProductSearchCondition condition)
		{
			var model = _productService.GetProductsByCondition(condition).Select(c=>new ProductModel
			{
				Id = c.Id,
				CategoryId = c.Category.Id,
				BussnessId = c.BussnessId,
				BussnessName = c.BussnessName,
				Price = c.Price,
				Name = c.Name,
				Status = c.Status,
				MainImg = c.MainImg,
				IsRecommend = c.IsRecommend,
				Sort = c.Sort,
				Stock = c.Stock,
				Subtitte = c.Subtitte,
				Contactphone = c.Contactphone,
				Type = c.Type,
                NewPrice = c.NewPrice,
                Owner =c.Owner,
                Addtime = c.AddTime,
				Detail = c.Detail.Detail
//				Comments = c.Comments,
//				Parameters = c.Parameters,
			}).ToList();
            var totalCount = _productService.GetProductCount(condition);
            return PageHelper.toJson(new { List = model, Condition = condition, TotalCount = totalCount });
		}
        /// <summary>
        /// ������Ʒ
        /// </summary>
        /// <param name="model">��Ʒmodel</param>
        /// <returns>��ʾ��Ϣ</returns>
		public HttpResponseMessage Post(ProductModel model)
		{
		    var category = _categoryService.GetCategoryById(model.CategoryId);
			var entity = new ProductEntity
			{
                Category = category,
				BussnessId = model.BussnessId,
				BussnessName =model.BussnessName,
				Price = model.Price,
				Name = model.Name,
				Status = model.Status,
				MainImg = model.MainImg,
				IsRecommend =model.IsRecommend,
				Sort = model.Sort,
				Stock = model.Stock,
				AddUser = 1,
				AddTime =DateTime.Now,
				UpdUser =1,
				UpdTime = DateTime.Now,
				Subtitte = model.Subtitte,
				Contactphone = model.Contactphone,
				Type =model.Type,
                NewPrice =model.NewPrice,
                Owner = model.Owner
			   // Detail = model.Detail,
				//Comments = model.Comments,
//				Parameters = model.Parameters,
			};
		    int id = _productService.Create(entity).Id; 
			if(id> 0)
			{               
				var productDetail = new ProductDetailEntity
               {
                    Id = id,
                    Name = model.Name,
                    Detail = model.Detail,
                    Img = model.Img,
                    Img1 = model.Img1,
                    Img2 = model.Img2,
                    Img3 = model.Img3,
                    Img4 = model.Img4,
                    SericeInstruction = model.SericeInstruction,
                    AddUser = 1,
                    AddTime = DateTime.Now,
                    UpdUser = 1,
                    UpdTime = DateTime.Now,
                    Ad1 = model.Ad1,
                    Ad2 = model.Ad2,
                    Ad3 = model.Ad3,
                };              
                if (_productDetailService.Create(productDetail).Id>0)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true,"�������ӳɹ�"));
                }
			    return PageHelper.toJson(PageHelper.ReturnValue(false,"��Ʒ��ϸ����ʧ��"));
			}            
			return PageHelper.toJson(PageHelper.ReturnValue(false,"��������ʧ��"));
		}
        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <param name="model">��Ʒmodel</param>
        /// <returns>��ʾ��Ϣ</returns>
		public HttpResponseMessage Put(ProductModel model)
		{
			var entity = _productService.GetProductById(model.Id);
			if(entity == null)
				return PageHelper.toJson(PageHelper.ReturnValue(false,"���ݲ�����"));
            var category = _categoryService.GetCategoryById(model.CategoryId);
            entity.Category = category;
			entity.BussnessId = model.BussnessId;
			entity.BussnessName = model.BussnessName;
			entity.Price = model.Price;
			entity.Name = model.Name;
			entity.Status = model.Status;
			entity.MainImg = model.MainImg;
			entity.IsRecommend = model.IsRecommend;
			entity.Sort = model.Sort;
		    entity.Stock = model.Stock;	
			entity.UpdUser = 1;
			entity.UpdTime = DateTime.Now;
			entity.Subtitte = model.Subtitte;
			entity.Contactphone = model.Contactphone;
			entity.Type = model.Type;
            entity.NewPrice = model.NewPrice;
            entity.Owner = model.Owner;
//			entity.Detail = model.Detail;
			//entity.Comments = model.Comments;
//			entity.Parameters = model.Parameters;
		    if (_productService.Update(entity) != null)
		    {
		        var productDetail = _productDetailService.GetProductDetailById(model.Id);
                productDetail.Name = model.Name;
                productDetail.Detail = model.Detail;
                productDetail.Img = model.Img;
                productDetail.Img1 = model.Img1;
                productDetail.Img2 = model.Img2;
                productDetail.Img3 = model.Img3;
                productDetail.Img4 = model.Img4;
                productDetail.SericeInstruction = model.SericeInstruction;              
                productDetail.UpdUser =1;
                productDetail.UpdTime =DateTime.Now;
                productDetail.Ad1 = model.Ad1;
                productDetail.Ad2 = model.Ad2;
                productDetail.Ad3 = model.Ad3;
                if (_productDetailService.Update(productDetail) != null)
                    return PageHelper.toJson(PageHelper.ReturnValue(true,"���ݸ��³ɹ�"));
                return PageHelper.toJson(PageHelper.ReturnValue(false,"��Ʒ��ϸ����ʧ��"));
		    }
			return PageHelper.toJson(PageHelper.ReturnValue(false,"���ݸ���ʧ��"));
		}
        /// <summary>
        /// ɾ����Ʒ
        /// </summary>
        /// <param name="id">��ƷId</param>
        /// <returns>��ʾ��Ϣ</returns>
		public HttpResponseMessage Delete(int id)
        {
            var productDetail = _productDetailService.GetProductDetailById(id);
            if (productDetail == null)
                return PageHelper.toJson(PageHelper.ReturnValue(false, "��Ʒ��ϸ������"));            
                if (_productDetailService.Delete(productDetail))
                {
                    var entity = _productService.GetProductById(id);
                    if (entity == null)
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "���ݲ�����"));                                       
                        if (_productService.Delete(entity))
                            return PageHelper.toJson(PageHelper.ReturnValue(true, "����ɾ���ɹ�"));                   
                }
                return PageHelper.toJson(PageHelper.ReturnValue(true, "����ɾ��ʧ��"));
           
        }
	}
}