using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.PointExchangeDetail
{
	public interface IPointExchangeDetailService : IDependency
	{
		PointExchangeDetailEntity Create (PointExchangeDetailEntity entity);

		bool Delete(PointExchangeDetailEntity entity);

		PointExchangeDetailEntity Update (PointExchangeDetailEntity entity);

		PointExchangeDetailEntity GetPointExchangeDetailById (int id);

		IQueryable<PointExchangeDetailEntity> GetPointExchangeDetailsByCondition(PointExchangeDetailSearchCondition condition);

		int GetPointExchangeDetailCount (PointExchangeDetailSearchCondition condition);
	}
}