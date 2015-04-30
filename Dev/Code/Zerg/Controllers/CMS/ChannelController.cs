using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Service.Channel;
using CMS.Entity.Model;
using Zerg.Models.CMS;
using Zerg.Models;
using Zerg.Common;

namespace Zerg.Controllers.CMS
{
    public class ChannelController : ApiController
    {
        private readonly IChannelService _channelService;
        public ChannelController(IChannelService channelService) {
            _channelService = channelService;
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
                Status=a.Status                
            }).ToList();
            return PageHelper.toJson(channelList);
        }
        /// <summary>
        /// 新建频道
        /// </summary>
        /// <param name="model">频道参数</param>
        /// <returns></returns>
        public HttpResponseMessage Docreate(ChannelModel model)
        {
           var newParent = _channelService.GetChannelById(model.Parent.Id);
            var channel = new ChannelEntity
            {
                Name=model.Name,
                Status=model.Status,
                Parent=newParent,
                Addtime=DateTime.Now
            };
            if (_channelService.Create(channel)!=null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！"));
            }
            else
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
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
                Parent = new ChannelModel
                {
                    Id=channel.Parent.Id,
                    Name=channel.Parent.Name,
                    Status=channel.Parent.Status
                }
            };
            return PageHelper.toJson(channelDetail);
        }
        /// <summary>
        /// 保存修改信息
        /// </summary>
        /// <param name="model">频道参数</param>
        /// <returns></returns>        
         public HttpResponseMessage DoEdit(ChannelModel model)
         {
             var channel = _channelService.GetChannelById(model.Id);
             var newParent = _channelService.GetChannelById(model.Parent.Id);
             channel.Name = model.Name;
             channel.UpdTime = DateTime.Now;
             channel.Parent = newParent;
             if (_channelService.Update(channel)!=null) {
                 return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功！"));
             }
             else
             {
                 return PageHelper.toJson(PageHelper.ReturnValue(false,"数据更新失败！"));
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
             if (_channelService.Delete(channel))
             {
                 return PageHelper.toJson(PageHelper.ReturnValue(true,"数据删除成功！"));
             }
             else{
                 return PageHelper.toJson(PageHelper.ReturnValue(false,"数据删除失败！"));
             }
         }
    }
}
