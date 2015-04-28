using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using CRM.Entity.Model;
using CRM.Service.MessageDetail;

namespace Zerg.Controllers
{

    /// <summary>
    /// CRM  管理员
    /// </summary>
    public class CRMAdminController : ApiController
    {
        private readonly IMessageDetailService _messageDetailService;
        public CRMAdminController(IMessageDetailService messageDetailService )
        {
            _messageDetailService = messageDetailService;

        }
        #region   huangxiuyu   2015.4.28 
        /// <summary>
      /// 查询短信列表
      /// </summary>
      /// <param name="Page"></param>
      /// <param name="PageCount"></param>
      /// <param name="isDescending"></param>
      /// <param name="Ids"></param>
      /// <param name="AddtimeBegin"></param>
      /// <param name="AddtimeEnd"></param>
        /// <returns></returns>
       
        [System.Web.Http.HttpGet]
        public List<MessageDetailEntity> MessageDetailSearchCondition( int[] Ids, DateTime AddtimeBegin , DateTime AddtimeEnd, int Page=1, int PageCount=1, bool isDescending=true)
        {
            var mDetail = new MessageDetailSearchCondition()
            {
     
            Page=Page,         
            PageCount=PageCount,
            isDescending=isDescending,
            Ids=Ids,
            AddtimeBegin=AddtimeBegin,
            AddtimeEnd=AddtimeEnd
            };

            var test = _messageDetailService.GetMessageDetailsByCondition(mDetail);
            var temp = (from uu in test select uu).ToList();

            return temp;
        } 
        #endregion
        #region   huangxiuyu   2015.4.28
        /// <summary>
        /// 删除短信
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public bool MessageDetailDelete(int  id=0)
        {
            var mDetailDel = new MessageDetailEntity()
            {
                Id=id
            };
            return _messageDetailService.Delete(mDetailDel);
           
        }
        #endregion
        #region   huangxiuyu   2015.4.28
        /// <summary>
        /// /发送短信
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="content"></param>
        /// <param name="sender"></param>
        /// <param name="addtime"></param>
        /// <param name="mobile"></param>
        [System.Web.Http.HttpGet]
        public  void  MessageDetailCreate(string Title,string content,string sender,DateTime addtime,string mobile)
        {
            var MessageDetailInsert = new MessageDetailEntity()
            {
                Title = Title,
                Content = content,
                Sender = sender,
                Addtime = addtime,
                Mobile = mobile

            };
           _messageDetailService.Create(MessageDetailInsert);
          //if ( !=null)
          //{

          //}



        }
        #endregion



    }
}