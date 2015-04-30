using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRM.Service.MessageConfig ;
using CRM.Entity.Model;
using Zerg.Common;

namespace Zerg.Controllers.CRM
{
    /// <summary>
    /// 短信配置
    /// </summary>
    public class MessageConfigController : ApiController
    {
        public readonly IMessageConfigService _MessageConfigService;
        public MessageConfigController (IMessageConfigService messageConfig)
        {
            _MessageConfigService = messageConfig;
        }
        #region 短信配置 黄秀宇 2015.04.29

        /// <summary>
        /// 查询短信配置
        /// </summary>
        /// <param name="Page">页码</param>
        /// <param name="PageCount">每页大小</param>
        /// <param name="isDescending">是否降序</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public List<MessageConfigEntity> SearchMessageConfig(int Page = 1, int PageCount = 1, bool isDescending = true)
        {

            var mConfig = new MessageConfigSearchCondition()
            {
                Page = Page,
                PageCount = PageCount,
                isDescending = isDescending

            };
           return  _MessageConfigService.GetMessageConfigsByCondition(mConfig).ToList();
           
        }
        /// <summary>
        /// 添加短信配置模板
        /// </summary>
        /// <param name="Name">模板名称</param>
        /// <param name="Template">配置模板</param>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddMessageConfig(string Name, string Template)
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Template))
            {
                var MessageConfigInsert = new MessageConfigEntity()
                {
                    Name = Name,
                    Template = Template,
                     Uptime = DateTime.Now,
                    Addtime = DateTime.Now
                };
                try
                {
                    if ( _MessageConfigService.Create(MessageConfigInsert)!= null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功！"));
                    }
                }
                catch
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败！"));
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
          
          
        }
        /// <summary>
        /// 删除短信配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DeleteMessageConfig(string id)
        {
            if (!string.IsNullOrEmpty(id) && PageHelper.ValidateNumber(id))
            {
                if (_MessageConfigService.Delete(_MessageConfigService.GetMessageConfigById(Convert.ToInt32(id))))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据成功删除！"));
                }
                else
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败！"));
                }
            }

            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
          
        }
        /// <summary>
        /// 更新短信配置模板
        /// </summary>
        /// <param name="Name">模板名称</param>
        /// <param name="Template">配置模板</param>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateMessageConfig(string id, string Name, string Template)
        {
            if (!string.IsNullOrEmpty(id) && PageHelper.ValidateNumber(id) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Template))
            {
                
                var mConfigUpdate = _MessageConfigService.GetMessageConfigById(Convert.ToInt32(id));
                mConfigUpdate.Uptime = DateTime.Now;
                mConfigUpdate.Name = Name;
                mConfigUpdate.Template = Template;
              
                try
                {
                    if ( _MessageConfigService.Update(mConfigUpdate)!= null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功！"));
                    }
                }
                catch
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败！"));
                }


            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
        }
        #endregion
    }
}
