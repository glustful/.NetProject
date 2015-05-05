using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Service.Advertisement;
using CMS.Service.Content;
using CMS.Entity.Model;
using Zerg.Models.CMS;
using Zerg.Common;
using YooPoon.Core.Site;

namespace Zerg.Controllers.CMS
{
    public class AdvertisementController : ApiController
    {
        private IAdvertisementService _advertisementService;
        private IContentService _contentService;
        private IWorkContext _workContent;
        public AdvertisementController(IAdvertisementService advertisementService,IContentService contentService,IWorkContext workContent)
        {
            _advertisementService = advertisementService;
            _contentService = contentService;
            _workContent = workContent;
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="title">广告标题</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面记录</param>
        /// <returns>广告列表</returns>
        [HttpGet]
        public HttpResponseMessage Index(string title=null, int page=1, int pageSize=10) 
        {
            var advertisementCon = new AdvertisementSearchCondition
            {
                Title = title,
                Page = page,
                PageCount = pageSize
            };
            var advertisementList = _advertisementService.GetAdvertisementsByCondition(advertisementCon).Select(a => new AdvertisementModel { 
                 Id=a.Id,
                 Title=a.Title,
                 Continue=a.Continue.ToString()
            }).ToList();
            return PageHelper.toJson(advertisementList);
        }
        /// <summary>
        /// 详细信息
        /// </summary>
        /// <param name="id">广告ID</param>
        /// <returns>广告详细</returns>
        [HttpGet]
        public HttpResponseMessage Detailed(int id)
        {
            var advertisement = _advertisementService.GetAdvertisementById(id);
            if (advertisement == null) 
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "该数据不存在"));
            }
            else {
                var advertisementDetail = new AdvertisementModel
                {
                      Id = advertisement.Id,
                      Title = advertisement.Title,
                      Detail = advertisement.Detail,
                      Continue=advertisement.Continue.ToString(),
                      ContentId=advertisement.Content.Id,
                      ContentTitle=advertisement.Content.Title
                 };
                 return PageHelper.toJson(advertisementDetail);
            }
        }
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="model">广告参数</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Create(AdvertisementModel model)
        {
            var newContent=_contentService.GetContentById(model.ContentId);
            var advertisement = new AdvertisementEntity
            {
                Title = model.Title,
                Detail = model.Detail,
                Continue = Convert.ToDateTime(model.Continue),
                Content=newContent,
                Adduser=_workContent.CurrentUser.Id,
                Addtime=DateTime.Now,
                UpdUser=_workContent.CurrentUser.Id,
                UpdTime=DateTime.Now
            };
            if (_advertisementService.Create(advertisement) != null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功"));
            }
            else
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false,"数据添加失败"));
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">数据</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Edit(AdvertisementModel model)
        {
            var newContent = _contentService.GetContentById(model.ContentId);
             var advertisement=_advertisementService.GetAdvertisementById(model.Id);
             advertisement.Title = model.Title;
             advertisement.Detail = model.Detail;
             advertisement.Continue = Convert.ToDateTime(model.Continue);
             advertisement.UpdUser = _workContent.CurrentUser.Id;
             advertisement.UpdTime = DateTime.Now;
             advertisement.Content = newContent;
             if (_advertisementService.Update(advertisement) != null)
             {
                 return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功"));
             }
  
            return PageHelper.toJson(PageHelper.ReturnValue(false,"数据更新失败"));
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Delete(int id)
        {
           var advertisement=_advertisementService.GetAdvertisementById(id);
           if (_advertisementService.Delete(advertisement))
           {
               return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功"));
           }
           else 
           {
               return PageHelper.toJson(PageHelper.ReturnValue(false,"数据删除失败"));
           }
        }
    }
}
