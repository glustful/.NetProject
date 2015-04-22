using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.MerchantBill
{
	public interface IMerchantBillService : IDependency
	{
		MerchantBillEntity Create (MerchantBillEntity entity);

		bool Delete(MerchantBillEntity entity);

		MerchantBillEntity Update (MerchantBillEntity entity);

		MerchantBillEntity GetMerchantBillById (int id);

		IQueryable<MerchantBillEntity> GetMerchantBillsByCondition(MerchantBillSearchCondition condition);

		int GetMerchantBillCount (MerchantBillSearchCondition condition);
	}
}