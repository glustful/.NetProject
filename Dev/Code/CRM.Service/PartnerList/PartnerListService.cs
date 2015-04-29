using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.PartnerList
{
	public class PartnerListService : IPartnerListService
	{
		private readonly Zerg.Common.Data.ICRMRepository<PartnerListEntity> _partnerlistRepository;
		private readonly ILog _log;

		public PartnerListService(Zerg.Common.Data.ICRMRepository<PartnerListEntity> partnerlistRepository,ILog log)
		{
			_partnerlistRepository = partnerlistRepository;
			_log = log;
		}
		
		public PartnerListEntity Create (PartnerListEntity entity)
		{
			try
            {
                _partnerlistRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(PartnerListEntity entity)
		{
			try
            {
                _partnerlistRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public PartnerListEntity Update (PartnerListEntity entity)
		{
			try
            {
                _partnerlistRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public PartnerListEntity GetPartnerListById (int id)
		{
			try
            {
                return _partnerlistRepository.GetById(id); ;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<PartnerListEntity> GetPartnerListsByCondition(PartnerListSearchCondition condition)
		{
			var query = _partnerlistRepository.Table;
			try
			{
				if (condition.RegtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Regtime>= condition.RegtimeBegin.Value);
                }
                if (condition.RegtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Regtime < condition.RegtimeEnd.Value);
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
				if (condition.Phone.HasValue)
                {
                    query = query.Where(q => q.Phone == condition.Phone.Value);
                }
				if (!string.IsNullOrEmpty(condition.Brokername))
                {
                    query = query.Where(q => q.Brokername.Contains(condition.Brokername));
                }
				if (!string.IsNullOrEmpty(condition.Agentlevel))
                {
                    query = query.Where(q => q.Agentlevel.Contains(condition.Agentlevel));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Brokers != null && condition.Brokers.Any())
                {
                    query = query.Where(q => condition.Brokers.Contains(q.Broker));
                }
				if (condition.PartnerIds != null && condition.PartnerIds.Any())
                {
                    query = query.Where(q => condition.PartnerIds.Contains(q.PartnerId));
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
						case EnumPartnerListSearchOrderBy.OrderById:
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

		public int GetPartnerListCount (PartnerListSearchCondition condition)
		{
			var query = _partnerlistRepository.Table;
			try
			{
				if (condition.RegtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Regtime>= condition.RegtimeBegin.Value);
                }
                if (condition.RegtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Regtime < condition.RegtimeEnd.Value);
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
				if (condition.Phone.HasValue)
                {
                    query = query.Where(q => q.Phone == condition.Phone.Value);
                }
				if (!string.IsNullOrEmpty(condition.Brokername))
                {
                    query = query.Where(q => q.Brokername.Contains(condition.Brokername));
                }
				if (!string.IsNullOrEmpty(condition.Agentlevel))
                {
                    query = query.Where(q => q.Agentlevel.Contains(condition.Agentlevel));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Brokers != null && condition.Brokers.Any())
                {
                    query = query.Where(q => condition.Brokers.Contains(q.Broker));
                }
				if (condition.PartnerIds != null && condition.PartnerIds.Any())
                {
                    query = query.Where(q => condition.PartnerIds.Contains(q.PartnerId));
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