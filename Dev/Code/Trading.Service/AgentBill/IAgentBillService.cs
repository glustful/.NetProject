using System.Linq;
using Trading.Entity.Model;
using YooPoon.Core.Autofac;

namespace Trading.Service.AgentBill
{
	public interface IAgentBillService : IDependency
	{
		AgentBillEntity Create (AgentBillEntity entity);

		bool Delete(AgentBillEntity entity);

		AgentBillEntity Update (AgentBillEntity entity);

		AgentBillEntity GetAgentBillById (int id);

		IQueryable<AgentBillEntity> GetAgentBillsByCondition(AgentBillSearchCondition condition);

		int GetAgentBillCount (AgentBillSearchCondition condition);
	}
}