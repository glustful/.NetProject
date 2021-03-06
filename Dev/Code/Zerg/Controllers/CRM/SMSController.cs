﻿using System;
using System.ComponentModel;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.MessageConfig;
using CRM.Service.MessageDetail;
using YooPoon.Common.Encryption;
using YooPoon.Core.Site;
using YooPoon.WebFramework.User.Entity;
using Zerg.Common;

namespace Zerg.Controllers.CRM
{
    /// <summary>
    ///     短信 发送通用
    /// </summary>
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [Description("通用短信发送管理类")]
    public class SMSController : ApiController
    {
        private readonly IBrokerService _brokerService;
        private readonly IMessageConfigService _messageConfigService;
        private readonly IMessageDetailService _messageService;
        private readonly IWorkContext _workContext;

        /// <summary>
        ///     通用短信发送管理类
        /// </summary>
        /// <param name="workContext">workContext</param>
        /// <param name="brokerService">brokerService</param>
        /// <param name="messageConfigService">MessageConfigService</param>
        /// <param name="messageService">MessageService</param>
        [Description("通用短信发送管理初始化")]
        public SMSController(IWorkContext workContext, IBrokerService brokerService,
            IMessageConfigService messageConfigService, IMessageDetailService messageService)
        {
            _workContext = workContext;
            _brokerService = brokerService;
            _messageConfigService = messageConfigService;
            _messageService = messageService;
        }

        /// <summary>
        /// 通过用户输入手机号发送短信，返回短信发送结果状态信息。（手机号，短信类型）
        /// </summary>
        /// <returns>短信发送结果状态信息</returns>
        [Description("用户输入手机号发送短信")]
        [HttpPost]
        public HttpResponseMessage SendSMS([FromBody] YzMsg yzmsg)
        {
            if (!string.IsNullOrEmpty(yzmsg.Mobile) && !string.IsNullOrEmpty(yzmsg.SmsType))
            {
                var messageConfigName = Enum.GetName(typeof (MessageConfigTypeEnum), Convert.ToInt32(yzmsg.SmsType));
                    //获取短信模版名称
                var messageTemplate = _messageConfigService.GetMessageConfigByName(messageConfigName).Template;
                    //获取到的短信模版
                string messages;

                var strNumber = new Random().Next(100000, 1000000).ToString();
                    //生成大于等于100000，小于等于999999的随机数，也就是六位随机数                                  
                var nowTimestr = DateTime.Now.ToLongTimeString();
                var strs = EncrypHelper.Encrypt(strNumber+"#"+yzmsg.Mobile + "$" + nowTimestr, "Hos2xNLrgfaYFY2MKuFf3g==");
                    //EMS 加密短信验证码 

                if (messageConfigName == "推荐经纪人") //不需要生成数字验证码
                {
                    var user = (UserBase) _workContext.CurrentUser;
                    if (user != null)
                    {
                        var broker = _brokerService.GetBrokerByUserId(user.Id); //获取当前经纪人
                        if (broker == null)
                        {
                            return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
                        }
                        if (broker.Phone == yzmsg.Mobile) //不能给自己发
                        {
                            return PageHelper.toJson(PageHelper.ReturnValue(false, "对不起，不能给自己发送短信"));
                        }

                        //添加到短信表中去 
                        messages = string.Format(messageTemplate, strNumber, broker.Brokername); //更改模版 
                        AddMessageDetails(new MessageDetailEntity
                        {
                            Content = messages,
                            InvitationCode = strNumber,
                            InvitationId = broker.Id.ToString(),
                            Mobile = yzmsg.Mobile,
                            Sender = yzmsg.Mobile,
                            Title = messageConfigName,
                            Addtime = DateTime.Now
                        });
                        return PageHelper.toJson(SMSHelper.Sending(yzmsg.Mobile, messages));
                    }
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
                }

                messages = string.Format(messageTemplate, strNumber); //更改模版

                //添加到短信表中去 
                AddMessageDetails(new MessageDetailEntity
                {
                    Content = messages,
                    Mobile = yzmsg.Mobile,
                    Sender = yzmsg.Mobile,
                    Title = messageConfigName,
                    Addtime = DateTime.Now
                });


                //返回到前台的加密内容  和短信发送返回值
                return PageHelper.toJson(new {Desstr = strs, Message = SMSHelper.Sending(yzmsg.Mobile, messages)});
            }

            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据错误，不能发送短信"));
        }

