using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.Bank
{
	public interface IBankService : IDependency
	{
		BankEntity Create (BankEntity entity);

		bool Delete(BankEntity entity);

		BankEntity Update (BankEntity entity);

		BankEntity GetBankById (int id);

		IQueryable<BankEntity> GetBanksByCondition(BankSearchCondition condition);

		int GetBankCount (BankSearchCondition condition);
	}
}