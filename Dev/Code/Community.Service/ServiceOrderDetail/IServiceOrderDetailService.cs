using System.Linq;
using Community.Entity.Model.ServiceOrderDetail;
using YooPoon.Core.Autofac;

namespace Community.Service.ServiceOrderDetail
{
	public interface IServiceOrderDetailService : IDependency
	{
		ServiceOrderDetailEntity Create (ServiceOrderDetailEntity entity);

		bool Delete(ServiceOrderDetailEntity entity);

		ServiceOrderDetailEntity Update (ServiceOrderDetailEntity entity);

		ServiceOrderDetailEntity GetServiceOrderDetailById (int id);

		IQueryable<ServiceOrderDetailEntity> GetServiceOrderDetailsByCondition(ServiceOrderDetailSearchCondition condition);

		int GetServiceOrderDetailCount (ServiceOrderDetailSearchCondition condition);
	}
}