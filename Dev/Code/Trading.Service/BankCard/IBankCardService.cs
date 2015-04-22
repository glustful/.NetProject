using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.BankCard
{
	public interface IBankCardService : IDependency
	{
		BankCardEntity Create (BankCardEntity entity);

		bool Delete(BankCardEntity entity);

		BankCardEntity Update (BankCardEntity entity);

		BankCardEntity GetBankCardById (int id);

		IQueryable<BankCardEntity> GetBankCardsByCondition(BankCardSearchCondition condition);

		int GetBankCardCount (BankCardSearchCondition condition);
	}
}