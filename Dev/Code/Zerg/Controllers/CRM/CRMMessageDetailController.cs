using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRM.Service.MessageDetail;
using CRM.Entity.Model;
using Zerg.Common;

namespace Zerg.Controllers.CRM
{
    /// <summary>
    /// 短信发送明细
    /// </summary>
    public class CRMMessageDetailController : ApiController
    {

        private readonly IMessageDetailService _messageDetailService;
        public CRMMessageDetailController(IMessageDetailService messageDetailService)
        {
            _messageDetailService = messageDetailService;
        }
        #region 短信发送明细  黄秀宇  2015.04.28
        /// <summary>
        /// 查询短信明细
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="PageCount"></param>
        /// <param name="isDescending"></param>
        /// <param name="Ids"></param>
        /// <param name="AddtimeBegin"></param>
        /// <param name="AddtimeEnd"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public List<MessageDetailEntity> SearchMessageDetail( string  AddtimeBegin, string  AddtimeEnd, string title)
        {
            var mDetail = new MessageDetailSearchCondition()
            {

                AddtimeBegin =DateTime .Parse( AddtimeBegin),
                AddtimeEnd  =DateTime .Parse(AddtimeEnd),
                Title =title 

            };

            return  _messageDetailService.GetMessageDetailsByCondition(mDetail).ToList();
        }


        /// <summary>
        /// /发送短信
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="content"></param>
        /// <param name="sender"></param>
        /// <param name="addtime"></param>
        /// <param name="mobile"></param>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddMessageDetail(string Title, string content, string sender, string mobile)
        {
            if (!string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(sender) && !string.IsNullOrEmpty(mobile))
            {
                var MessageDetailInsert = new MessageDetailEntity()
                {
                    Title = Title,
                    Content = content,
                    Sender = sender,
                    Mobile = mobile,
                    Addtime = DateTime.Now

                };

                try
                {
                    if (  _messageDetailService.Create(MessageDetailInsert) != null)
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
        #endregion
    }
}
