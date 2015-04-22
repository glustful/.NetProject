using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.BrokerLeadClient
{
	public interface IBrokerLeadClientService : IDependency
	{
		BrokerLeadClientEntity Create (BrokerLeadClientEntity entity);

		bool Delete(BrokerLeadClientEntity entity);

		BrokerLeadClientEntity Update (BrokerLeadClientEntity entity);

		BrokerLeadClientEntity GetBrokerLeadClientById (int id);

		IQueryable<BrokerLeadClientEntity> GetBrokerLeadClientsByCondition(BrokerLeadClientSearchCondition condition);

		int GetBrokerLeadClientCount (BrokerLeadClientSearchCondition condition);
	}
}