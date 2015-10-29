using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Community.Entity.Model.Product;
using Community.Entity.Model.Service;
using Community.Service.Product;
using Community.Service.Service;
using YooPoon.Core.Site;
using Zerg.Common;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    public class ServiceController : ApiController
    {
        private readonly IProductService _productService;
        private readonly  IServiceService _serviceService;
        private readonly  IWorkContext _workContext;


        public ServiceController(IProductService productService,IServiceService serviceService,IWorkContext workContext)
		{
			_productService = productService;
            _serviceService = serviceService;
            _workContext = workContext;
		}
        #region 获取服务型商品
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
#endregion

        #region 首页服务图标

        public HttpResponseMessage GetService(int id)
        {
            var entity = _serviceService.GetServiceById(id);
            if (entity == null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不存在"));
            }
            var model=new ServiceModel
            {
                Id = entity.Id,
                Link = entity.Link,
                Name = entity.Name,
                Class = entity.Class
            };
            return PageHelper.toJson(model);
        }
        [HttpGet]
        public HttpResponseMessage GetList([FromUri] ServiceSearchCondition condition)
        {
            var model = _serviceService.GetServiceByCondition(condition).Select(c => new ServiceModel
            {
                Id = c.Id,
                Link = c.Link,
                Class = c.Class,
                Name = c.Name
            }).ToList();
            var totalCount = _serviceService.GetServiceCount(condition);
            return PageHelper.toJson(new {List=model,Condition=condition,TotalCount=totalCount});
        }

        public HttpResponseMessage Post(ServiceModel model)
        {
            var entity=new ServiceEntity
            {
                Name = model.Name,
                Class = model.Class,
                AddTime = DateTime.Now,
                AddUser = _workContext.CurrentUser.Id,
                Link = model.Link,
                UpTime = DateTime.Now,
                UpUser = _workContext.CurrentUser.Id
            };
            if (_serviceService.Create(entity).Id > 0)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败"));
        }

        public HttpResponseMessage Put(ServiceModel model)
        {
            var entity = _serviceService.GetServiceById(model.Id);
            if (entity==null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不存在"));
            }
            entity.Name = model.Name;
            entity.Class = model.Class;
            entity.Link= model.Link;
            entity.UpTime=DateTime.Now;
            entity.UpUser = _workContext.CurrentUser.Id;
            if (_serviceService.Update(entity) != null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败"));
        }

        public HttpResponseMessage Delete(int id)
        {
            var entity = _serviceService.GetServiceById(id);
            if (entity == null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不存在"));
            }
            if (_serviceService.Delete(entity))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败"));
        }
        #endregion
    }

}
