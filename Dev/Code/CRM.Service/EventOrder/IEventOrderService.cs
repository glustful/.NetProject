using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.EventOrder
{
    public interface IEventOrderService : IDependency
    {
        EventOrderEntity Create(EventOrderEntity entity);

        bool Delete(EventOrderEntity entity);

        EventOrderEntity Update(EventOrderEntity entity);

        EventOrderEntity GetEventOrderById(int id);

        IQueryable<EventOrderEntity> GetEventOrderByCondition(EventOrderSearchCondition condition);

        int GetEventOrderCount(EventOrderSearchCondition condition);
    }
}