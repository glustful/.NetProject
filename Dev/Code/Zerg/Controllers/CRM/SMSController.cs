﻿using CRM.Service.Broker;
using CRM.Service.MessageConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using YooPoon.Common.Encryption;
using YooPoon.Core.Site;
using YooPoon.WebFramework.User.Entity;
using Zerg.Common;

namespace Zerg.Controllers.CRM
{
    /// <summary>
    /// 短信 发送通用
    /// </summary>
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    public class SMSController : ApiController
    {
        private readonly IWorkContext _workContext;
        private readonly IBrokerService _brokerService;
        private readonly IMessageConfigService _MessageConfigService;
        public SMSController(IWorkContext workContext, IBrokerService brokerService, IMessageConfigService MessageConfigService)
        {
            _workContext = workContext;
            _brokerService = brokerService;
            _MessageConfigService =MessageConfigService;
        }

        /// <summary>
        /// 通过用户输入手机号发送短信
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="smstype">短信类型( 注册=0,修改密码=1,找回密码=2,添加银行卡=3,佣金提现=4,添加合伙人=5,推荐经纪人=6)</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SendSMS([FromBody] string mobile,string smstype)
        {
            if(!string.IsNullOrEmpty(mobile) && !string.IsNullOrEmpty(smstype))
            {
                var messageConfigName = Enum.GetName(typeof(MessageConfigTypeEnum), Convert.ToInt32(smstype));//获取短信模版名称
                var messageTemplate = _MessageConfigService.GetMessageConfigByName(messageConfigName).Template;//获取到的短信模版
                var messages = "";

                string strNumber = new Random().Next(100000, 1000000).ToString();//生成大于等于100000，小于等于999999的随机数，也就是六位随机数                                  
                string nowTimestr = DateTime.Now.ToLongTimeString();
                var strs = EncrypHelper.Encrypt(strNumber + "$" + nowTimestr, "des");//EMS 加密短信验证码                  
                messages = string.Format(messageTemplate, ""); //更改模版

                //返回到前台的加密内容  和短信发送返回值
                return PageHelper.toJson(new { Desstr = strs, Message = SMSHelper.Sending(mobile, messages) });
                    
            }

            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据错误，不能发送短信"));
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="smstype">短信类型( 注册=0,修改密码=1,找回密码=2,添加银行卡=3,佣金提现=4,添加合伙人=5,推荐经纪人=6)</param>
        /// <returns></returns>
        public HttpResponseMessage  SendSmsForbroker([FromBody] string smstype)
        {
            var user = (UserBase)_workContext.CurrentUser;
            if(user!=null)
            {
                var broker = _brokerService.GetBrokerByUserId(user.Id);//获取当前经纪人
                var messageConfigName=Enum.GetName(typeof(MessageConfigTypeEnum),Convert.ToInt32(smstype));//获取短信模版名称
                var messageTemplate = _MessageConfigService.GetMessageConfigByName(messageConfigName).Template;//获取到的短信模版
                var messages="";
                if(messageConfigName=="添加合伙人" || messageConfigName=="推荐经纪人")//不需要生成数字验证码
                {
                    messages=string.Format(messageTemplate,"");
                    return PageHelper.toJson(SMSHelper.Sending(broker.Phone, messages));

                }else//生成数字验证码
                {
                    string strNumber = new Random().Next(100000, 1000000).ToString();//生成大于等于100000，小于等于999999的随机数，也就是六位随机数                                  
                    string nowTimestr = DateTime.Now.ToLongTimeString();
                    var strs = EncrypHelper.Encrypt(strNumber + "$" + nowTimestr, "des");//EMS 加密短信验证码                  
                    messages = string.Format(messageTemplate, ""); //更改模版

                    //返回到前台的加密内容  和短信发送返回值
                    return PageHelper.toJson(new { Desstr =strs, Message = SMSHelper.Sending(broker.Phone, messages) });
                    
                }
              

               
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
        }





    }
}