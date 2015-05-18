using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRM.Service.MessageConfig;
using CRM.Entity.Model;
using Zerg.Common;
using Zerg.Models.CRM;
using System.Web.Http.Cors;

namespace Zerg.Controllers.CRM
{
    [EnableCors("*", "*", "*")]
    /// <summary>
    /// 短信配置
    /// </summary>
    public class MessageConfigController : ApiController
    {
        public readonly IMessageConfigService _MessageConfigService;
        public MessageConfigController(IMessageConfigService messageConfig)
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
        [HttpGet]
        public HttpResponseMessage SearchMessageConfig(string page, string pageSize, string totalPage)
        {

            var mDetailCondition = new MessageConfigSearchCondition()
            {
                Page = Convert.ToInt16(page),
                PageCount = 100

            };
            var list = _MessageConfigService.GetMessageConfigsByCondition(mDetailCondition).Select(c => new { c.Id, c.Name, c.Template }).ToList();

            return PageHelper.toJson(list);
        }

        /// <summary>
        /// 添加短信配置模板
        /// </summary>
        /// <param name="Name">模板名称</param>
        /// <param name="Template">配置模板</param>
        [HttpPost]
        public HttpResponseMessage AddMessageConfig(CreateMessageConfigModel messageConfigModel)
        {
            if (!string.IsNullOrEmpty(messageConfigModel.Name) && !string.IsNullOrEmpty(messageConfigModel.Template))
            {
                var MessageConfigInsert = new MessageConfigEntity()
                {
                    Name = messageConfigModel.Name,
                    Template = messageConfigModel.Template,
                    Uptime = DateTime.Now,
                    Addtime = DateTime.Now
                };
                try
                {
                    if (_MessageConfigService.Create(MessageConfigInsert) != null)
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
        public HttpResponseMessage DeleteMessageConfig(MessageConfigModel messageConfigModel)
        {
            if (!string.IsNullOrEmpty(messageConfigModel.Id) && PageHelper.ValidateNumber(messageConfigModel.Id))
            {
                if (_MessageConfigService.Delete(_MessageConfigService.GetMessageConfigById(Convert.ToInt32(messageConfigModel.Id))))
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
        public HttpResponseMessage UpdateMessageConfig(MessageConfigModel messageConfigModel)
        {
            if (!string.IsNullOrEmpty(messageConfigModel.Id) && PageHelper.ValidateNumber(messageConfigModel.Id) && !string.IsNullOrEmpty(messageConfigModel.Name) && !string.IsNullOrEmpty(messageConfigModel.Template))
            {

                var mConfigUpdate = _MessageConfigService.GetMessageConfigById(Convert.ToInt32(messageConfigModel.Id));
                mConfigUpdate.Uptime = DateTime.Now;
                mConfigUpdate.Name = messageConfigModel.Name;
                mConfigUpdate.Template = messageConfigModel.Template;

                try
                {
                    if (_MessageConfigService.Update(mConfigUpdate) != null)
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
