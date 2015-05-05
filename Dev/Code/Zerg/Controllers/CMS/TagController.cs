using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using CMS.Entity.Model;
using CMS.Service.Tag;
using YooPoon.Core.Site;
using Zerg.Common;
using Zerg.Models;

namespace Zerg.Controllers.CMS
{
    public class TagController : ApiController
    {
        private readonly ITagService _tagService;
        private readonly IWorkContext _workContext;
        public TagController(ITagService tagService,IWorkContext workcontext)
        {
            _tagService = tagService;
            _workContext = workcontext; 
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="tag">标签名称</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面记录数</param>
        /// <returns></returns>   
        [HttpGet] 
        public HttpResponseMessage Index(string tag = null,int page=1,int pageSize=10)
        {
            var tagCon = new TagSearchCondition{
                Tag = tag,
                Page = page,
                PageCount = pageSize
            };
            var tagList = _tagService.GetTagsByCondition(tagCon).Select(a => new TagModel { 
                Id=a.Id,
                Tag=a.Tag
            }).ToList();
            return PageHelper.toJson(tagList);
        }
        /// <summary>
        /// tag详细信息
        /// </summary>
        /// <param name="id">标签Id</param>
        /// <returns></returns>
        [HttpGet] 
        public HttpResponseMessage Detailed(int id)
        {
            var tag = _tagService.GetTagById(id);
            if (tag == null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false,"该数据不存在！"));
            }      
            var content = tag.Content;
            List<ContentModel> list;
            if (content == null)
            {
                list = new List<ContentModel>();
            }
            else
            {
                list =    ( from c in content
                           select new ContentModel
                           {
                               Id=c.Id,
                               Title = c.Title,
                               AddUser = c.Adduser,
                               Status = c.Status,
                               Channel = c.Channel.Name
                           }).ToList();
            }
            var newModel = new TagDetailModel {
                Tag = new TagModel { Id = tag.Id,Tag=tag.Tag },
                Contents = list
            };
            return PageHelper.toJson(newModel);
        }
        /// <summary>
        /// 新建Tag
        /// </summary>
        /// <param name="tag">标签参数</param>
        [HttpPost]
        public HttpResponseMessage Create(TagModel tag)
        {
            var tagModel = new TagEntity
            {
                Tag=tag.Tag,
                Adduser=_workContext.CurrentUser.Id,
                Addtime=DateTime.Now,
                UpdUser = _workContext.CurrentUser.Id,
                UpdTime=DateTime.Now
            };
            if (_tagService.Create(tagModel)!=null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true,"数据添加成功！"));
            }
            else {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
            }
        }
        /// <summary>
        /// 保存修改
        /// </summary>
        /// <param name="model">标签参数</param> 
        [HttpPost]
        public HttpResponseMessage Edit(TagModel model)
        {
            var tag = _tagService.GetTagById(model.Id);
            tag.Tag = model.Tag;
            tag.UpdTime=DateTime.Now;
            tag.UpdUser = _workContext.CurrentUser.Id;
            if (_tagService.Update(tag)!=null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功！"));
            }
            else 
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败！"));
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">标签Id</param> 
       [HttpPost]
        public HttpResponseMessage Delete(int id)
        {
            var tag = _tagService.GetTagById(id);                      
            if (_tagService.Delete(tag))
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
