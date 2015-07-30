using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.BrokerWithdraw
{
    public class BrokerWithdrawService : IBrokerWithdrawService
    {
        private readonly Zerg.Common.Data.ICRMRepository<BrokerWithdrawEntity> _brokerwithdrawRepository;
        private readonly ILog _log;

        public BrokerWithdrawService(Zerg.Common.Data.ICRMRepository<BrokerWithdrawEntity> brokerwithdrawRepository, ILog log)
        {
            _brokerwithdrawRepository = brokerwithdrawRepository;
            _log = log;
        }

        public BrokerWithdrawEntity Create(BrokerWithdrawEntity entity)
        {
            try
            {
                _brokerwithdrawRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public bool Delete(BrokerWithdrawEntity entity)
        {
            try
            {
                _brokerwithdrawRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return false;
            }
        }

        public BrokerWithdrawEntity Update(BrokerWithdrawEntity entity)
        {
            try
            {
                _brokerwithdrawRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public BrokerWithdrawEntity GetBrokerWithdrawById(int id)
        {
            try
            {
                return _brokerwithdrawRepository.GetById(id); ;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public IQueryable<BrokerWithdrawEntity> GetBrokerWithdrawsByCondition(BrokerWithdrawSearchCondition condition)
        {
            var query = _brokerwithdrawRepository.Table;
            try
            {
                if (condition.WithdrawtimeBegin.HasValue)
                {
                    query = query.Where(q => q.WithdrawTime >= condition.WithdrawtimeBegin.Value);
                }
                if (condition.WithdrawtimeEnd.HasValue)
                {
                    query = query.Where(q => q.WithdrawTime < condition.WithdrawtimeEnd.Value);
                }
                if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime >= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }
                if (condition.UptimeBegin.HasValue)
                {
                    query = query.Where(q => q.Uptime >= condition.UptimeBegin.Value);
                }
                if (condition.UptimeEnd.HasValue)
                {
                    query = query.Where(q => q.Uptime < condition.UptimeEnd.Value);
                }
                if (condition.WithdrawTotalNum.HasValue)
                {
                    query = query.Where(q => q.WithdrawTotalNum == condition.WithdrawTotalNum.Value);
                }
                if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
                if (condition.Brokers != null)
                {
                    query = query.Where(q => (q.Broker.Id == condition.Brokers.Id));
                }
                if (condition.BankCards != null && condition.BankCards.Any())
                {
                    query = query.Where(q => condition.BankCards.Contains(q.BankCard));
                }
                if (condition.Addusers != null && condition.Addusers.Any())
                {
                    query = query.Where(q => condition.Addusers.Contains(q.Adduser));
                }

                if (condition.State!=null)
                {
                    query = query.Where(q => q.State == condition.State);
                }

                if (condition.Upusers != null && condition.Upusers.Any())
                {
                    query = query.Where(q => condition.Upusers.Contains(q.Upuser));
                }
                if (condition.OrderBy.HasValue)
                {
                    switch (condition.OrderBy.Value)
                    {
                        case EnumBrokerWithdrawSearchOrderBy.OrderById:
                            query = condition.isDescending ? query.OrderByDescending(q => q.Id) : query.OrderBy(q => q.Id);
                            break;
                        case EnumBrokerWithdrawSearchOrderBy.State:
                            query = condition.isAescending ? query.OrderBy(q => q.State) : query.OrderByDescending(q => q.State);
                            break;
                    }

                }
                else
                {
                    query = query.OrderBy(q => q.Id);
                }

                if (condition.Page.HasValue && condition.PageCount.HasValue)
                {
                    query = query.Skip((condition.Page.Value - 1) * condition.PageCount.Value).Take(condition.PageCount.Value);
                }
                return query;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public int GetBrokerWithdrawCount(BrokerWithdrawSearchCondition condition)
        {
            var query = _brokerwithdrawRepository.Table;
            try
            {
                if (condition.WithdrawtimeBegin.HasValue)
                {
                    query = query.Where(q => q.WithdrawTime >= condition.WithdrawtimeBegin.Value);
                }
                if (condition.WithdrawtimeEnd.HasValue)
                {
                    query = query.Where(q => q.WithdrawTime < condition.WithdrawtimeEnd.Value);
                }
                if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime >= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }
                if (condition.UptimeBegin.HasValue)
                {
                    query = query.Where(q => q.Uptime >= condition.UptimeBegin.Value);
                }

                if (condition.State!=null)
                {
                query = query.Where(q => q.State == condition.State);
                }
                if (condition.UptimeEnd.HasValue)
                {
                    query = query.Where(q => q.Uptime < condition.UptimeEnd.Value);
                }
                if (condition.WithdrawTotalNum.HasValue)
                {
                    query = query.Where(q => q.WithdrawTotalNum == condition.WithdrawTotalNum.Value);
                }
                if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
                if (condition.Brokers != null)
                {
                    query = query.Where(q => q.Broker.Id==condition.Brokers.Id);
                }
                if (condition.BankCards != null && condition.BankCards.Any())
                {
                    query = query.Where(q => condition.BankCards.Contains(q.BankCard));
                }
                if (condition.Addusers != null && condition.Addusers.Any())
                {
                    query = query.Where(q => condition.Addusers.Contains(q.Adduser));
                }
                if (condition.Upusers != null && condition.Upusers.Any())
                {
                    query = query.Where(q => condition.Upusers.Contains(q.Upuser));
                }
                return query.Count();
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return -1;
            }
        }
    }
}
