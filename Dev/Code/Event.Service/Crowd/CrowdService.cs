using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using Event.Entity.Model;
using Zerg.Common.Data;

namespace Event.Service.Crowd
{
	public class CrowdService : ICrowdService
	{
		private readonly IEventRepository<CrowdEntity> _crowdRepository;
		private readonly ILog _log;

		public CrowdService(IEventRepository<CrowdEntity> crowdRepository,ILog log)
		{
			_crowdRepository = crowdRepository;
			_log = log;
		}
		
		public CrowdEntity Create (CrowdEntity entity)
		{
			try
            {
                _crowdRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(CrowdEntity entity)
		{
			try
            {
                _crowdRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public CrowdEntity Update (CrowdEntity entity)
		{
			try
            {
                _crowdRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public CrowdEntity GetCrowdById (int id)
		{
			try
            {
                return _crowdRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<CrowdEntity> GetCrowdsByCondition(CrowdSearchCondition condition)
		{
			var query = _crowdRepository.Table;
			try
			{
				if (condition.StarttimeBegin.HasValue)
                {
                    query = query.Where(q => q.Starttime>= condition.StarttimeBegin.Value);
                }
                if (condition.StarttimeEnd.HasValue)
                {
                    query = query.Where(q => q.Starttime < condition.StarttimeEnd.Value);
                }
				if (condition.EndtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Endtime>= condition.EndtimeBegin.Value);
                }
                if (condition.EndtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Endtime < condition.EndtimeEnd.Value);
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
				if (!string.IsNullOrEmpty(condition.Ttitle))
                {
                    query = query.Where(q => q.Ttitle.Contains(condition.Ttitle));
                }
				if (!string.IsNullOrEmpty(condition.Intro))
                {
                    query = query.Where(q => q.Intro.Contains(condition.Intro));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Statuss>=0)
                {
                    query = query.Where(q => condition.Statuss==q.Status);
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
						case EnumCrowdSearchOrderBy.OrderById:
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

		public int GetCrowdCount (CrowdSearchCondition condition)
		{
			var query = _crowdRepository.Table;
			try
			{
				if (condition.StarttimeBegin.HasValue)
                {
                    query = query.Where(q => q.Starttime>= condition.StarttimeBegin.Value);
                }
                if (condition.StarttimeEnd.HasValue)
                {
                    query = query.Where(q => q.Starttime < condition.StarttimeEnd.Value);
                }
				if (condition.EndtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Endtime>= condition.EndtimeBegin.Value);
                }
                if (condition.EndtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Endtime < condition.EndtimeEnd.Value);
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
				if (!string.IsNullOrEmpty(condition.Ttitle))
                {
                    query = query.Where(q => q.Ttitle.Contains(condition.Ttitle));
                }
				if (!string.IsNullOrEmpty(condition.Intro))
                {
                    query = query.Where(q => q.Intro.Contains(condition.Intro));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Statuss>=0)
                {
                    query = query.Where(q => condition.Statuss==q.Status);
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