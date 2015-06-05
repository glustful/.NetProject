using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using CRM.Service.MessageDetail;
using CRM.Entity.Model;
using Zerg.Common;
using Zerg.Models.CRM;
using System.Web.Http.Cors;

namespace Zerg.Controllers.CRM
{
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [AllowAnonymous]
    /// <summary>
    /// 短信发送明细
    /// </summary>
    public class MessageDetailController : ApiController
    {

        private readonly IMessageDetailService _messageDetailService;
        public MessageDetailController(IMessageDetailService messageDetailService)
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
        [HttpGet]
        public HttpResponseMessage SearchMessageDetail(string endTime, string startTime, string type, int page = 1, int pageSize = 10)
        {
            //================================================赵旭初 by 2015-05-13 start===============================================================
            string strStarttime = "";
            string strEndtime = "";
            string strType = "";

            if (startTime == null || startTime.Length <= 0) strStarttime = "1900-01-01";
            else strStarttime = startTime.Substring(0, 10);

            if (endTime == null || endTime.Length <= 0)
                strEndtime = string.Format(DateTime.Now.ToShortDateString(), "yyyy-mm-dd");
            else
                strEndtime = endTime.Substring(0, 10);

            strType = type;


            var mDetailCondition = new MessageDetailSearchCondition()
            {
                AddtimeBegin = Convert.ToDateTime(strStarttime),
                AddtimeEnd = Convert.ToDateTime(strEndtime), 
                Title = strType,
                 Page = Convert.ToInt32(page),
                PageCount = pageSize
            };

         
            var list = _messageDetailService.GetMessageDetailsByCondition(mDetailCondition).Select(c => new { c.Id, c.Title, c.Sender, c.Mobile, c.Content,c.Addtime }).ToList();
            var listCount = _messageDetailService.GetMessageDetailsByCondition(mDetailCondition);
            return PageHelper.toJson(new { List = list, Condition = mDetailCondition, totalCount = listCount });
          
            //================================================赵旭初 by 2015-05-13 end===============================================================
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
        public HttpResponseMessage AddMessageDetail(MessageDetailModel messageDetailModel)
        {
            if (!string.IsNullOrEmpty(messageDetailModel.Title) && !string.IsNullOrEmpty(messageDetailModel.Content) && !string.IsNullOrEmpty(messageDetailModel.Sender) && !string.IsNullOrEmpty(messageDetailModel.Mobile))
            {
                if (!PageHelper.IsMobilePhone(messageDetailModel.Mobile))
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "手机号格式验证错误！"));
                }
                var MessageDetailInsert = new MessageDetailEntity()
                {
                    Title = messageDetailModel.Title,
                    Content = messageDetailModel.Content,
                    Sender = messageDetailModel.Sender,
                    Mobile = messageDetailModel.Mobile,
                    Addtime = DateTime.Now

                };

                try
                {
                    if (_messageDetailService.Create(MessageDetailInsert) != null)
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
        /// 获取SMS平台可用数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetSMSCount()
        {
            return PageHelper.toJson(SMSHelper.GetSMSCount());
        }
        #endregion

        [HttpPost]
        public HttpResponseMessage SendMessage([FromBody] string phone)
        {
            if (!string.IsNullOrEmpty(phone))
            {

            }
            var p = SMSHelper.Sending(phone, "你好，感谢你使用创富宝软件！发送时间是：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return PageHelper.toJson(p);
        }
    }
}
