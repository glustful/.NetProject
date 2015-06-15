using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using Event.Entity.Model;

namespace Event.Service.RedConfig
{
	public class RedConfigService : IRedConfigService
	{
		private readonly IRepository<RedConfigEntity> _redconfigRepository;
		private readonly ILog _log;

		public RedConfigService(IRepository<RedConfigEntity> redconfigRepository,ILog log)
		{
			_redconfigRepository = redconfigRepository;
			_log = log;
		}
		
		public RedConfigEntity Create (RedConfigEntity entity)
		{
			try
            {
                _redconfigRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(RedConfigEntity entity)
		{
			try
            {
                _redconfigRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public RedConfigEntity Update (RedConfigEntity entity)
		{
			try
            {
                _redconfigRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public RedConfigEntity GetRedConfigById (int id)
		{
			try
            {
                return _redconfigRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<RedConfigEntity> GetRedConfigsByCondition(RedConfigSearchCondition condition)
		{
			var query = _redconfigRepository.Table;
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
				if (condition.StatusBegin.HasValue)
                {
                    query = query.Where(q => q.Status>= condition.StatusBegin.Value);
                }
                if (condition.StatusEnd.HasValue)
                {
                    query = query.Where(q => q.Status < condition.StatusEnd.Value);
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
				if (condition.商家关联ids != null && condition.商家关联ids.Any())
                {
                    query = query.Where(q => condition.商家关联ids.Contains(q.商家关联id));
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
						case EnumRedConfigSearchOrderBy.OrderByStarttime:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Starttime):query.OrderBy(q=>q.Starttime);
							break;
						case EnumRedConfigSearchOrderBy.OrderByEndtime:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Endtime):query.OrderBy(q=>q.Endtime);
							break;
						case EnumRedConfigSearchOrderBy.OrderByStatus:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Status):query.OrderBy(q=>q.Status);
							break;
                    }
					
				}
				else
				{
					query = query.OrderBy(q=>q.Starttime);
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

		public int GetRedConfigCount (RedConfigSearchCondition condition)
		{
			var query = _redconfigRepository.Table;
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
				if (condition.StatusBegin.HasValue)
                {
                    query = query.Where(q => q.Status>= condition.StatusBegin.Value);
                }
                if (condition.StatusEnd.HasValue)
                {
                    query = query.Where(q => q.Status < condition.StatusEnd.Value);
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
				if (condition.商家关联ids != null && condition.商家关联ids.Any())
                {
                    query = query.Where(q => condition.商家关联ids.Contains(q.商家关联id));
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