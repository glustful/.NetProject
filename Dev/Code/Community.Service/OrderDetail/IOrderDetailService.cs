using System.Linq;
using Community.Entity.Model.OrderDetail;
using YooPoon.Core.Autofac;

namespace Community.Service.OrderDetail
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