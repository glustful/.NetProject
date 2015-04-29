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

namespace Zerg.Controllers.CMS
{
    public class ContentController : ApiController
    {
        private readonly IContentService _contentService;
        private readonly IUserService _userService;
        private readonly IChannelService _channelService;


        public ContentController(IContentService contentService,IUserService userService,IChannelService channelService)
        {
            _contentService = contentService;
            _channelService = channelService;
            _userService = userService;
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
        public List<ContentModel> Index(string title=null,int page = 1, int pageSize = 10)
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
            return contentList;
        }
        /// <summary>
        /// 内容详细信息
        /// </summary>
        /// <param name="id">内容id</param>
        /// <returns></returns>
        [HttpGet]
        public ContentDetailModel Detailed(int id)
        {
            var content = _contentService.GetContentById(id);
            var contentDetail = new ContentDetailModel
            {
                Id=content.Id,
                Title=content.Title,
                Content=content.Content,
                ChannelName=content.Channel.Name,
                Status=content.Status
            };
            return contentDetail;
        }
        /// <summary>
        /// 内容修改
        /// </summary>
        /// <param name="model">内容参数</param>
        /// <returns></returns>
        public ResultModel DoEdit(ContentDetailModel model)
        { 
            ResultModel result = new ResultModel();
            var content = _contentService.GetContentById(model.Id);
            content.Title = model.Title;
            content.Content = model.Content;
            content.UpdTime = DateTime.Now;
            if (_contentService.Update(content)) {
                result.Status = true;
                result.Msg = "修改成功";
            }
            else
            {
                result.Status = false;
                result.Msg = "修改失败";
            }
            return result;
        }
        /// <summary>
        /// 新建内容
        /// </summary>
        /// <param name="model">内容参数</param>
        /// <returns></returns>
        public ResultModel DoCreate(ContentDetailModel model)
        {
            var channel = _channelService.GetChannelById(model.ChannelId);
            if (channel == null)
            {
                return new ResultModel();
            }
            var content = new ContentEntity
            {
                Title = model.Title,
                Content = model.Content,
                Status=model.Status, 
                Channel=channel,
                Addtime=DateTime.Now
            };
            ResultModel result = new ResultModel();
            if (_contentService.Create(content).Id > 0)
            {
                result.Status = true;
                result.Msg = "保存成功";
            }
            else {
                result.Status = false;
                result.Msg = "保存失败";
            }
            return result;
        }
        /// <summary>
        /// 删除内容
        /// </summary>
        /// <param name="id">内容id</param>
        /// <returns></returns>
        [HttpGet]
        public ResultModel Delete(int id) {
            var content = _contentService.GetContentById(id);
            ResultModel result = new ResultModel();
            if (_contentService.Delete(content))
            {
                result.Status = true;
                result.Msg = "删除成功";
            }
            else {
                result.Status = false;
                result.Msg = "删除失败";
            }
            return result;
        }
    }
}
