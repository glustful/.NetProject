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
using YooPoon.Core.Site;
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
        private readonly IWorkContext _workContext;

        public CommunityProductController(IProductService productService,ICategoryService categoryService,IProductDetailService productDetailService,IWorkContext workContext)
		{
			_productService = productService;
		    _categoryService = categoryService;
            _productDetailService = productDetailService;
            _workContext = workContext;
		}
        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <returns>商品实体</returns>
        public HttpResponseMessage Get(int id)
		{
			var entity =_productService.GetProductById(id);
            if (entity == null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不存在"));
            }
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
                            //AddUser = c.AddUser
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
			    OldPrice = entity.OldPrice,
                Owner = entity.Owner,
                Detail = entity.Detail.Detail,
		        Ad1 = entity.Detail.Ad1,
                Ad2 = entity.Detail.Ad2,
                Ad3 = entity.Detail.Ad3,
                //Comments = entity.Comments,		
                //ParameterValue =entity.Parameters.Select(c => new ProductParameterValueModel
                //{
                //    ParameterId = c.Parameter.Id,
                //    ParameterString = c.Parameter.Name,
                //    ValueId = c.ParameterValue.Id,
                //    Value = c.ParameterValue.Value
                //}).ToList()
            };
            var product=new ProductComment
            {
                ProductModel = model,
                Comments = commentList
            };
			return PageHelper.toJson(product);
		}
        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>商品列表</returns>
        public HttpResponseMessage Get([FromUri]ProductSearchCondition condition)
        {
            var con = new ProductSearchCondition
            {
                Page = condition.Page,
                PageCount = condition.PageCount,
               Name = condition.Name,
               IsDescending = condition.IsDescending,
               OrderBy = condition.OrderBy,
               PriceBegin = condition.PriceBegin,
               PriceEnd = condition.PriceEnd,
               CategoryName=condition.CategoryName
            };
            if (condition.CategoryId!=0 && condition.CategoryId!=null)
            {
                var category = _categoryService.GetCategoryById(Convert.ToInt32(condition.CategoryId));
                if (category.Father.Father == null)//判断是否是二级
                {
                    //var firstOrDefault = _categoryService.GetCategorysBySuperFather(category.Id).FirstOrDefault();
                    //if (firstOrDefault != null)
                    //    con.CategoryId = firstOrDefault.Id;
                    con.Categorys = category;//_categoryService.GetCategorysBySuperFather(category.Id).ToArray();
                }
                else//3级
                {
                    con.CategoryId = condition.CategoryId;
                }
            }

            var model = _productService.GetProductsByCondition(con).Select(c => new ProductModel
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
                OldPrice = c.OldPrice,
                Owner =c.Owner,
                Addtime = c.AddTime,
				Detail = c.Detail.Detail,
                Ad1 = c.Detail.Ad1
//				Comments = c.Comments,
//				Parameters = c.Parameters,
                //ParameterValue =c.Parameters.Select(p => new ProductParameterValueModel
                //{
                //    ParameterId = p.Parameter.Id,
                //    ParameterString = p.Parameter.Name,
                //    ValueId = p.ParameterValue.Id,
                //    Value = p.ParameterValue.Value
                //}).ToList()
			}).ToList();
            var totalCount = _productService.GetProductCount(con);
            return PageHelper.toJson(new { List = model, Condition = con, TotalCount = totalCount });
		}
        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="model">商品model</param>
        /// <returns>提示信息</returns>
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
				AddUser = _workContext.CurrentUser.Id,
				AddTime =DateTime.Now,
				UpdUser =_workContext.CurrentUser.Id,
				UpdTime = DateTime.Now,
				Subtitte = model.Subtitte,
				Contactphone = model.Contactphone,
				Type =model.Type,
                OldPrice =model.OldPrice,
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
                    AddUser = _workContext.CurrentUser.Id,
                    AddTime = DateTime.Now,
                    UpdUser = _workContext.CurrentUser.Id,
                    UpdTime = DateTime.Now,
                    Ad1 = model.Ad1,
                    Ad2 = model.Ad2,
                    Ad3 = model.Ad3,
                };              
                if (_productDetailService.Create(productDetail).Id>0)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true,"数据添加成功"));
                }
			    return PageHelper.toJson(PageHelper.ReturnValue(false,"商品详细添加失败"));
			}            
			return PageHelper.toJson(PageHelper.ReturnValue(false,"数据添加失败"));
		}
        /// <summary>
        /// 商品更新
        /// </summary>
        /// <param name="model">商品model</param>
        /// <returns>提示信息</returns>
		public HttpResponseMessage Put(ProductModel model)
		{
			var entity = _productService.GetProductById(model.Id);
			if(entity == null)
				return PageHelper.toJson(PageHelper.ReturnValue(false,"数据不存在"));
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
            entity.OldPrice = model.OldPrice;
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
                    return PageHelper.toJson(PageHelper.ReturnValue(true,"数据更新成功"));
                return PageHelper.toJson(PageHelper.ReturnValue(false,"商品详细更新失败"));
		    }
			return PageHelper.toJson(PageHelper.ReturnValue(false,"数据更新失败"));
		}
        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <returns>提示信息</returns>
		public HttpResponseMessage Delete(int id)
        {
            var productDetail = _productDetailService.GetProductDetailById(id);
            if (productDetail == null)
                return PageHelper.toJson(PageHelper.ReturnValue(false, "商品详细不存在"));            
                if (_productDetailService.Delete(productDetail))
                {
                    var entity = _productService.GetProductById(id);
                    if (entity == null)
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不存在"));                                       
                        if (_productService.Delete(entity))
                            return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功"));                   
                }
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除失败"));
           
        }
	}
}