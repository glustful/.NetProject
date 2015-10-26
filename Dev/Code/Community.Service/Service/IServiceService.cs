using System.Linq;
using Community.Entity.Model.Service;
using YooPoon.Core.Autofac;

namespace Community.Service.Service
{
    public interface IServiceService : IDependency
    {
        ServiceEntity Create(ServiceEntity entity);

        bool Delete(ServiceEntity entity);

        ServiceEntity Update(ServiceEntity entity);

        ServiceEntity GetServiceById(int id);

        IQueryable<ServiceEntity> GetServiceByCondition(ServiceSearchCondition condition);

        int GetServiceCount(ServiceSearchCondition condition);
    }
}
