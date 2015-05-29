using System.Linq;
using Trading.Entity.Model;
using YooPoon.Core.Autofac;

namespace Trading.Service.CFBBill
{
	public interface ICFBBillService : IDependency
	{
		CFBBillEntity Create (CFBBillEntity entity);

		bool Delete(CFBBillEntity entity);

		CFBBillEntity Update (CFBBillEntity entity);

		CFBBillEntity GetCFBBillById (int id);

		IQueryable<CFBBillEntity> GetCFBBillsByCondition(CFBBillSearchCondition condition);

		int GetCFBBillCount (CFBBillSearchCondition condition);
	}
}