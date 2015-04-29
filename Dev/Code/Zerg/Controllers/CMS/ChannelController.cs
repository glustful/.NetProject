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
        public List<ChannelModel> Index(string name = null,int page = 1, int pageSize = 10)
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
            return channelList;
        }
        /// <summary>
        /// 新建频道
        /// </summary>
        /// <param name="model">频道参数</param>
        /// <returns></returns>
        public ResultModel Docreate(ChannelModel model)
        {
           var newParent = _channelService.GetChannelById(model.Parent.Id);
            var channel = new ChannelEntity
            {
                Name=model.Name,
                Status=model.Status,
                Parent=newParent,
                Addtime=DateTime.Now
            };
            ResultModel result = new ResultModel();
            if (_channelService.Create(channel).Id > 0)
            {
                result.Status = true;
                result.Msg = "创建成功";
            }
            else
            {
                result.Status = false;
                result.Msg = "创建失败";
            }
            return result;
        }
        /// <summary>
        /// 频道详细信息
        /// </summary>
        /// <param name="id">频道id</param>
        /// <returns></returns>
         [HttpGet]
        public ChannelModel Detailed(int id)
        {
            var channel = _channelService.GetChannelById(id);
            if (channel == null) {
                return new ChannelModel();
            }
            var channelDetail = new ChannelModel
            {
                Id = channel.Id,
                Name = channel.Name,
                Status = channel.Status,
                //parentid=channel.Parent.Id,
                Parent = new ChannelModel
                {
                    Id=channel.Parent.Id,
                    Name=channel.Parent.Name,
                    Status=channel.Parent.Status
                }
            };
            return channelDetail;
        }
        /// <summary>
        /// 保存修改信息
        /// </summary>
        /// <param name="model">频道参数</param>
        /// <returns></returns>        
         public ResultModel DoEdit(ChannelModel model)
         {
             var channel = _channelService.GetChannelById(model.Id);
             var newParent = _channelService.GetChannelById(model.Parent.Id);
             channel.Name = model.Name;
             channel.UpdTime = DateTime.Now;
             channel.Parent = newParent;
             ResultModel result = new ResultModel();
             if (_channelService.Update(channel)) {
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
        /// 频道删除
        /// </summary>
        /// <param name="id">频道id</param>
        /// <returns></returns>
        [HttpGet]
         public ResultModel Delete(int id)
         {
             var channel = _channelService.GetChannelById(id);
             ResultModel result = new ResultModel();
             if (_channelService.Delete(channel))
             {
                 result.Status=true;
                 result.Msg="删除成功";
             }
             else{
                 result.Status = false;
                 result.Msg = "删除失败";
             }
             return result;
         }
    }
}
