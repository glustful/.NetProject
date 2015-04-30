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
    public class CRMMessageConfigController : ApiController
    {
        public readonly IMessageConfigService _MessageConfigService;
        public CRMMessageConfigController (IMessageConfigService messageConfig)
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
        public List<MessageConfigEntity> MessageConfigSearchCondition(int Page = 1, int PageCount = 1, bool isDescending = true)
        {
            var mConfig = new MessageConfigSearchCondition()
            {
                Page = Page,
                PageCount = PageCount,
                isDescending = isDescending

            };
            var temp = _MessageConfigService.GetMessageConfigsByCondition(mConfig).ToList();
            return temp;
        }
        /// <summary>
        /// 添加短信配置模板
        /// </summary>
        /// <param name="Name">模板名称</param>
        /// <param name="Template">配置模板</param>
        [System.Web.Http.HttpGet]
        public ResultModel  MessageConfigCreate(string Name, string Template)
        {
            var MessageConfigInsert = new MessageConfigEntity()
            {
                Name = Name,
                Template = Template
            };
            _MessageConfigService.Create(MessageConfigInsert);
            return new ResultModel { Status = true, Msg = "操作成功" };
        }
        /// <summary>
        /// 删除短信配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public ResultModel  MessageConfigDelete(int id = 0)
        {
            var mConfigDel = new MessageConfigEntity()
            {
                Id = id
            };
            _MessageConfigService.Delete(mConfigDel);
               return new ResultModel { Status = true, Msg = "操作成功" }; 
            
        }
        /// <summary>
        /// 更新短信配置模板
        /// </summary>
        /// <param name="Name">模板名称</param>
        /// <param name="Template">配置模板</param>
        [System.Web.Http.HttpGet]
        public ResultModel  MessageConfigUpdate(string Name, string Template)
        {
            var mConfigUpdate = new MessageConfigEntity()
            {
                Name = Name,
                Template = Template
            };
            _MessageConfigService.Update(mConfigUpdate);
            return new ResultModel { Status = true, Msg = "操作成功" };
        }
        #endregion
    }
}
