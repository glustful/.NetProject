using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.TaskAward
{
	public class TaskAwardService : ITaskAwardService
	{
		private readonly Zerg.Common.Data.ICRMRepository<TaskAwardEntity> _taskawardRepository;
		private readonly ILog _log;

		public TaskAwardService(Zerg.Common.Data.ICRMRepository<TaskAwardEntity> taskawardRepository,ILog log)
		{
			_taskawardRepository = taskawardRepository;
			_log = log;
		}
		
		public TaskAwardEntity Create (TaskAwardEntity entity)
		{
			try
            {
                _taskawardRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(TaskAwardEntity entity)
		{
			try
            {
                _taskawardRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public TaskAwardEntity Update (TaskAwardEntity entity)
		{
			try
            {
                _taskawardRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public TaskAwardEntity GetTaskAwardById (int id)
		{
			try
            {
                return _taskawardRepository.GetById(id); ;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<TaskAwardEntity> GetTaskAwardsByCondition(TaskAwardSearchCondition condition)
		{
			var query = _taskawardRepository.Table;
			try
			{
				if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
                }
				if (!string.IsNullOrEmpty(condition.Describe))
                {
                    query = query.Where(q => q.Describe.Contains(condition.Describe));
                }
				if (!string.IsNullOrEmpty(condition.Value))
                {
                    query = query.Where(q => q.Value.Contains(condition.Value));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumTaskAwardSearchOrderBy.OrderById:
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

		public int GetTaskAwardCount (TaskAwardSearchCondition condition)
		{
			var query = _taskawardRepository.Table;
			try
			{
				if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
                }
                if (!string.IsNullOrEmpty(condition.NameRe))
                {
                    query = query.Where(q => q.Name==condition.NameRe);
                }
				if (!string.IsNullOrEmpty(condition.Describe))
                {
                    query = query.Where(q => q.Describe.Contains(condition.Describe));
                }
				if (!string.IsNullOrEmpty(condition.Value))
                {
                    query = query.Where(q => q.Value.Contains(condition.Value));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
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