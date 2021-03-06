using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.BrokerRECClient
{
	public class BrokerRECClientService : IBrokerRECClientService
	{
		private readonly Zerg.Common.Data.ICRMRepository<BrokerRECClientEntity> _brokerrecclientRepository;
		private readonly ILog _log;

		public BrokerRECClientService(Zerg.Common.Data.ICRMRepository<BrokerRECClientEntity> brokerrecclientRepository,ILog log)
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
                if (!string.IsNullOrEmpty(condition.Phone))
                {
                    query = query.Where(q => q.Phone.Contains(condition.Phone));
                }
                if (!string.IsNullOrEmpty(condition.Qq))
                {
                    query = query.Where(q => q.Qq.Contains(condition.Qq));
                }
				if (!string.IsNullOrEmpty(condition.Clientname))
                {
                    query = query.Where(q => q.Clientname.Contains(condition.Clientname));
                }
				if (!string.IsNullOrEmpty(condition.Brokername))
                {
                    query = query.Where(q => q.Brokername.Contains(condition.Brokername));
                }
				if (!string.IsNullOrEmpty(condition.Brokerlevel))
                {
                    query = query.Where(q => q.Brokerlevel.Contains(condition.Brokerlevel));
                }
				if (!string.IsNullOrEmpty(condition.Projectname))
                {
                    query = query.Where(q => q.Projectname.Contains(condition.Projectname));
                }
				if (!string.IsNullOrEmpty(condition.SecretaryPhone))
                {
                    query = query.Where(q => q.SecretaryPhone.Contains(condition.SecretaryPhone));
                }
				if (!string.IsNullOrEmpty(condition.WriterPhone))
                {
                    query = query.Where(q => q.WriterPhone.Contains(condition.WriterPhone));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Brokers != null )
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
				if (condition.Projectids != null && condition.Projectids.Any())
                {
                    query = query.Where(q => condition.Projectids.Contains(q.Projectid));
                }
				if (condition.SecretaryIDs != null && condition.SecretaryIDs.Any())
                {
                    query = query.Where(q => condition.SecretaryIDs.Contains(q.SecretaryId));
                }
                if (condition.Id >0)
                {
                    query = query.Where(q => condition.Id ==q.Id);
                }
				if (condition.WriterIDs != null && condition.WriterIDs.Any())
                {
                    query = query.Where(q => condition.WriterIDs.Contains(q.WriterId));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumBrokerRECClientSearchOrderBy.OrderById:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.Id) : query.OrderBy(q => q.Id);
							break;
                        case EnumBrokerRECClientSearchOrderBy.OrderByTime:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.Uptime) : query.OrderBy(q => q.Uptime);
                            break;
                        case EnumBrokerRECClientSearchOrderBy.OrderByBrokername :
                            query = condition.IsDescending ? query.OrderByDescending(q => q.Brokername) : query.OrderBy(q => q.Brokername);
                            break;
                        case EnumBrokerRECClientSearchOrderBy.OrderByBrokerlevel:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.Brokerlevel) : query.OrderBy(q => q.Brokerlevel);
                            break;
                        case EnumBrokerRECClientSearchOrderBy.OrderByClientname:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.Clientname) : query.OrderBy(q => q.Clientname);
                            break;
                        case EnumBrokerRECClientSearchOrderBy.OrderByPhone:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.Phone) : query.OrderBy(q => q.Phone);
                            break;
                        case EnumBrokerRECClientSearchOrderBy.OrderByProjectname:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.Projectname) : query.OrderBy(q => q.Projectname);
                            break;
                        case EnumBrokerRECClientSearchOrderBy.OrderByAddtime:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.Addtime) : query.OrderBy(q => q.Addtime);
                            break;
                        case EnumBrokerRECClientSearchOrderBy.OrderBySecretaryName:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.SecretaryId.Brokername) : query.OrderBy(q => q.SecretaryId.Brokername);
                            break;
                        case EnumBrokerRECClientSearchOrderBy.OrderByWaiter:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.WriterId.Brokername) : query.OrderBy(q => q.WriterId.Brokername);
                            break;
                        case EnumBrokerRECClientSearchOrderBy.OrderByUptime:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.Uptime) : query.OrderBy(q => q.Uptime);
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

                if (condition.DelFlag.HasValue)
                {
                    query = query.Where(c => c.DelFlag == condition.DelFlag);
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
                if (!string.IsNullOrEmpty(condition.Qq))
                {
                    query = query.Where(q => q.Qq.Contains(condition.Qq));
                }
				if (!string.IsNullOrEmpty(condition.Clientname))
                {
                    query = query.Where(q => q.Clientname.Contains(condition.Clientname));
                }
                if (!string.IsNullOrEmpty(condition.Phone))
                {
                    query = query.Where(q => q.Phone.Contains(condition.Phone));
                }
				if (!string.IsNullOrEmpty(condition.Brokername))
                {
                    query = query.Where(q => q.Brokername.Contains(condition.Brokername));
                }
				if (!string.IsNullOrEmpty(condition.Brokerlevel))
                {
                    query = query.Where(q => q.Brokerlevel.Contains(condition.Brokerlevel));
                }
				if (!string.IsNullOrEmpty(condition.Projectname))
                {
                    query = query.Where(q => q.Projectname.Contains(condition.Projectname));
                }
				if (!string.IsNullOrEmpty(condition.SecretaryPhone))
                {
                    query = query.Where(q => q.SecretaryPhone.Contains(condition.SecretaryPhone));
                }
				if (!string.IsNullOrEmpty(condition.WriterPhone))
                {
                    query = query.Where(q => q.WriterPhone.Contains(condition.WriterPhone));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Brokers != null )
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
				if (condition.Projectids != null && condition.Projectids.Any())
                {
                    query = query.Where(q => condition.Projectids.Contains(q.Projectid));
                }
				if (condition.SecretaryIDs != null && condition.SecretaryIDs.Any())
                {
                    query = query.Where(q => condition.SecretaryIDs.Contains(q.SecretaryId));
                }
				if (condition.WriterIDs != null && condition.WriterIDs.Any())
                {
                    query = query.Where(q => condition.WriterIDs.Contains(q.WriterId));
                }
                if (condition.DelFlag != null)
                {
                    query = query.Where(q => q.DelFlag == condition.DelFlag);
                }
                if (condition.Status != null)
                {
                    query = query.Where(q => q.Status == condition.Status);
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