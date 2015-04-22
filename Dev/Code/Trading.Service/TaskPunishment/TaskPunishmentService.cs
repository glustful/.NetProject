using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.TaskPunishment
{
	public class TaskPunishmentService : ITaskPunishmentService
	{
		private readonly IRepository<TaskPunishmentEntity> _taskpunishmentRepository;
		private readonly ILog _log;

		public TaskPunishmentService(IRepository<TaskPunishmentEntity> taskpunishmentRepository,ILog log)
		{
			_taskpunishmentRepository = taskpunishmentRepository;
			_log = log;
		}
		
		public TaskPunishmentEntity Create (TaskPunishmentEntity entity)
		{
			try
            {
                _taskpunishmentRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(TaskPunishmentEntity entity)
		{
			try
            {
                _taskpunishmentRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public TaskPunishmentEntity Update (TaskPunishmentEntity entity)
		{
			try
            {
                _taskpunishmentRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public TaskPunishmentEntity GetTaskPunishmentById (int id)
		{
			try
            {
                return _taskpunishmentRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<TaskPunishmentEntity> GetTaskPunishmentsByCondition(TaskPunishmentSearchCondition condition)
		{
			var query = _taskpunishmentRepository.Table;
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
						case EnumTaskPunishmentSearchOrderBy.OrderById:
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

		public int GetTaskPunishmentCount (TaskPunishmentSearchCondition condition)
		{
			var query = _taskpunishmentRepository.Table;
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