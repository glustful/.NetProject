using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.PointDetail
{
	public interface IPointDetailService : IDependency
	{
		PointDetailEntity Create (PointDetailEntity entity);

		bool Delete(PointDetailEntity entity);

		PointDetailEntity Update (PointDetailEntity entity);

		PointDetailEntity GetPointDetailById (int id);

		IQueryable<PointDetailEntity> GetPointDetailsByCondition(PointDetailSearchCondition condition);

		int GetPointDetailCount (PointDetailSearchCondition condition);
	}
}