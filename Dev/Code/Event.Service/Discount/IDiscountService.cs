using System.Collections.Generic;
using System.Linq;
using Event.Entity.Model;
using YooPoon.Core.Autofac;

namespace Event.Service.Discount
{
	public interface IDiscountService : IDependency
	{
		DiscountEntity Create (DiscountEntity entity);

		bool Delete(DiscountEntity entity);

		DiscountEntity Update (DiscountEntity entity);

		DiscountEntity GetDiscountById (int id);

		IQueryable<DiscountEntity> GetDiscountsByCondition(DiscountSearchCondition condition);

		int GetDiscountCount (DiscountSearchCondition condition);

	    List<DiscountEntity> GetDiscountByCrowdId(int crowdId);
       int GetDiscountMaxCountByCrowdId(int id);
	}
}