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
        public List<MessageDetailEntity> MessageDetailSearchCondition(int[] Ids, DateTime AddtimeBegin, DateTime AddtimeEnd, int Page = 1, int PageCount = 1, bool isDescending = true)
        {
            var mDetail = new MessageDetailSearchCondition()
            {

                Page = Page,
                PageCount = PageCount,
                isDescending = isDescending,
                Ids = Ids,
                AddtimeBegin = AddtimeBegin,
                AddtimeEnd = AddtimeEnd
            };

            var temp = _messageDetailService.GetMessageDetailsByCondition(mDetail).ToList();
           

            return temp;
        }


        ///// <summary>
        ///// 删除短信
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[System.Web.Http.HttpGet]
        //public bool MessageDetailDelete(int id = 0)
        //{
        //    var mDetailDel = new MessageDetailEntity()
        //    {
        //        Id = id
        //    };
        //    return _messageDetailService.Delete(mDetailDel);

        //}


        /// <summary>
        /// /发送短信
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="content"></param>
        /// <param name="sender"></param>
        /// <param name="addtime"></param>
        /// <param name="mobile"></param>
        [System.Web.Http.HttpGet]
        public ResultModel  MessageDetailCreate(string Title, string content, string sender, string mobile)
        {
            var MessageDetailInsert = new MessageDetailEntity()
            {
                Title = Title,
                Content = content,
                Sender = sender,
                Mobile = mobile

            };
            _messageDetailService.Create(MessageDetailInsert);

            return new ResultModel { Status = true, Msg = "操作成功" };
        }
        #endregion
    }
}
