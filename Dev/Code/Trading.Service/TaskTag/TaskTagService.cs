using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.TaskTag
{
	public class TaskTagService : ITaskTagService
	{
		private readonly IRepository<TaskTagEntity> _tasktagRepository;
		private readonly ILog _log;

		public TaskTagService(IRepository<TaskTagEntity> tasktagRepository,ILog log)
		{
			_tasktagRepository = tasktagRepository;
			_log = log;
		}
		
		public TaskTagEntity Create (TaskTagEntity entity)
		{
			try
            {
                _tasktagRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(TaskTagEntity entity)
		{
			try
            {
                _tasktagRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public TaskTagEntity Update (TaskTagEntity entity)
		{
			try
            {
                _tasktagRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public TaskTagEntity GetTaskTagById (int id)
		{
			try
            {
                return _tasktagRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<TaskTagEntity> GetTaskTagsByCondition(TaskTagSearchCondition condition)
		{
			var query = _tasktagRepository.Table;
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
						case EnumTaskTagSearchOrderBy.OrderById:
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

		public int GetTaskTagCount (TaskTagSearchCondition condition)
		{
			var query = _tasktagRepository.Table;
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