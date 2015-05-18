using System.Linq;
using Trading.Entity.Model;
using YooPoon.Core.Autofac;

namespace Trading.Service.OrderDetail
{
	public interface IOrderDetailService : IDependency
	{
		OrderDetailEntity Create (OrderDetailEntity entity);

		bool Delete(OrderDetailEntity entity);

		OrderDetailEntity Update (OrderDetailEntity entity);

		OrderDetailEntity GetOrderDetailById (int id);

		IQueryable<OrderDetailEntity> GetOrderDetailsByCondition(OrderDetailSearchCondition condition);

		int GetOrderDetailCount (OrderDetailSearchCondition condition);
	}
}