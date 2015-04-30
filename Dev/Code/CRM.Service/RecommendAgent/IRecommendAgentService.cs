using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.RecommendAgent
{
	public interface IRecommendAgentService : IDependency
	{
		RecommendAgentEntity Create (RecommendAgentEntity entity);

		bool Delete(RecommendAgentEntity entity);

		RecommendAgentEntity Update (RecommendAgentEntity entity);

		RecommendAgentEntity GetRecommendAgentById (int id);

		IQueryable<RecommendAgentEntity> GetRecommendAgentsByCondition(RecommendAgentSearchCondition condition);

		int GetRecommendAgentCount (RecommendAgentSearchCondition condition);
	}
}