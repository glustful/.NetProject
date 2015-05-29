using System.Linq;
using Trading.Entity.Model;
using YooPoon.Core.Autofac;

namespace Trading.Service.LandAgentBill
{
	public interface ILandAgentBillService : IDependency
	{
		LandAgentBillEntity Create (LandAgentBillEntity entity);

		bool Delete(LandAgentBillEntity entity);

		LandAgentBillEntity Update (LandAgentBillEntity entity);

		LandAgentBillEntity GetLandAgentBillById (int id);

		IQueryable<LandAgentBillEntity> GetLandAgentBillsByCondition(LandAgentBillSearchCondition condition);

		int GetLandAgentBillCount (LandAgentBillSearchCondition condition);
	}
}