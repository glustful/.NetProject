using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.MerchantClient
{
	public interface IMerchantClientService : IDependency
	{
		MerchantClientEntity Create (MerchantClientEntity entity);

		bool Delete(MerchantClientEntity entity);

		MerchantClientEntity Update (MerchantClientEntity entity);

		MerchantClientEntity GetMerchantClientById (int id);

		IQueryable<MerchantClientEntity> GetMerchantClientsByCondition(MerchantClientSearchCondition condition);

		int GetMerchantClientCount (MerchantClientSearchCondition condition);
	}
}