        /// <summary>
        /// 发送短信（短信类型）
        /// </summary>
        /// <param name="smstype">短信类型(修改密码=1,找回密码=2,添加银行卡=3,佣金提现=4,)</param>
        /// <returns>发送短信结果状态信息</returns>
        public HttpResponseMessage SendSmsForbroker([FromBody] string smstype)
        {
            var user = (UserBase) _workContext.CurrentUser;
            if (user != null)
            {
                var broker = _brokerService.GetBrokerByUserId(user.Id); //获取当前经纪人
                if (broker == null)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
                }
                var messageConfigName = Enum.GetName(typeof (MessageConfigTypeEnum), Convert.ToInt32(smstype));
                    //获取短信模版名称
                var messageTemplate = _messageConfigService.GetMessageConfigByName(messageConfigName).Template;
                    //获取到的短信模版
                string messages;
                if (messageConfigName == "推荐经纪人") //不需要生成数字验证码
                {
                    messages = string.Format(messageTemplate, ""); //更改模版
                    //添加到短信表中去 
                    AddMessageDetails(new MessageDetailEntity
                    {
                        Content = messages,
                        Mobile = broker.Phone,
                        Sender = broker.Phone,
                        Title = messageConfigName,
                        Addtime = DateTime.Now
                    });

                    return PageHelper.toJson(SMSHelper.Sending(broker.Phone, messages));
                }
                var strNumber = new Random().Next(100000, 1000000).ToString();
                    //生成大于等于100000，小于等于999999的随机数，也就是六位随机数                                  
                var nowTimestr = DateTime.Now.ToLongTimeString();
                var strs = EncrypHelper.Encrypt(strNumber + "$" + nowTimestr, "Hos2xNLrgfaYFY2MKuFf3g==");
                    //EMS 加密短信验证码                  
                messages = string.Format(messageTemplate, strNumber); //更改模版

                //添加到短信表中去 
                AddMessageDetails(new MessageDetailEntity
                {
                    Content = messages,
                    Mobile = broker.Phone,
                    Sender = broker.Phone,
                    Title = messageConfigName,
                    Addtime = DateTime.Now
                });


                //返回到前台的加密内容  和短信发送返回值
                return PageHelper.toJson(new {Desstr = strs, Message = SMSHelper.Sending(broker.Phone, messages)});
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
        }


        /// <summary>
        /// 发送短信（短信类型 和经纪人Id）
        /// </summary>
        /// <param name="smstype">短信类型(修改密码=1,找回密码=2,添加银行卡=3,佣金提现=4,)</param>
        /// <returns>发送短信结果状态信息</returns>
        public HttpResponseMessage SendSmsForbrokerid([FromBody] YzMsg yzmsg)
        {
            if(string.IsNullOrEmpty( yzmsg.userId) || string.IsNullOrEmpty(yzmsg.SmsType) )
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "参数错误，发送失败"));
            }

            var broker = _brokerService.GetBrokerById(Convert.ToInt32(yzmsg.userId));

            //var user = (UserBase)_workContext.CurrentUser;
            //if (user != null)
            //{
            //    var broker = _brokerService.GetBrokerByUserId(user.Id); //获取当前经纪人
                if (broker == null)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
                }
                var messageConfigName = Enum.GetName(typeof(MessageConfigTypeEnum), Convert.ToInt32(yzmsg.SmsType));
                //获取短信模版名称
                var messageTemplate = _messageConfigService.GetMessageConfigByName(messageConfigName).Template;
                //获取到的短信模版
                string messages;
                if (messageConfigName == "推荐经纪人") //不需要生成数字验证码
                {
                    messages = string.Format(messageTemplate, ""); //更改模版
                    //添加到短信表中去 
                    AddMessageDetails(new MessageDetailEntity
                    {
                        Content = messages,
                        Mobile = broker.Phone,
                        Sender = broker.Phone,
                        Title = messageConfigName,
                        Addtime = DateTime.Now
                    });

                    return PageHelper.toJson(SMSHelper.Sending(broker.Phone, messages));
                }
                var strNumber = new Random().Next(100000, 1000000).ToString();
                //生成大于等于100000，小于等于999999的随机数，也就是六位随机数                                  
                var nowTimestr = DateTime.Now.ToLongTimeString();
                var strs = EncrypHelper.Encrypt(strNumber + "$" + nowTimestr, "Hos2xNLrgfaYFY2MKuFf3g==");
                //EMS 加密短信验证码                  
                messages = string.Format(messageTemplate, strNumber); //更改模版

                //添加到短信表中去 
                AddMessageDetails(new MessageDetailEntity
                {
                    Content = messages,
                    Mobile = broker.Phone,
                    Sender = broker.Phone,
                    Title = messageConfigName,
                    Addtime = DateTime.Now
                });


                //返回到前台的加密内容  和短信发送返回值
                return PageHelper.toJson(new { Desstr = strs, Message = SMSHelper.Sending(broker.Phone, messages) });
            
            return PageHelper.toJson(PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆"));
        }



        /// <summary>
        /// 添加到短信表中
        /// </summary>
        /// <param name="messageDetail">短信参数</param>
        public void AddMessageDetails(MessageDetailEntity messageDetail)
        {
            _messageService.Create(messageDetail);
        }
    }


    /// <summary>
    /// 验证消息（手机号，验证类型（注册=0,修改密码=1,找回密码=2,添加银行卡=3,佣金提现=4,添加合伙人=5,推荐经纪人=6））
    /// </summary>
    [Description("验证消息类")]
    public class YzMsg
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string SmsType { get; set; }

        /// <summary>
        /// 经纪人Id
        /// </summary>
        public string userId { get; set; }
    }
}