using System.Linq;
using Trading.Entity.Entity.Area;
using YooPoon.Core.Autofac;

namespace Trading.Service.Area
{
    public  interface IAreaService : IDependency
    {
        AreaEntity Create(AreaEntity entity);
        AreaEntity Update(AreaEntity entity);
        bool Delete(AreaEntity entity);
        AreaEntity GetAreaById(int id);
        IQueryable<AreaEntity> GetAreaByCondition(AreaSearchCondition condition);
        int GetAreaCount(AreaSearchCondition condition);
    }
}
