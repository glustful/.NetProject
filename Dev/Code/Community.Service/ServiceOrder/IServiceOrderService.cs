using System.Linq;
using Community.Entity.Model;
using Community.Entity.Model.ServiceOrder;
using YooPoon.Core.Autofac;

namespace Community.Service.ServiceOrder
{
	public interface IServiceOrderService : IDependency
	{
		ServiceOrderEntity Create (ServiceOrderEntity entity);

		bool Delete(ServiceOrderEntity entity);

		ServiceOrderEntity Update (ServiceOrderEntity entity);

		ServiceOrderEntity GetServiceOrderById (int id);

		IQueryable<ServiceOrderEntity> GetServiceOrdersByCondition(ServiceOrderSearchCondition condition);

		int GetServiceOrderCount (ServiceOrderSearchCondition condition);
	}
}