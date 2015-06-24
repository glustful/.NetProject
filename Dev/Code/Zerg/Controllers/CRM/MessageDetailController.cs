using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using CRM.Service.MessageDetail;
using CRM.Entity.Model;
using Zerg.Common;
using Zerg.Models.CRM;
using System.Web.Http.Cors;
using YooPoon.Common.Encryption;
using Zerg.Common.Com;
using CRM.Service.Broker;
using YooPoon.WebFramework.User.Services;
using System.ComponentModel;

//using System.Collections.Generic;

namespace Zerg.Controllers.CRM
{
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [AllowAnonymous]
    /// <summary>
    /// 短信发送明细
    /// </summary>
    [Description("短信发送明细类")]
    public class MessageDetailController : ApiController
    {

        private readonly IMessageDetailService _messageDetailService;
        private readonly IUserService _userService;
        /// <summary>
        /// 短信发送明细类初始化
        /// </summary>
        /// <param name="messageDetailService">messageDetailService</param>
        /// <param name="UserService">UserService</param>
        public MessageDetailController(IMessageDetailService messageDetailService,
             IUserService UserService)
        {
            _messageDetailService = messageDetailService;
            _userService = UserService;
        }

        #region 短信发送明细  黄秀宇  2015.04.28
        /// <summary>
        /// 传入短信参数，检索查询短信详细信息，返回短信详细信息
        /// </summary>
        /// <param name="endTime">结束时间</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="type">类型</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>短信详细信息</returns>
        [Description("检索返回短信详细信息")]
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


            var list = _messageDetailService.GetMessageDetailsByCondition(mDetailCondition).Select(c => new { c.Id, c.Title, c.Sender, c.Mobile, c.Content, c.Addtime }).ToList();
            var listCount = _messageDetailService.GetMessageDetailsByCondition(mDetailCondition);
            return PageHelper.toJson(new { List = list, Condition = mDetailCondition, totalCount = listCount });

            //================================================赵旭初 by 2015-05-13 end===============================================================
        }


        /// <summary>
        /// 传入短信参数，添加短信信息，返回添加结果状态信息
        /// </summary>
        /// <param name="messageDetailModel">短信参数</param>
        /// <returns>短信发送结果状态信息</returns>
        [Description("添加短信详情")]
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
        /// <returns>SMS平台可用数量</returns>
        [HttpGet]
        [Description("获取SMS平台可用数量")]
        public HttpResponseMessage GetSMSCount()
        {
            return PageHelper.toJson(SMSHelper.GetSMSCount());
        }
        #endregion

        #region 黄秀宇 发送短信验证码 2015.06.05
        /// <summary>
        /// 发送短信验证码，返回发送结果状态信息
        /// </summary>
        /// <param name="phone">手机号码</param>
        ///  /// <param name="userid">用户ID</param>
        /// <returns>短信验证码</returns>
        [Description("发送短信验证码，返回发送结果状态信息")]
        [HttpPost]
        public HttpResponseMessage SendMessage([FromBody] string phone, int userid)
        {
            string salt = _userService.FindUser(userid).PasswordSalt.ToString();//查找用户密钥
            return ValidateMessage.SendMessage6(phone, salt);//发送短信验证码
        }
        /// <summary>
        /// 验证短信验证码，返回发送结果状态信息
        /// </summary>
        /// <param name="sourc">加密后的字符串</param>
        /// <param name="messa">验证码</param>
        /// <param name="userid">用户id,EMS加密用</param>
        /// <returns>短信验证码</returns>
        [Description("发送短信验证码，返回发送结果状态信息")]
        [HttpGet]
        public HttpResponseMessage validate([FromBody] string sourc, string messa, int userid)
        {

            string salt = _userService.FindUser(userid).PasswordSalt.ToString();//查找用户密钥
            return ValidateMessage.validate(sourc, messa, salt);//验证短信验证码


        }
        #endregion
    }
}
