using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.BrokerRECClient
{
	public interface IBrokerRECClientService : IDependency
	{
		BrokerRECClientEntity Create (BrokerRECClientEntity entity);

		bool Delete(BrokerRECClientEntity entity);

		BrokerRECClientEntity Update (BrokerRECClientEntity entity);

		BrokerRECClientEntity GetBrokerRECClientById (int id);

		IQueryable<BrokerRECClientEntity> GetBrokerRECClientsByCondition(BrokerRECClientSearchCondition condition);

		int GetBrokerRECClientCount (BrokerRECClientSearchCondition condition);
	}
}