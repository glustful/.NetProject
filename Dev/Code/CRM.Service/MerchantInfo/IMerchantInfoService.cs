using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.MerchantInfo
{
	public interface IMerchantInfoService : IDependency
	{
		MerchantInfoEntity Create (MerchantInfoEntity entity);

		bool Delete(MerchantInfoEntity entity);

		MerchantInfoEntity Update (MerchantInfoEntity entity);

		MerchantInfoEntity GetMerchantInfoById (int id);

		IQueryable<MerchantInfoEntity> GetMerchantInfosByCondition(MerchantInfoSearchCondition condition);

		int GetMerchantInfoCount (MerchantInfoSearchCondition condition);
	}
}