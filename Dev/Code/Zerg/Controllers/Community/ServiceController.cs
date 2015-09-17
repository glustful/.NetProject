using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Community.Entity.Model.Product;
using Community.Service.Product;
using Zerg.Common;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    public class ServiceController : ApiController
    {
        private readonly IProductService _productService;       
       

        public ServiceController(IProductService productService)
		{
			_productService = productService;		             
		}
        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <returns>商品实体</returns>
        public HttpResponseMessage Get(int id)
        {
            var entity = _productService.GetProductById(id);
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
                           // AddUser = c.AddUser
                        }).ToList();
            }
            var model = new ProductModel
            {               
                BussnessId = entity.BussnessId,
                BussnessName = entity.BussnessName,
                Price = entity.Price,
                Name = entity.Name,
                Status = entity.Status,
                MainImg = entity.MainImg,
                IsRecommend = entity.IsRecommend,
                Sort = entity.Sort,
                Stock = entity.Stock,
                Subtitte = entity.Subtitte,
                Contactphone = entity.Contactphone,
                SericeInstruction = entity.Detail.SericeInstruction,
                Type = entity.Type             
            };
            var product = new ProductComment
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
            var model = _productService.GetProductsByCondition(condition).Select(c => new ProductModel
            {
                Id = c.Id,               
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
                Type = c.Type              
            }).ToList();
            var totalCount = _productService.GetProductCount(condition);
            return PageHelper.toJson(new{List=model,Condition=condition,TotalCount=totalCount});
        }
    }

}
