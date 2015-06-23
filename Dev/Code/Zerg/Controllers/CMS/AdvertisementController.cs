using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using CMS.Entity.Model;
using CMS.Service.Advertisement;
using CMS.Service.Content;
using YooPoon.Core.Site;
using Zerg.Common;
using Zerg.Models.CMS;
using System.ComponentModel;

namespace Zerg.Controllers.CMS
{
    [Description("广告控制类")]
    public class AdvertisementController : ApiController
    {
        private readonly IAdvertisementService _advertisementService;
        private readonly IContentService _contentService;
        private readonly IWorkContext _workContent;
        /// <summary>
        /// 广告控制初始化
        /// </summary>
        /// <param name="advertisementService">advertisementService</param>
        /// <param name="contentService">contentService</param>
        /// <param name="workContent">workContent</param>
        public AdvertisementController(IAdvertisementService advertisementService,IContentService contentService,IWorkContext workContent)
        {
            _advertisementService = advertisementService;
            _contentService = contentService;
            _workContent = workContent;
        }
        /// <summary>
        /// 广告首页,根据页面数量设置,返回广告列表
        /// </summary>
        /// <param name="title">广告标题</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面记录</param>
        /// <returns>广告列表</returns>
        [HttpGet]
        [Description("广告首页,返回列表")]
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
                 Continue=a.Continue.ToString(CultureInfo.InvariantCulture)
            }).ToList();
            return PageHelper.toJson(advertisementList);
        }
        /// <summary>
        /// 根据广告Id查询广告的详细信息
        /// </summary>
        /// <param name="id">广告ID</param>
        /// <returns>广告详细信息</returns>
        [HttpGet]
        [Description("查询广告详细信息")]
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
                      Continue=advertisement.Continue.ToString(CultureInfo.InvariantCulture),
                      ContentId=advertisement.Content.Id,
                      ContentTitle=advertisement.Content.Title
                 };
                 return PageHelper.toJson(advertisementDetail);
            }
        }
        /// <summary>
        /// 传入广告参数,创建广告,返回创建成功或是失败状态信息,成功返回"数据添加成功",失败返回"数据添加失败"
        /// </summary>
        /// <param name="model">广告参数</param>
        /// <returns>创建广告结果状态信息</returns>
        [HttpPost]
        [Description("输入参数,创建广告")]
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
        /// 传入广告参数,更新公告信息,返回更新广告成功还是失败结果信息,失败提示"数据更新失败",成功提示＂数据更新成功＂
        /// </summary>
        /// <param name="model">广告参数</param>
        /// <returns>广告更新结果状态信息</returns>
        [HttpPost]
        [Description("编辑更新公告信息")]
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
        /// 传入广告Id,删除该条广告,返回删除结果,成功提示"数据删除成功",失败提示"数据删除失败"
        /// </summary>
        /// <param name="id">广告Id</param>
        /// <returns>广告删除结果状态信息</returns>
        [HttpPost]
        [Description("删除广告")]
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
