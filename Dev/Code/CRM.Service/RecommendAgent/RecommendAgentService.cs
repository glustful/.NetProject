using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.RecommendAgent
{
	public class RecommendAgentService : IRecommendAgentService
	{
		private readonly Zerg.Common.Data.ICRMRepository<RecommendAgentEntity> _recommendagentRepository;
		private readonly ILog _log;

		public RecommendAgentService(Zerg.Common.Data.ICRMRepository<RecommendAgentEntity> recommendagentRepository,ILog log)
		{
			_recommendagentRepository = recommendagentRepository;
			_log = log;
		}
		
		public RecommendAgentEntity Create (RecommendAgentEntity entity)
		{
			try
            {
                _recommendagentRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(RecommendAgentEntity entity)
		{
			try
            {
                _recommendagentRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public RecommendAgentEntity Update (RecommendAgentEntity entity)
		{
			try
            {
                _recommendagentRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public RecommendAgentEntity GetRecommendAgentById (int id)
		{
			try
            {
                return _recommendagentRepository.GetById(id); ;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<RecommendAgentEntity> GetRecommendAgentsByCondition(RecommendAgentSearchCondition condition)
		{
			var query = _recommendagentRepository.Table;
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
				if (!string.IsNullOrEmpty( condition.Phone))
                {
                    query = query.Where(q => q.Phone == condition.Phone);
                }
				if (!string.IsNullOrEmpty(condition.Qq))
                {
                    query = query.Where(q => q.Qq == condition.Qq);
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
				if (!string.IsNullOrEmpty(condition.BrokerId.ToString()) )
                {
                    query = query.Where(q => q.Broker.Id == condition.BrokerId);
                }
				if (condition.PresenteebIds != null && condition.PresenteebIds.Any())
                {
                    query = query.Where(q => condition.PresenteebIds.Contains(q.PresenteebId));
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
						case EnumRecommendAgentSearchOrderBy.OrderById:
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

		public int GetRecommendAgentCount (RecommendAgentSearchCondition condition)
		{
			var query = _recommendagentRepository.Table;
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
                if (!string.IsNullOrEmpty(condition.Phone))
                {
                    query = query.Where(q => q.Phone == condition.Phone);
                }
                if (!string.IsNullOrEmpty(condition.Qq))
                {
                    query = query.Where(q => q.Qq == condition.Qq);
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
				if (   !string.IsNullOrEmpty( condition.BrokerId.ToString()) )
                {
                    query = query.Where(q =>q.Broker.Id==condition.BrokerId);
                }
				if (condition.PresenteebIds != null && condition.PresenteebIds.Any())
                {
                    query = query.Where(q => condition.PresenteebIds.Contains(q.PresenteebId));
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