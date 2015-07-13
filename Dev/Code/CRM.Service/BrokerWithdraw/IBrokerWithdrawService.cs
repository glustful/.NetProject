using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;


namespace CRM.Service.BrokerWithdraw
{
    public interface IBrokerWithdrawService : IDependency
    {
        BrokerWithdrawEntity Create(BrokerWithdrawEntity entity);

        bool Delete(BrokerWithdrawEntity entity);

        BrokerWithdrawEntity Update(BrokerWithdrawEntity entity);

        BrokerWithdrawEntity GetBrokerWithdrawById(int id);

        IQueryable<BrokerWithdrawEntity> GetBrokerWithdrawsByCondition(BrokerWithdrawSearchCondition condition);

        int GetBrokerWithdrawCount(BrokerWithdrawSearchCondition condition);
    }
}
