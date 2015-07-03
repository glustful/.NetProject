using System.Linq;
using Event.Entity.Model;
using YooPoon.Core.Autofac;

namespace Event.Service.Crowd
{
	public interface ICrowdService : IDependency
	{
		CrowdEntity Create (CrowdEntity entity);

		bool Delete(CrowdEntity entity);

		CrowdEntity Update (CrowdEntity entity);

		CrowdEntity GetCrowdById (int id);

		IQueryable<CrowdEntity> GetCrowdsByCondition(CrowdSearchCondition condition);

		int GetCrowdCount (CrowdSearchCondition condition);
	}
}