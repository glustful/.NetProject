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
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [AllowAnonymous]
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
        /// 查询短信配置，返回短息配置信息
        /// </summary>
        /// <param name="Page">页码</param>
        /// <param name="PageCount">每页大小</param>
        /// <param name="isDescending">是否降序</param>
        /// <returns>短信配置信息</returns>
        [HttpGet]
        public HttpResponseMessage SearchMessageConfig(int page = 1, int pageSize = 10)
        {

            var mDetailCondition = new MessageConfigSearchCondition()
            {
                Page = Convert.ToInt32(page),
                PageCount = pageSize

            };
            var list = _MessageConfigService.GetMessageConfigsByCondition(mDetailCondition).Select(c => new { c.Id, c.Name, c.Template }).ToList();

            var listCount = _MessageConfigService.GetMessageConfigCount(mDetailCondition);
            return PageHelper.toJson(new { List = list, Condition = mDetailCondition, totalCount = listCount });
          
        }

        /// <summary>
        /// 传入短信id，获取一条配置信息，返回配置信息
        /// </summary>
        /// <param name="id">短信id</param>
        /// <returns>配置信息</returns>
        [HttpGet]
        public HttpResponseMessage GetMessageConfig(string id)
        {
            return PageHelper.toJson(_MessageConfigService.GetMessageConfigById(Convert.ToInt32(id)));
        }


        /// <summary>
        /// 传入短信配置模板参数，添加短信配置模板，返回添加结果状态信息
        /// </summary>
        /// <param name="messageConfigModel">短信配置模板参数</param>
        /// <returns></returns>
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
        /// 传入短信配置id，删除短信配置，返回删除结果状态信息
        /// </summary>
        /// <param name="id">短信配置ID</param>
        /// <returns>删除短信配置结果状态信息</returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage DeleteMessageConfig(string  Id)
        {
            if (!string.IsNullOrEmpty(Id) && PageHelper.ValidateNumber(Id))
            {
                if (_MessageConfigService.Delete(_MessageConfigService.GetMessageConfigById(Convert.ToInt32(Id))))
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
        /// 传入短信配置模板参数，更新短信配置模板，返回短信配置模板参数更新结果状态信息。
        /// </summary>
        /// <param name="messageConfigModel">短信配置模板参数</param>
        /// <returns>短信配置模板参数更新结果状态信息</returns>
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




        [HttpGet]
        public HttpResponseMessage  GetMessageConfigNameList()
        {
           

            List<string> list = EnumToList(typeof(MessageConfigTypeEnum));         
            return PageHelper.toJson(new { List = list});
        }



        /// <summary>
        /// 将枚举转换成ArrayList
        /// </summary>
        /// <returns></returns>
        public  List<string> EnumToList(Type enumType)
        {
            List<string> list = new List<string>();
            foreach (int i in Enum.GetValues(enumType))
            {
               // ListItem listitem = new ListItem(Enum.GetName(enumType, i), i.ToString());
                list.Add(Enum.GetName(enumType, i));
            }

            return list;
        } 


        #endregion
    }
}
