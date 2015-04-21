using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.BrokeAccount
{
	public interface IBrokeAccountService : IDependency
	{
		BrokeAccountEntity Create (BrokeAccountEntity entity);

		bool Delete(BrokeAccountEntity entity);

		BrokeAccountEntity Update (BrokeAccountEntity entity);

		BrokeAccountEntity GetBrokeAccountById (int id);

		IQueryable<BrokeAccountEntity> GetBrokeAccountsByCondition(BrokeAccountSearchCondition condition);

		int GetBrokeAccountCount (BrokeAccountSearchCondition condition);
	}
}