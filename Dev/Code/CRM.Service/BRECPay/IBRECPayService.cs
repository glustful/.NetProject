using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.BRECPay
{
	public interface IBRECPayService : IDependency
	{
		BRECPayEntity Create (BRECPayEntity entity);

		bool Delete(BRECPayEntity entity);

		BRECPayEntity Update (BRECPayEntity entity);

		BRECPayEntity GetBRECPayById (int id);

		IQueryable<BRECPayEntity> GetBRECPaysByCondition(BRECPaySearchCondition condition);

		int GetBRECPayCount (BRECPaySearchCondition condition);
	}
}