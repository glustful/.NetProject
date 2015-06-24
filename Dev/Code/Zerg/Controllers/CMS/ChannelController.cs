using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;
using CMS.Entity.Model;
using CMS.Service.Channel;
using YooPoon.Core.Site;
using Zerg.Common;
using Zerg.Models.CMS;
using CMS.Service.Content;
using System.ComponentModel;

namespace Zerg.Controllers.CMS
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [Description("频道管理类")]
    public class ChannelController : ApiController
    {
        private readonly IChannelService _channelService;
        private readonly IWorkContext _workContent;
        private readonly IContentService _contentService;
        /// <summary>
        /// 频道管理初始化
        /// </summary>
        /// <param name="channelService">channelService</param>
        /// <param name="workContent">workContent</param>
        /// <param name="contentService">contentService</param>
        public ChannelController(IChannelService channelService,IWorkContext workContent,IContentService contentService) {
            _channelService = channelService;
            _workContent = workContent;
            _contentService = contentService;
        }
        /// <summary>
        /// 频道管理首页,根据页面数量设置频道数量,返回频道列表
        /// </summary>
        /// <param name="name">频道名称</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面记录数</param>
        /// <returns>频道列表</returns>
        [HttpGet]
        [Description("频道管理首页,返回频道列表")]
       
        public HttpResponseMessage Index(string name = null,int page = 1, int pageSize = 10)
        {
            var channelCon = new ChannelSearchCondition
            { 
                Name=name,
                Page=page,
                PageCount=pageSize
            };
            var channelList = _channelService.GetChannelsByCondition(channelCon).Select(a=> new ChannelModel
            { 
                Id=a.Id,
                Name=a.Name,
                Status=a.Status,
            }).ToList();
            var totalCount = _channelService.GetChannelCount(channelCon);
            return PageHelper.toJson(new{List=channelList,Condition=channelCon,TotalCount=totalCount});
        }
        /// <summary>
        /// 传入频道参数,创建一个频道,返回创建成功还是失败的提示,成功提示"数据添加成功",失败提示"数据添加失败"
        /// </summary>
        /// <param name="model">频道参数</param>
        /// <returns>频道创建结果状态信息</returns>
        [HttpPost]
        [Description("创建一个频道")]
        public HttpResponseMessage Create(ChannelModel model)
        {
            Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
            var m = reg.IsMatch(model.Name);
            if (!m)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "存在非法字符！"));
            }
            else
            {
                var channelCon = new ChannelSearchCondition
                {
                    Name = model.Name               
                };
                var totalCount = _channelService.GetChannelCount(channelCon);
                if (totalCount > 0)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据已存在！"));
                }
                else
                {
                    var newParent = model.ParentId == 0 ? null : _channelService.GetChannelById(model.ParentId);
                    var channel = new ChannelEntity
                    {
                        Name = model.Name,
                        Status = model.Status,
                        Parent = newParent,
                        Adduser = _workContent.CurrentUser.Id,
                        Addtime = DateTime.Now,
                        UpdUser = _workContent.CurrentUser.Id,
                        UpdTime = DateTime.Now
                    };
                    if (_channelService.Create(channel) != null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！"));
                    }
                    else
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
                    }
                }
            }
        }
        /// <summary>
        /// 传入频道Id,查询频道信息,返回频道详细信息
        /// </summary>
        /// <param name="id">频道id</param>
        /// <returns>频道详细信息</returns>
        [HttpGet]
        [Description("查询频道详细信息")]
        public HttpResponseMessage Detailed(int id)
        {
            var channel = _channelService.GetChannelById(id);
            if (channel == null) {
                return PageHelper.toJson(PageHelper.ReturnValue(false,"该数据不存在！"));
            }
            var channelDetail = new ChannelModel
            {
                Id = channel.Id,
                Name = channel.Name,
                Status = channel.Status,
                ParentId=channel.Parent==null?0:channel.Parent.Id
            };
            return PageHelper.toJson(channelDetail);
        }
        /// <summary>
        /// 传入频道参数,修改频道信息,返回修改结果状态信息,成功提示"数据更新成功",失败提示"数据更新失败"
        /// </summary>
        /// <param name="model">频道参数</param>
        /// <returns>频道更新结果状态信息</returns>  
        [HttpPost]
        [Description("编辑修改频道信息")]
         public HttpResponseMessage Edit(ChannelModel model)
         {
             Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
             var m = reg.IsMatch(model.Name);
            if (!m)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "存在非法字符！"));
            }
            else
            {
                var channel = _channelService.GetChannelById(model.Id);
                if (channel.Name == model.Name)
                {
                    var newParent = _channelService.GetChannelById(model.ParentId);
                    channel.Name = model.Name;
                    channel.Status = model.Status;
                    channel.UpdUser = _workContent.CurrentUser.Id;
                    channel.UpdTime = DateTime.Now;
                    channel.Parent = newParent;
                    if (_channelService.Update(channel) != null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功！"));
                    }
                    else
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败！"));
                    }
                }
                else
                {
                      var channelCon = new ChannelSearchCondition
                      {
                          Name = model.Name
                      };
                      var totalCount = _channelService.GetChannelCount(channelCon);
                      if (totalCount > 0)
                      {
                          return PageHelper.toJson(PageHelper.ReturnValue(false, "数据已存在！"));
                      }
                      else
                      {
                          var newParent = _channelService.GetChannelById(model.ParentId);
                          channel.Name = model.Name;
                          channel.Status = model.Status;
                          channel.UpdUser = _workContent.CurrentUser.Id;
                          channel.UpdTime = DateTime.Now;
                          channel.Parent = newParent;
                          if (_channelService.Update(channel) != null)
                          {
                              return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功！"));
                          }
                          else
                          {
                              return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败！"));
                          }
                      }
                }
            }          
         }
        /// <summary>
        /// 传入频道Id,判断频道是否为空,无关联信息,返回删除结果状态信息,成功提示"数据删除成功",失败提示"数据删除失败"
        /// </summary>
        /// <param name="id">频道id</param>
        /// <returns>频道删除结果状态信息</returns>
        [Description("传入Id,删除频道")]
        [HttpGet]
         public HttpResponseMessage Delete(int id)
         {
            var channel = _channelService.GetChannelById(id);
            if (channel.Contents != null && channel.Contents.Any())
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "有内容关联不能删除"));
            }
            if (_channelService.Delete(channel))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功！"));
                }
                else
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败！"));
                }
         }
        /// <summary>
        /// 传入频道名称,查询频道内容,返回频道中五条图片内容
        /// </summary>
        /// <param name="channelName">频道名称</param>
        /// <returns>频道内容列表</returns>
        [HttpGet]
        [Description("查询频道下前五条内容")]
        public HttpResponseMessage GetTitleImg(string channelName)
        {
            //var channel = _channelService.GetChannelById(channelId);

            //var contents = channel.Contents.AsQueryable().OrderByDescending(c => c.Addtime).Take(5).Select(c => new
            //{
            //    c.Title,
            //    c.TitleImg,
            //    c.AdSubTitle
            //}).ToList();

            var content = _contentService.GetContentsByCondition(new ContentSearchCondition
            {
                ChannelName = channelName
            }).OrderByDescending(c => c.Addtime).Take(5).Select(c => new
            {
                c.Title,
                c.AdSubTitle,
                c.TitleImg,
                c.Id
            });

            return PageHelper.toJson(content);
        }

        /// <summary>
        /// 传入频道名称,查询频道下前6条活动内容,返回活动内容列表
        /// </summary>
        /// <param name="channelName">频道名称</param>
        /// <returns>活动列表</returns>
    
        [Description("查询频道下前6条活动信息")]
       public HttpResponseMessage GetActiveTitleImg(string channelName)
       {
        
           var Actcontent = _contentService.GetContentsByCondition(new ContentSearchCondition
           {
               ChannelName = channelName
           }).OrderByDescending(c => c.Addtime).Take(6).Select(c => new
           {
               c.Title,
               c.AdSubTitle,
               c.TitleImg,
               c.Id
           });

           return PageHelper.toJson(Actcontent);
       }
    }
}
