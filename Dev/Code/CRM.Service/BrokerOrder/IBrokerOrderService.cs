using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.BrokerOrder
{
	public interface IBrokerOrderService : IDependency
	{
		BrokerOrderEntity Create (BrokerOrderEntity entity);

		bool Delete(BrokerOrderEntity entity);

		BrokerOrderEntity Update (BrokerOrderEntity entity);

		BrokerOrderEntity GetBrokerOrderById (int id);

		IQueryable<BrokerOrderEntity> GetBrokerOrdersByCondition(BrokerOrderSearchCondition condition);

		int GetBrokerOrderCount (BrokerOrderSearchCondition condition);
	}
}