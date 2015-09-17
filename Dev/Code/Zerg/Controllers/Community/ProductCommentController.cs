using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Community.Entity.Model.ProductComment;
using Community.Service.Product;
using Community.Service.ProductComment;
using Zerg.Common;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
	public class ProductCommentController : ApiController
	{
		private readonly IProductCommentService _productCommentService;
        private readonly IProductService _productService;

		public ProductCommentController(IProductCommentService productCommentService,IProductService productService)
		{
		    _productCommentService = productCommentService;
            _productService = productService;
		}
        /// <summary>
        /// 根据评论ID获取该评论
        /// </summary>
        /// <param name="id">评论ID</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get(int id)
		{
			var entity =_productCommentService.GetProductCommentById(id);
			var model = new ProductCommentModel
			{
				Id = entity.Id,	
                ProductId = entity.Product.Id,	
                AddUser = entity.AddUser,		
                AddTime = entity.AddTime,		
                Content = entity.Content,		
                Stars = entity.Stars,			
            };
            return PageHelper.toJson(model);
		}

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get([FromUri]ProductCommentSearchCondition condition)
		{
			var model = _productCommentService.GetProductCommentsByCondition(condition).Select(c=>new ProductCommentModel
			{
				Id = c.Id,
                ProductId = c.Product.Id,
                ProductName = c.Product.Name,
				AddUser = c.AddUser,
				AddTime = c.AddTime,
				Content = c.Content,
				Stars = c.Stars,
			}).ToList();
            var totalCount = _productCommentService.GetProductCommentCount(condition);
            

            return PageHelper.toJson(new { Model = model, Condition=condition,TotalCount = totalCount });
		}


        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="model">评论实体</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post(ProductCommentModel model)
        {
            
            
			var entity = new ProductCommentEntity
			{
				Product =_productService.GetProductById(model.ProductId),
                AddUser = model.AddUser,
				AddTime =DateTime.Now,
				Content = model.Content,
				Stars = model.Stars
			};
			if(_productCommentService.Create(entity).Id > 0)
			{
                return PageHelper.toJson(PageHelper.ReturnValue(true, "添加成功！"));
			}
            return PageHelper.toJson(PageHelper.ReturnValue(false, "添加失败！"));
		}


        /// <summary>
        /// 修改评论
        /// </summary>
        /// <param name="model">评论实体</param>
        /// <returns>Bool</returns>
        public HttpResponseMessage Put(ProductCommentModel model)
		{
			var entity = _productCommentService.GetProductCommentById(model.Id);
			if(entity == null)
                return PageHelper.toJson(PageHelper.ReturnValue(false, "没有该评论！"));
			entity.Product = _productService.GetProductById(model.Id);
			entity.AddUser = model.AddUser;
			entity.AddTime = model.AddTime;
			entity.Content = model.Content;
			entity.Stars = model.Stars;
			if(_productCommentService.Update(entity) != null)
                return PageHelper.toJson(PageHelper.ReturnValue(true, "修改成功！"));
            return PageHelper.toJson(PageHelper.ReturnValue(false, "修改失败！"));
		}


        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="id">评论ID</param>
        /// <returns></returns>
        
        public HttpResponseMessage Delete(int id)
		{
			var entity = _productCommentService.GetProductCommentById(id);
			if(entity == null)
                return PageHelper.toJson(PageHelper.ReturnValue(false, "没有该评论！"));
			if(_productCommentService.Delete(entity))
                return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功！"));
            return PageHelper.toJson(PageHelper.ReturnValue(true, "删除失败！"));
		}
	}
}