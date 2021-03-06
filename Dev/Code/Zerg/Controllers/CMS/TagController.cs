﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;
using CMS.Entity.Model;
using CMS.Service.Tag;
using YooPoon.Core.Site;
using YooPoon.WebFramework.API;
using Zerg.Common;
using Zerg.Models;
using System.ComponentModel;

namespace Zerg.Controllers.CMS
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [Description("Tag管理类")]
    public class TagController : ApiController
    {
        private readonly ITagService _tagService;
        private readonly IWorkContext _workContext;
        /// <summary>
        /// Tag管理初始化
        /// </summary>
        /// <param name="tagService">tagService</param>
        /// <param name="workcontext">workcontext</param>
        public TagController(ITagService tagService,IWorkContext workcontext)
        {
            _tagService = tagService;
            _workContext = workcontext; 
        }
        /// <summary>
        /// Tag首页,根据页面数量设置,返回Tag列表
        /// </summary>
        /// <param name="tag">标签名称</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面记录数</param>
        /// <returns></returns>   
        [HttpGet]
        [Description("Tag管理首页,返回Tag列表")]
        
        public HttpResponseMessage Index(string tag = null,int page=1,int pageSize=10)
        {
            var tagCon = new TagSearchCondition{
                LikeTag = tag,
                Page = page,
                PageCount = pageSize
            };
            var tagList = _tagService.GetTagsByCondition(tagCon).Select(a => new TagModel
            { 
                Id=a.Id,
                Tag=a.Tag
            }).ToList();
            var totalCount = _tagService.GetTagCount(tagCon);
            return PageHelper.toJson(new{List=tagList,Condition=tagCon,TotalCount=totalCount});
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
            Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
            var m = reg.IsMatch(tag.Tag);
            if (!m)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "存在非法字符！"));
            }
                        
                var tagCon = new TagSearchCondition
                {
                    Tag = tag.Tag
                };
                var tagCount = _tagService.GetTagCount(tagCon);
                if (tagCount > 0)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据已存在"));
                }
                else
                {
                    var tagModel = new TagEntity
                    {
                        Tag = tag.Tag,
                        Adduser = _workContext.CurrentUser.Id,
                        Addtime = DateTime.Now,
                        UpdUser = _workContext.CurrentUser.Id,
                        UpdTime = DateTime.Now
                    };
                    if (_tagService.Create(tagModel) != null)
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
        /// 保存修改
        /// </summary>
        /// <param name="model">标签参数</param> 
        [HttpPost]
        public HttpResponseMessage Edit(TagModel model)
        {
            Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
            var m = reg.IsMatch(model.Tag);
            if (!m)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "存在非法字符！"));
            }
            else
            {
            var tag = _tagService.GetTagById(model.Id);
                if (tag.Tag == model.Tag)
                {
                    tag.Tag = model.Tag;
                    tag.UpdTime = DateTime.Now;
                    tag.UpdUser = _workContext.CurrentUser.Id;
                    if (_tagService.Update(tag) != null)
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
                    var tagCon = new TagSearchCondition
                    {
                        Tag = model.Tag
                    };
                    var tagCount = _tagService.GetTagCount(tagCon);
                    if (tagCount > 0)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "数据已存在"));
                    }
                    else
                    {
            tag.Tag = model.Tag;
                        tag.UpdTime = DateTime.Now;
            tag.UpdUser = _workContext.CurrentUser.Id;
                        if (_tagService.Update(tag) != null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功！"));
            }
            else 
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败！"));
            }
        }
                }
                //var tagCon = new TagSearchCondition
                //{
                //    Tag = model.Tag
                //};
                //var tagCount = _tagService.GetTagCount(tagCon);
                //if (tagCount > 0)
                //{
                //    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据已存在"));
                //}
                //var tag = _tagService.GetTagById(model.Id);
                //tag.Tag = model.Tag;
                //tag.UpdTime = DateTime.Now;
                //tag.UpdUser = _workContext.CurrentUser.Id;
                //if (_tagService.Update(tag) != null)
                //{
                //    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功！"));
                //}
                //else
                //{
                //    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败！"));
                //}
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tagId">标签Id</param> 
       [HttpGet]
        public HttpResponseMessage Delete(int tagId)
        {
            var tag = _tagService.GetTagById(tagId);                      
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
