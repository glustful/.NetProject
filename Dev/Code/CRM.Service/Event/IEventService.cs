using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.Event
{
    public interface IEventService : IDependency
    {
        EventEntity Create(EventEntity entity);

        bool Delete(EventEntity entity);

        EventEntity Update(EventEntity entity);

        EventEntity GetEventById(int id);

        IQueryable<EventEntity> GetEventByCondition(EventSearchCondition condition);
    }
}
