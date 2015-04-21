using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.BrokerRECClient
{
	public class BrokerRECClientService : IBrokerRECClientService
	{
		private readonly IRepository<BrokerRECClientEntity> _brokerrecclientRepository;
		private readonly ILog _log;

		public BrokerRECClientService(IRepository<BrokerRECClientEntity> brokerrecclientRepository,ILog log)
		{
			_brokerrecclientRepository = brokerrecclientRepository;
			_log = log;
		}
		
		public BrokerRECClientEntity Create (BrokerRECClientEntity entity)
		{
			try
            {
                _brokerrecclientRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(BrokerRECClientEntity entity)
		{
			try
            {
                _brokerrecclientRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public BrokerRECClientEntity Update (BrokerRECClientEntity entity)
		{
			try
            {
                _brokerrecclientRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public BrokerRECClientEntity GetBrokerRECClientById (int id)
		{
			try
            {
                return _brokerrecclientRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<BrokerRECClientEntity> GetBrokerRECClientsByCondition(BrokerRECClientSearchCondition condition)
		{
			var query = _brokerrecclientRepository.Table;
			try
			{
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
				if (condition.Phone.HasValue)
                {
                    query = query.Where(q => q.Phone == condition.Phone.Value);
                }
				if (condition.Qq.HasValue)
                {
                    query = query.Where(q => q.Qq == condition.Qq.Value);
                }
				if (!string.IsNullOrEmpty(condition.Clientname))
                {
                    query = query.Where(q => q.Clientname.Contains(condition.Clientname));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Brokers != null && condition.Brokers.Any())
                {
                    query = query.Where(q => condition.Brokers.Contains(q.Broker));
                }
				if (condition.ClientInfos != null && condition.ClientInfos.Any())
                {
                    query = query.Where(q => condition.ClientInfos.Contains(q.ClientInfo));
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
						case EnumBrokerRECClientSearchOrderBy.OrderById:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
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

		public int GetBrokerRECClientCount (BrokerRECClientSearchCondition condition)
		{
			var query = _brokerrecclientRepository.Table;
			try
			{
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
				if (condition.Phone.HasValue)
                {
                    query = query.Where(q => q.Phone == condition.Phone.Value);
                }
				if (condition.Qq.HasValue)
                {
                    query = query.Where(q => q.Qq == condition.Qq.Value);
                }
				if (!string.IsNullOrEmpty(condition.Clientname))
                {
                    query = query.Where(q => q.Clientname.Contains(condition.Clientname));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Brokers != null && condition.Brokers.Any())
                {
                    query = query.Where(q => condition.Brokers.Contains(q.Broker));
                }
				if (condition.ClientInfos != null && condition.ClientInfos.Any())
                {
                    query = query.Where(q => condition.ClientInfos.Contains(q.ClientInfo));
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