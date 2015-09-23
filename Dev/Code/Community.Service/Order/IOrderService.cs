using System.Linq;
using Community.Entity.Model.Order;
using YooPoon.Core.Autofac;

namespace Community.Service.Order
{
	public interface IOrderService : IDependency
	{
		OrderEntity Create (OrderEntity entity);

		bool Delete(OrderEntity entity);

		OrderEntity Update (OrderEntity entity);

		OrderEntity GetOrderById (int id);

		IQueryable<OrderEntity> GetOrdersByCondition(OrderSearchCondition condition);

		int GetOrderCount (OrderSearchCondition condition);

	    OrderEntity GetOrderByNo(string orderNo);
	}
}