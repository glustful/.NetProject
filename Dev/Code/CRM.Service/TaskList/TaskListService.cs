using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.TaskList
{
	public class TaskListService : ITaskListService
	{
		private readonly IRepository<TaskListEntity> _tasklistRepository;
		private readonly ILog _log;

		public TaskListService(IRepository<TaskListEntity> tasklistRepository,ILog log)
		{
			_tasklistRepository = tasklistRepository;
			_log = log;
		}
		
		public TaskListEntity Create (TaskListEntity entity)
		{
			try
            {
                _tasklistRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(TaskListEntity entity)
		{
			try
            {
                _tasklistRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public TaskListEntity Update (TaskListEntity entity)
		{
			try
            {
                _tasklistRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public TaskListEntity GetTaskListById (int id)
		{
			try
            {
                return _tasklistRepository.GetById(id); ;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<TaskListEntity> GetTaskListsByCondition(TaskListSearchCondition condition)
		{
			var query = _tasklistRepository.Table;
			try
			{
				if (!string.IsNullOrEmpty(condition.Taskschedule))
                {
                    query = query.Where(q => q.Taskschedule.Contains(condition.Taskschedule));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Tasks != null && condition.Tasks.Any())
                {
                    query = query.Where(q => condition.Tasks.Contains(q.Task));
                }
				if (condition.Brokers != null && condition.Brokers.Any())
                {
                    query = query.Where(q => condition.Brokers.Contains(q.Broker));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumTaskListSearchOrderBy.OrderById:
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

		public int GetTaskListCount (TaskListSearchCondition condition)
		{
			var query = _tasklistRepository.Table;
			try
			{
				if (!string.IsNullOrEmpty(condition.Taskschedule))
                {
                    query = query.Where(q => q.Taskschedule.Contains(condition.Taskschedule));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Tasks != null && condition.Tasks.Any())
                {
                    query = query.Where(q => condition.Tasks.Contains(q.Task));
                }
				if (condition.Brokers != null && condition.Brokers.Any())
                {
                    query = query.Where(q => condition.Brokers.Contains(q.Broker));
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