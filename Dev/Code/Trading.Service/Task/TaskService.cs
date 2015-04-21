using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.Task
{
	public class TaskService : ITaskService
	{
		private readonly IRepository<TaskEntity> _taskRepository;
		private readonly ILog _log;

		public TaskService(IRepository<TaskEntity> taskRepository,ILog log)
		{
			_taskRepository = taskRepository;
			_log = log;
		}
		
		public TaskEntity Create (TaskEntity entity)
		{
			try
            {
                _taskRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(TaskEntity entity)
		{
			try
            {
                _taskRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public TaskEntity Update (TaskEntity entity)
		{
			try
            {
                _taskRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public TaskEntity GetTaskById (int id)
		{
			try
            {
                return _taskRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<TaskEntity> GetTasksByCondition(TaskSearchCondition condition)
		{
			var query = _taskRepository.Table;
			try
			{
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
				if (!string.IsNullOrEmpty(condition.Taskname))
                {
                    query = query.Where(q => q.Taskname.Contains(condition.Taskname));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.TaskPunishments != null && condition.TaskPunishments.Any())
                {
                    query = query.Where(q => condition.TaskPunishments.Contains(q.TaskPunishment));
                }
				if (condition.TaskAwards != null && condition.TaskAwards.Any())
                {
                    query = query.Where(q => condition.TaskAwards.Contains(q.TaskAward));
                }
				if (condition.TaskTags != null && condition.TaskTags.Any())
                {
                    query = query.Where(q => condition.TaskTags.Contains(q.TaskTag));
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
						case EnumTaskSearchOrderBy.OrderById:
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

		public int GetTaskCount (TaskSearchCondition condition)
		{
			var query = _taskRepository.Table;
			try
			{
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
				if (!string.IsNullOrEmpty(condition.Taskname))
                {
                    query = query.Where(q => q.Taskname.Contains(condition.Taskname));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.TaskPunishments != null && condition.TaskPunishments.Any())
                {
                    query = query.Where(q => condition.TaskPunishments.Contains(q.TaskPunishment));
                }
				if (condition.TaskAwards != null && condition.TaskAwards.Any())
                {
                    query = query.Where(q => condition.TaskAwards.Contains(q.TaskAward));
                }
				if (condition.TaskTags != null && condition.TaskTags.Any())
                {
                    query = query.Where(q => condition.TaskTags.Contains(q.TaskTag));
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