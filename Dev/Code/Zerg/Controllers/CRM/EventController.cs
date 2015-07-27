using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.BrokeAccount;
using CRM.Service.Event;
using CRM.Service.EventOrder;
using CRM.Service.InvitedCode;
using CRM.Service.Level;
using Zerg.Common;

namespace Zerg.Controllers.CRM
{
      [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]

    /// <summary>
    /// 活动表  杨跃  2015-07-23
    /// </summary>
    public class EventController : ApiController
      {
          private readonly IEventService _eventService;
          private readonly IBrokeAccountService _brokerAccountService;
          private readonly IEventOrderService _eventOrderService;
          private readonly IInviteCodeService _inviteCodeService;
          private readonly ILevelService _levelService;



          public EventController(IBrokeAccountService brokerAccountService,
              IEventOrderService eventOrderService,
              IInviteCodeService inviteCodeService,
              ILevelService levelService,
              IEventService eventService
              )
          {
                _brokerAccountService = brokerAccountService;
                 _eventOrderService = eventOrderService;
                _inviteCodeService = inviteCodeService;
                _levelService = levelService;
              _eventService = eventService;
          }
        [Description("获取所有活动，返回活动列表")]
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
          public HttpResponseMessage GetEventList()
        {
            EventSearchCondition eventcoCondition = new EventSearchCondition();
            var EventList = _eventService.GetEventByCondition(eventcoCondition).Select(a => new
            {
                a.Id,
                a.EventContent,
                a.Starttime,
                a.Endtime,
                a.State
            }).ToList();
            return PageHelper.toJson(EventList);
            
        }

          [Description("添加活动")]
          [HttpGet]
          [EnableCors("*", "*", "*", SupportsCredentials = true)]
          public HttpResponseMessage AddEvent([FromBody] EventEntity EventModel)
          {
              Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
              var m = reg.IsMatch(EventModel.EventContent);
              if (!m)
              {
                  return PageHelper.toJson(PageHelper.ReturnValue(false, "存在非法字符！"));
              }
              else
              {
                  EventEntity ee = new EventEntity()
                  {
                      EventContent = EventModel.EventContent,
                      Starttime = DateTime.Now,
                      Endtime = DateTime.Now,
                      ActionControllers = null,
                      State = EventModel.State

                  };
                  try
                  {
                      _eventService.Create(ee);
                      return PageHelper.toJson(PageHelper.ReturnValue(true, "添加成功！"));
                  }
                  catch (Exception)
                  {

                      return PageHelper.toJson(PageHelper.ReturnValue(false, "不能添加自身！"));
                  }

              }
          }
      }
 }
