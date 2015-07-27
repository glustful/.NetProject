using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.BrokerLeadClient
{
	public class BrokerLeadClientService : IBrokerLeadClientService
	{
		private readonly Zerg.Common.Data.ICRMRepository<BrokerLeadClientEntity> _brokerleadclientRepository;
		private readonly ILog _log;

		public BrokerLeadClientService(Zerg.Common.Data.ICRMRepository<BrokerLeadClientEntity> brokerleadclientRepository,ILog log)
		{
			_brokerleadclientRepository = brokerleadclientRepository;
			_log = log;
		}
		
		public BrokerLeadClientEntity Create (BrokerLeadClientEntity entity)
		{
			try
            {
                _brokerleadclientRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(BrokerLeadClientEntity entity)
		{
			try
            {
                _brokerleadclientRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public BrokerLeadClientEntity Update (BrokerLeadClientEntity entity)
		{
			try
            {
                _brokerleadclientRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public BrokerLeadClientEntity GetBrokerLeadClientById (int id)
		{
			try
            {
                return _brokerleadclientRepository.GetById(id); ;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<BrokerLeadClientEntity> GetBrokerLeadClientsByCondition(BrokerLeadClientSearchCondition condition)
		{
			var query = _brokerleadclientRepository.Table;
			try
			{
				if (condition.AppointmenttimeBegin.HasValue)
                {
                    query = query.Where(q => q.Appointmenttime>= condition.AppointmenttimeBegin.Value);
                }
                if (condition.AppointmenttimeEnd.HasValue)
                {
                    query = query.Where(q => q.Appointmenttime < condition.AppointmenttimeEnd.Value);
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
				if (!string.IsNullOrEmpty(condition.Appointmentstatus))
                {
                    query = query.Where(q => q.Appointmentstatus.Contains(condition.Appointmentstatus));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Brokers != null)
                {
                    query = query.Where(q =>q.Broker.Id== condition.Brokers.Id);
                }
                if (condition.DelFlag != null)
                {
                    query = query.Where(q => q.DelFlag== condition.DelFlag);
                }
                if (condition.Projectids != null && condition.Projectids.Any())
                {
                    query = query.Where(q => condition.Projectids.Contains(q.ProjectId));
                }
				if (condition.ClientInfos != null && condition.ClientInfos.Any())
                {
                    query = query.Where(q => condition.ClientInfos.Contains(q.ClientInfo));
                }
                if (condition.ClientName != null && condition.ClientName.Any())
                {
                    query = query.Where(q => condition.ClientName.Contains(q.ClientName));
                }
                if (condition.Phone != null && condition.Phone.Any())
                {
                    query = query.Where(q => condition.Phone.Contains(q.Phone));
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
						case EnumBrokerLeadClientSearchOrderBy.OrderById:
							query = condition.isDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
                        case EnumBrokerLeadClientSearchOrderBy.OrderByTime:
                            query = condition.isDescending ? query.OrderByDescending(q => q.Uptime) : query.OrderBy(q => q.Uptime);
                            break;
                    }
					
				}
				else
				{
					query = query.OrderBy(q=>q.Id);
				}

                if (condition.Status.HasValue)
                {
                    query = query.Where(c => c.Status == condition.Status);
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

		public int GetBrokerLeadClientCount (BrokerLeadClientSearchCondition condition)
		{
			var query = _brokerleadclientRepository.Table;
			try
			{
				if (condition.AppointmenttimeBegin.HasValue)
                {
                    query = query.Where(q => q.Appointmenttime>= condition.AppointmenttimeBegin.Value);
                }
                if (condition.AppointmenttimeEnd.HasValue)
                {
                    query = query.Where(q => q.Appointmenttime < condition.AppointmenttimeEnd.Value);
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
				if (!string.IsNullOrEmpty(condition.Appointmentstatus))
                {
                    query = query.Where(q => q.Appointmentstatus.Contains(condition.Appointmentstatus));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Brokers != null)
                {
                    query = query.Where(q =>q.Broker.Id== condition.Brokers.Id);
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
                if (condition.Status.HasValue)
                {
                    query = query.Where(c => c.Status == condition.Status);
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