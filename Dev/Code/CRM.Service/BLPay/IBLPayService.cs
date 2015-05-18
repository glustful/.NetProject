using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.BLPay
{
	public interface IBLPayService : IDependency
	{
		BLPayEntity Create (BLPayEntity entity);

		bool Delete(BLPayEntity entity);

		BLPayEntity Update (BLPayEntity entity);

		BLPayEntity GetBLPayById (int id);

		IQueryable<BLPayEntity> GetBLPaysByCondition(BLPaySearchCondition condition);

		int GetBLPayCount (BLPaySearchCondition condition);
	}
}