using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using CMS.Service.Content;
using CMS.Service.Channel;
using YooPoon.Core.Site;
using YooPoon.WebFramework.User.Services;
using CMS.Entity.Model;
using Zerg.Models;
using Zerg.Models.CMS;
using Zerg.Common;
using YooPoon.Core;

namespace Zerg.Controllers.CMS
{
    public class ContentController : ApiController
    {
        private readonly IContentService _contentService;
        private readonly IUserService _userService;
        private readonly IChannelService _channelService;
        private IWorkContext _workContent;


        public ContentController(IContentService contentService,IUserService userService,IChannelService channelService,IWorkContext workContent)
        {
            _contentService = contentService;
            _channelService = channelService;
            _userService = userService;
            _workContent = workContent;
        }


        public string Get()
        {
            _contentService.GetContentById(1);
            _userService.GetUserByName("a");
            return "0";
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="title">内容标题</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面记录数</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Index(string title=null,int page = 1, int pageSize = 10)
        {
            var contentCon = new ContentSearchCondition
            {
                Title=title,
                Page = page,
                PageCount = pageSize
            };
            var contentList = _contentService.GetContentsByCondition(contentCon).Select(a => new ContentModel
            { 
                Id=a.Id,
                Title=a.Title,
                Status=a.Status,
                Channel=a.Channel.Name
            }).ToList();
            return PageHelper.toJson(contentList);
        }
        /// <summary>
        /// 内容详细信息
        /// </summary>
        /// <param name="id">内容id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Detailed(int id)
        {
            var content = _contentService.GetContentById(id);
            if (content == null) {
                return PageHelper.toJson(PageHelper.ReturnValue(false,"该数据不存在！"));
            }
            var contentDetail = new ContentDetailModel
            {
                Id=content.Id,
                Title=content.Title,
                Content=content.Content,
                ChannelName=content.Channel.Name,
                Status=content.Status
            };
            return PageHelper.toJson(contentDetail);
        }
        /// <summary>
        /// 内容修改
        /// </summary>
        /// <param name="model">内容参数</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Edit(ContentDetailModel model)
        { 
            var content = _contentService.GetContentById(model.Id);
            var newChannel = _channelService.GetChannelById(model.ChannelId);
            content.Title = model.Title;
            content.Content = model.Content;
            content.Status = model.Status;
            content.UpdUser = _workContent.CurrentUser.Id;
            content.UpdTime = DateTime.Now;
            content.Channel = newChannel;
            if (_contentService.Update(content)!=null) {
                return PageHelper.toJson(PageHelper.ReturnValue(true,"数据更新成功！"));
            }
            else
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false,"数据更新失败！"));
            }
        }
        /// <summary>
        /// 新建内容
        /// </summary>
        /// <param name="model">内容参数</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Create(ContentDetailModel model)
        {
            var newChannel = _channelService.GetChannelById(model.ChannelId);
            if (newChannel == null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false,"数据添加失败！"));
            }
            var content = new ContentEntity
            {
                Title = model.Title,
                Content = model.Content,
                Status=model.Status, 
                Channel=newChannel,
                Adduser=_workContent.CurrentUser.Id,
                Addtime=DateTime.Now,
                UpdUser=_workContent.CurrentUser.Id,
                UpdTime=DateTime.Now
            };
            if (_contentService.Create(content)!=null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true,"数据添加成功！"));
            }
            else {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
            }
        }
        /// <summary>
        /// 删除内容
        /// </summary>
        /// <param name="id">内容id</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Delete(int id) {
            var content = _contentService.GetContentById(id);
            if (_contentService.Delete(content))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true,"数据删除成功"));
            }
            else {
                return PageHelper.toJson(PageHelper.ReturnValue(false,"数据删除失败"));
            }          
        }
    }
}
