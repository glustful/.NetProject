using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;
using CRM.Service.EventOrder;

namespace CRM.Service.EventOrder
{
    public class EventOrderService : IEventOrderService
    {
        private readonly Zerg.Common.Data.ICRMRepository<EventOrderEntity> _eventorderRepository;
        private readonly ILog _log;

        public EventOrderService(Zerg.Common.Data.ICRMRepository<EventOrderEntity> eventorderRepository, ILog log)
        {
            _eventorderRepository = eventorderRepository;
            _log = log;
        }

        public EventOrderEntity Create(EventOrderEntity entity)
        {
            try
            {
                _eventorderRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public bool Delete(EventOrderEntity entity)
        {
            try
            {
                _eventorderRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return false;
            }
        }

        public EventOrderEntity Update(EventOrderEntity entity)
        {
            try
            {
                _eventorderRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

           public EventOrderEntity GetEventOrderById(int id)
        {
            try
            {
                return _eventorderRepository.GetById(id); ;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public IQueryable<EventOrderEntity> GetEventOrderByCondition(EventOrderSearchCondition condition)
        {
            var query = _eventorderRepository.Table;
            try
            {
                if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime >= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }
                if (!string.IsNullOrEmpty(condition.AcDetail))
                {
                    query = query.Where(q => q.AcDetail.Contains(condition.AcDetail));
                }
                if (condition.MoneyCount.HasValue)
                {
                    query = query.Where(q => q.MoneyCount == condition.MoneyCount.Value);
                }
                return query;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
   
        }

      

        public int GetEventOrderCount(EventOrderSearchCondition condition)
        {
            throw new NotImplementedException();
        }

     
    }
}
