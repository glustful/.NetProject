using System.Linq;
using Community.Entity.Model.Area;
using YooPoon.Core.Autofac;

namespace Community.Service.Area
{
	public interface IAreaService : IDependency
	{
		AreaEntity Create (AreaEntity entity);

		bool Delete(AreaEntity entity);

		AreaEntity Update (AreaEntity entity);

		AreaEntity GetAreaById (int id);

		IQueryable<AreaEntity> GetAreasByCondition(AreaSearchCondition condition);

		int GetAreaCount (AreaSearchCondition condition);
        //IQueryable<AreaEntity> GetsuperArea(int father);
       
	}
}