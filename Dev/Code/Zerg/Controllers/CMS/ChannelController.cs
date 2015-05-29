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

namespace Zerg.Controllers.CMS
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    public class ChannelController : ApiController
    {
        private readonly IChannelService _channelService;
        private readonly IWorkContext _workContent;

        public ChannelController(IChannelService channelService,IWorkContext workContent) {
            _channelService = channelService;
            _workContent = workContent;
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="name">频道名称</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页记录数</param>
        /// <returns></returns>
        [HttpGet]
       
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
        /// 新建频道
        /// </summary>
        /// <param name="model">频道参数</param>
        /// <returns></returns>
        [HttpPost]
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
        /// 频道详细信息
        /// </summary>
        /// <param name="id">频道id</param>
        /// <returns></returns>
         [HttpGet]
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
        /// 保存修改信息
        /// </summary>
        /// <param name="model">频道参数</param>
        /// <returns></returns>  
        [HttpPost]
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
        /// 频道删除
        /// </summary>
        /// <param name="id">频道id</param>
        /// <returns></returns>
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
    }
}
