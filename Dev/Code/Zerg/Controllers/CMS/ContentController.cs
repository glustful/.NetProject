using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;
using CMS.Entity.Model;
using CMS.Service.Channel;
using CMS.Service.Content;
using YooPoon.Core.Site;
using Zerg.Common;
using Zerg.Models;
using Zerg.Models.CMS;

namespace Zerg.Controllers.CMS
{
     [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    public class ContentController : ApiController
    {
        private readonly IContentService _contentService;
        private readonly IChannelService _channelService;
        private readonly IWorkContext _workContent;

        
        public ContentController(IContentService contentService,IChannelService channelService,IWorkContext workContent)
        {
            _contentService = contentService;
            _channelService = channelService;
            _workContent = workContent;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="title">内容标题</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面记录数</param>
        /// <returns></returns>
        [HttpGet]
       
        public HttpResponseMessage Index(string title=null,int page = 1,int pageSize = 10)
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
                Channel=a.Channel.Name,
                TitleImg = a.TitleImg,
                ChannelId = a.Channel.Id,
                AddUser = a.Adduser
            }).ToList();
            var totalCount = _contentService.GetContentCount(contentCon);
            return PageHelper.toJson(new { List = contentList, Condition = contentCon, TotalCount = totalCount });
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
                TitleImg = content.TitleImg,
                Content=content.Content,
                ChannelName=content.Channel.Name,
                ChannelId = content.Channel.Id,
                Status=content.Status,
                AdSubTitle=content.AdSubTitle
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
            Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
            var m = reg.IsMatch(model.Title);
            if (!m)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "标题存在非法字符！"));
            }
            else
            {
                var content = _contentService.GetContentById(model.Id);
                var newChannel = _channelService.GetChannelById(model.ChannelId);
                content.Title = model.Title;
                content.Content = model.Content;
                content.Status = model.Status;
                content.AdSubTitle = model.AdSubTitle;
                content.UpdUser = _workContent.CurrentUser.Id;
                content.UpdTime = DateTime.Now;
                content.Channel = newChannel;
                if (_contentService.Update(content) != null)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功！"));
                }
                else
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败！"));
                }
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
            Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
            var m = reg.IsMatch(model.Title);
            if (!m)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "标题存在非法字符！"));
            }
            else
            {                              
                    var newChannel = _channelService.GetChannelById(model.ChannelId);
                    if (newChannel == null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "请选择频道！"));
                    }
                    var content = new ContentEntity
                    {
                        Title = model.Title,
                        TitleImg = model.TitleImg,
                        Content = model.Content,
                        Status = model.Status,
                        Channel = newChannel,
                        Adduser = _workContent.CurrentUser.Id,
                        Addtime = DateTime.Now,
                        UpdUser = _workContent.CurrentUser.Id,
                        UpdTime = DateTime.Now,
                        AdSubTitle=model.AdSubTitle
                    };
                    if (_contentService.Create(content) != null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！"));
                    }
                    else
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
                    }
            }
        }
        /// <summary>
        /// 删除内容
        /// </summary>
        /// <param name="id">内容id</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Delete(int id) {
            var content = _contentService.GetContentById(id);
            if (content.Resources != null && content.Resources.Any())
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "有资源关联无法删除！"));
            }
            else
            {
                if (_contentService.Delete(content))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功！"));
                }
                else
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败！"));
                }
            }
        }
    }
}
