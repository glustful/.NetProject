using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.BrokerWithdrawDetail
{
	public interface IBrokerWithdrawDetailService : IDependency
	{
		BrokerWithdrawDetailEntity Create (BrokerWithdrawDetailEntity entity);

		bool Delete(BrokerWithdrawDetailEntity entity);

		BrokerWithdrawDetailEntity Update (BrokerWithdrawDetailEntity entity);

		BrokerWithdrawDetailEntity GetBrokerWithdrawDetailById (int id);

		IQueryable<BrokerWithdrawDetailEntity> GetBrokerWithdrawDetailsByCondition(BrokerWithdrawDetailSearchCondition condition);

		int GetBrokerWithdrawDetailCount (BrokerWithdrawDetailSearchCondition condition);
	}
}