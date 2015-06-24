using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.MerchantOrder
{
	public interface IMerchantOrderService : IDependency
	{
		MerchantOrderEntity Create (MerchantOrderEntity entity);

		bool Delete(MerchantOrderEntity entity);

		MerchantOrderEntity Update (MerchantOrderEntity entity);

		MerchantOrderEntity GetMerchantOrderById (int id);

		IQueryable<MerchantOrderEntity> GetMerchantOrdersByCondition(MerchantOrderSearchCondition condition);

		int GetMerchantOrderCount (MerchantOrderSearchCondition condition);
	}
}