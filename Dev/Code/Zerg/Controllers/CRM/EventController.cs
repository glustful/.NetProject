using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.Event;
using Zerg.Common;
using Zerg.Models.CRM;

namespace Zerg.Controllers.CRM
{
      [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
      public class EventController : ApiController
      {
          private readonly IEventService _eventService;


          public EventController(IEventService eventService
              )
          {
              _eventService = eventService;
          }
        [Description("获取所有活动，返回活动列表")]
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
          public HttpResponseMessage GetEventList()
        {
            EventSearchCondition eventcoCondition = new EventSearchCondition();
            var eventList = _eventService.GetEventByCondition(eventcoCondition).Select(a => new
            {
                a.Id,
                a.EventContent,
                a.Starttime,
                a.Endtime,
                a.State
            }).ToList();
            return PageHelper.toJson(eventList);
            
        }

          [Description("添加活动")]
          [HttpPost]
          [EnableCors("*", "*", "*", SupportsCredentials = true)]
          public HttpResponseMessage AddEvent([FromBody] EventEntity eventModel)
          {
              Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
              var m = reg.IsMatch(eventModel.EventContent);
              if (!m)
              {
                  return PageHelper.toJson(PageHelper.ReturnValue(false, "存在非法字符！"));
                  
              }
              else
              {
                  EventEntity ee = new EventEntity()
                  {
                      EventContent = eventModel.EventContent,
                      Starttime = eventModel.Starttime,
                      Endtime = eventModel.Endtime,
                      ActionControllers = eventModel.ActionControllers,
                      State = eventModel.State

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
          [Description("修改活动")]
          [HttpPost]
          [EnableCors("*", "*", "*", SupportsCredentials = true)]
          public HttpResponseMessage UpEvent( EventEntity eventModel)
          {
              Regex reg = new Regex(@"^[^ %@#!*~&',;=?$\x22]+$");
              var m = reg.IsMatch(eventModel.EventContent);
              if (!m)
              {
                  return PageHelper.toJson(PageHelper.ReturnValue(false, "存在非法字符！"));
              }
              else
              {
                  var e = _eventService.GetEventById(eventModel.Id);
                  e.EventContent = eventModel.EventContent;
                  e.Starttime = eventModel.Starttime;
                  e.Endtime = eventModel.Endtime;
                  e.ActionControllers = eventModel.ActionControllers;
                  e.State = eventModel.State; 
                  if (_eventService.Update(e) != null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "数据修改成功！"));
                    }
                  else
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败！"));
                    }

              }
          }


          [Description("删除活动")]
          [HttpPost]
          [EnableCors("*", "*", "*", SupportsCredentials = true)]
          public HttpResponseMessage DelEventById(string id)
          {
              try
              {
                  _eventService.Delete(_eventService.GetEventById(Convert.ToInt32(id)));
                  return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功！"));
              }
              catch (Exception)
              {
                  return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败！"));
              }

          }

          [Description("根据Id获取活动")]
          [HttpGet]
          [EnableCors("*", "*", "*", SupportsCredentials = true)]
          public HttpResponseMessage GetEventDetail(string id)
          {
              var eve = _eventService.GetEventById(Convert.ToInt32(id));
              if (eve == null)
              {
                  return PageHelper.toJson(PageHelper.ReturnValue(false, "活动不存在"));
              }
              var model = new EventModel
              {
                 Id = eve.Id,
                 EventContent = eve.EventContent,
                 StartTime =eve.Starttime,
                 EndTime = eve.Endtime,
                 ActionControllers =eve.ActionControllers,
                 State =eve.State
              };
              return PageHelper.toJson(model);
          }
      }
 }
