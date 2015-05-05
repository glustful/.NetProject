using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.BrokerBill
{
	public interface IBrokerBillService : IDependency
	{
		BrokerBillEntity Create (BrokerBillEntity entity);

		bool Delete(BrokerBillEntity entity);

		BrokerBillEntity Update (BrokerBillEntity entity);

		BrokerBillEntity GetBrokerBillById (int id);

		IQueryable<BrokerBillEntity> GetBrokerBillsByCondition(BrokerBillSearchCondition condition);

		int GetBrokerBillCount (BrokerBillSearchCondition condition);
	}
}