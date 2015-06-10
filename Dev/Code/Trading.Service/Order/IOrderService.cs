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
        /// ���ɶ�����
        /// ��������ʱ��+�����type��ʶ��+��ˮ�����20λ����string
        /// </summary>
        /// <returns>typeΪʮ��λ���ͱ�ʶ����1Ϊ�Ƽ���2Ϊ���ͣ�3Ϊ�ɽ�</returns>
	    string CreateOrderNumber(int type);
	}
}