﻿using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;
using CRM.Service.EventOrder;

namespace CRM.Service.Event
{
    public class EventService : IEventService
    {
        private readonly Zerg.Common.Data.ICRMRepository<EventEntity> _eventRepository;
        private readonly ILog _log;

        public EventService(Zerg.Common.Data.ICRMRepository<EventEntity> eventRepository, ILog log)
        {
            _eventRepository = eventRepository;
            _log = log;
        }

        public EventEntity Create(EventEntity entity)
        {
            try
            {
                _eventRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public bool Delete(EventEntity entity)
        {
            try
            {
                _eventRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return false;
            }
        }

        public EventEntity Update(EventEntity entity)
        {
            try
            {
                _eventRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public EventEntity GetEventById(int id)
        {
            try
            {
                return _eventRepository.GetById(id); ;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public IQueryable<EventEntity> GetEventByCondition(EventSearchCondition condition)
        {
            var query = _eventRepository.Table;
            try
            {
                if (condition.Starttime.HasValue)
                {
                    query = query.Where(q => q.Starttime >= condition.Starttime.Value);
                }
                if (condition.Endtime.HasValue)
                {
                    query = query.Where(q => q.Endtime < condition.Endtime.Value);
                }
                if (!string.IsNullOrEmpty(condition.EventContent))
                {
                    query = query.Where(q => q.EventContent.Contains(condition.EventContent));
                }
               
                return query;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }

        }




    }
}