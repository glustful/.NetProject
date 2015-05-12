using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.BrokerOrder
{
	public class BrokerOrderService : IBrokerOrderService
	{
		private readonly Zerg.Common.Data.ICRMRepository<BrokerOrderEntity> _brokerorderRepository;
		private readonly ILog _log;

		public BrokerOrderService(Zerg.Common.Data.ICRMRepository<BrokerOrderEntity> brokerorderRepository,ILog log)
		{
			_brokerorderRepository = brokerorderRepository;
			_log = log;
		}
		
		public BrokerOrderEntity Create (BrokerOrderEntity entity)
		{
			try
            {
                _brokerorderRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(BrokerOrderEntity entity)
		{
			try
            {
                _brokerorderRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public BrokerOrderEntity Update (BrokerOrderEntity entity)
		{
			try
            {
                _brokerorderRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public BrokerOrderEntity GetBrokerOrderById (int id)
		{
			try
            {
                return _brokerorderRepository.GetById(id); ;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<BrokerOrderEntity> GetBrokerOrdersByCondition(BrokerOrderSearchCondition condition)
		{
			var query = _brokerorderRepository.Table;
			try
			{
				if (condition.OrdertimeBegin.HasValue)
                {
                    query = query.Where(q => q.Ordertime>= condition.OrdertimeBegin.Value);
                }
                if (condition.OrdertimeEnd.HasValue)
                {
                    query = query.Where(q => q.Ordertime < condition.OrdertimeEnd.Value);
                }
				if (condition.ModifytimeBegin.HasValue)
                {
                    query = query.Where(q => q.Modifytime>= condition.ModifytimeBegin.Value);
                }
                if (condition.ModifytimeEnd.HasValue)
                {
                    query = query.Where(q => q.Modifytime < condition.ModifytimeEnd.Value);
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
				if (condition.Status.HasValue)
                {
                    query = query.Where(q => q.Status == condition.Status.Value);
                }
				if (!string.IsNullOrEmpty(condition.Merchantname))
                {
                    query = query.Where(q => q.Merchantname.Contains(condition.Merchantname));
                }
				if (!string.IsNullOrEmpty(condition.Orderuser))
                {
                    query = query.Where(q => q.Orderuser.Contains(condition.Orderuser));
                }
				if (!string.IsNullOrEmpty(condition.Modifyuser))
                {
                    query = query.Where(q => q.Modifyuser.Contains(condition.Modifyuser));
                }
				if (!string.IsNullOrEmpty(condition.Customername))
                {
                    query = query.Where(q => q.Customername.Contains(condition.Customername));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Brokers != null && condition.Brokers.Any())
                {
                    query = query.Where(q => condition.Brokers.Contains(q.Broker));
                }
				if (condition.OrderIds != null && condition.OrderIds.Any())
                {
                    query = query.Where(q => condition.OrderIds.Contains(q.OrderId));
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
						case EnumBrokerOrderSearchOrderBy.OrderById:
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

		public int GetBrokerOrderCount (BrokerOrderSearchCondition condition)
		{
			var query = _brokerorderRepository.Table;
			try
			{
				if (condition.OrdertimeBegin.HasValue)
                {
                    query = query.Where(q => q.Ordertime>= condition.OrdertimeBegin.Value);
                }
                if (condition.OrdertimeEnd.HasValue)
                {
                    query = query.Where(q => q.Ordertime < condition.OrdertimeEnd.Value);
                }
				if (condition.ModifytimeBegin.HasValue)
                {
                    query = query.Where(q => q.Modifytime>= condition.ModifytimeBegin.Value);
                }
                if (condition.ModifytimeEnd.HasValue)
                {
                    query = query.Where(q => q.Modifytime < condition.ModifytimeEnd.Value);
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
				if (condition.Status.HasValue)
                {
                    query = query.Where(q => q.Status == condition.Status.Value);
                }
				if (!string.IsNullOrEmpty(condition.Merchantname))
                {
                    query = query.Where(q => q.Merchantname.Contains(condition.Merchantname));
                }
				if (!string.IsNullOrEmpty(condition.Orderuser))
                {
                    query = query.Where(q => q.Orderuser.Contains(condition.Orderuser));
                }
				if (!string.IsNullOrEmpty(condition.Modifyuser))
                {
                    query = query.Where(q => q.Modifyuser.Contains(condition.Modifyuser));
                }
				if (!string.IsNullOrEmpty(condition.Customername))
                {
                    query = query.Where(q => q.Customername.Contains(condition.Customername));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Brokers != null && condition.Brokers.Any())
                {
                    query = query.Where(q => condition.Brokers.Contains(q.Broker));
                }
				if (condition.OrderIds != null && condition.OrderIds.Any())
                {
                    query = query.Where(q => condition.OrderIds.Contains(q.OrderId));
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