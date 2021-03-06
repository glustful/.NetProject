using System.Linq;
using Trading.Entity.Model;
using YooPoon.Core.Autofac;

namespace Trading.Service.Order
{
	public interface IOrderService : IDependency
	{
		OrderEntity Create (OrderEntity entity);

		bool Delete(OrderEntity entity);

		OrderEntity Update (OrderEntity entity);

		OrderEntity GetOrderById (int id);

		IQueryable<OrderEntity> GetOrdersByCondition(OrderSearchCondition condition);

		int GetOrderCount (OrderSearchCondition condition);

        IQueryable<OrderEntity> GetOrdersByAgent(int agentId);

        IQueryable<OrderEntity> GetOrdersByBus(int busId);

        /// <summary>
        /// 生成订单号
        /// 订单号由时间+传入的type标识符+流水号组成25位定长string
        /// </summary>
        /// <param name="type">type为右起10，11位类型标识符，1为推荐，2为带客，3为成交</param>
        /// <returns></returns>
	    string CreateOrderNumber(int type);
	}
}