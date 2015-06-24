using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.BrokerWithdrawDetail
{
	public class BrokerWithdrawDetailService : IBrokerWithdrawDetailService
	{
		private readonly Zerg.Common.Data.ICRMRepository<BrokerWithdrawDetailEntity> _brokerwithdrawdetailRepository;
		private readonly ILog _log;

		public BrokerWithdrawDetailService(Zerg.Common.Data.ICRMRepository<BrokerWithdrawDetailEntity> brokerwithdrawdetailRepository,ILog log)
		{
			_brokerwithdrawdetailRepository = brokerwithdrawdetailRepository;
			_log = log;
		}
		
		public BrokerWithdrawDetailEntity Create (BrokerWithdrawDetailEntity entity)
		{
			try
            {
                _brokerwithdrawdetailRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(BrokerWithdrawDetailEntity entity)
		{
			try
            {
                _brokerwithdrawdetailRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public BrokerWithdrawDetailEntity Update (BrokerWithdrawDetailEntity entity)
		{
			try
            {
                _brokerwithdrawdetailRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public BrokerWithdrawDetailEntity GetBrokerWithdrawDetailById (int id)
		{
			try
            {
                return _brokerwithdrawdetailRepository.GetById(id); ;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<BrokerWithdrawDetailEntity> GetBrokerWithdrawDetailsByCondition(BrokerWithdrawDetailSearchCondition condition)
		{
			var query = _brokerwithdrawdetailRepository.Table;
			try
			{
				if (condition.WithdrawtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Withdrawtime>= condition.WithdrawtimeBegin.Value);
                }
                if (condition.WithdrawtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Withdrawtime < condition.WithdrawtimeEnd.Value);
                }
				if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime>= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }
				if (condition.UptimeBegin.HasValue)
                {
                    query = query.Where(q => q.Uptime>= condition.UptimeBegin.Value);
                }
                if (condition.UptimeEnd.HasValue)
                {
                    query = query.Where(q => q.Uptime < condition.UptimeEnd.Value);
                }
				if (condition.Withdrawnum.HasValue)
                {
                    query = query.Where(q => q.Withdrawnum == condition.Withdrawnum.Value);
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Brokers != null )
                {
                    query = query.Where(q => (q.Broker.Id==condition.Brokers.Id));
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
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumBrokerWithdrawDetailSearchOrderBy.OrderById:
							query = condition.isDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
                    }
					
				}
				else
				{
					query = query.OrderBy(q=>q.Id);
				}

				if (condition.Page.HasValue && condition.PageCount.HasValue)
                {
                    query = query.Skip((condition.Page.Value - 1)*condition.PageCount.Value).Take(condition.PageCount.Value);
                }
				return query;
			}
			catch(Exception e)
			{
				_log.Error(e,"数据库操作出错");
                return null;
			}
		}

		public int GetBrokerWithdrawDetailCount (BrokerWithdrawDetailSearchCondition condition)
		{
			var query = _brokerwithdrawdetailRepository.Table;
			try
			{
				if (condition.WithdrawtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Withdrawtime>= condition.WithdrawtimeBegin.Value);
                }
                if (condition.WithdrawtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Withdrawtime < condition.WithdrawtimeEnd.Value);
                }
				if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime>= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }
				if (condition.UptimeBegin.HasValue)
                {
                    query = query.Where(q => q.Uptime>= condition.UptimeBegin.Value);
                }
                if (condition.UptimeEnd.HasValue)
                {
                    query = query.Where(q => q.Uptime < condition.UptimeEnd.Value);
                }
				if (condition.Withdrawnum.HasValue)
                {
                    query = query.Where(q => q.Withdrawnum == condition.Withdrawnum.Value);
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Brokers != null )
                {
                    query = query.Where(q => condition.Brokers.Id ==(q.Broker.Id));
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
			catch(Exception e)
			{
				_log.Error(e,"数据库操作出错");
                return -1;
			}
		}
	}
}