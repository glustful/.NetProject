using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Service.Tag;
using CMS.Service.Content;
using CMS.Service.Channel;
using CMS.Entity.Model;
using Zerg.Models;
using System.Web.Http.Results;
using Zerg.Common;

namespace Zerg.Controllers.CMS
{
    public class TagController : ApiController
    {
        private readonly ITagService _tagService;
        private readonly IContentService _contentService;
        public TagController(ITagService tagService, IContentService contentService)
        {
            _tagService = tagService;
            _contentService = contentService;
        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>   
        [System.Web.Http.HttpGet] 
        public List<TagModel> Index(string tag = null,int page=1,int pageSize=10)
        {
            var tagCon = new TagSearchCondition{
                Tag = tag,
                Page = page,
                PageCount = pageSize
            };
            var totalCount = _tagService.GetTagsByCondition(tagCon).Select(a => new TagModel { 
                Id=a.Id,
                Tag=a.Tag
            }).ToList();
            return totalCount;
        }
        /// <summary>
        /// 详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet] 
        public TagDetailModel Detailed(int id)
        {
            var tag = _tagService.GetTagById(id);
            if (tag == null)
            {
                return new TagDetailModel();
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
            return newModel;
        }
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="tag"></param>
        public void DoCreate(TagModel tag)
        {
            var tagModel = new TagEntity
            {
                Id=tag.Id,
                Tag=tag.Tag,                
            };          
            _tagService.Create(tagModel);     
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       [System.Web.Http.HttpGet] 
        public TagModel Edit(int id)
        {
            var tag = _tagService.GetTagById(id);         
            return new TagModel
            {
                Id=tag.Id,
                Tag=tag.Tag
            };
        }
        /// <summary>
        /// 保存编辑
        /// </summary>
        /// <param name="model"></param> 
        
        public ResultModel DoEdit(TagModel model)
        {
            var tag = _tagService.GetTagById(model.Id);
            tag.Tag = model.Tag;
            tag.UpdTime=DateTime.Now;
            ResultModel result = new ResultModel();
            if (_tagService.Update(tag))
            {
                result.Status =true;
                result.Msg = "修改成功";
            }
            else {
                result.Status = false;
                result.Msg = "修改失败";
            }
            return result;  
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param> 
       [System.Web.Http.HttpGet] 
        public ResultModel Delete(int id)
        {
            var tag = _tagService.GetTagById(id);                      
            ResultModel result = new ResultModel();
            if (_tagService.Delete(tag))
            {               
                result.Status =true;
                result.Msg = "删除成功";                
            }
            else
            {
                result.Status =false;
                result.Msg = "删除失败";
            }
            return result;                   
        }
    }
}
