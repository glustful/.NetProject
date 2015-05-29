using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.Broker
{
	public interface IBrokerService : IDependency
	{
		BrokerEntity Create (BrokerEntity entity);

		bool Delete(BrokerEntity entity);

		BrokerEntity Update (BrokerEntity entity);

		BrokerEntity GetBrokerById (int id);

		IQueryable<BrokerEntity> GetBrokersByCondition(BrokerSearchCondition condition);
     

		int GetBrokerCount (BrokerSearchCondition condition);

        /// <summary>
        /// 经纪人排行
        /// </summary>
        /// <returns></returns>
        IQueryable<BrokerEntity> OrderbyBrokersList();
	}
}