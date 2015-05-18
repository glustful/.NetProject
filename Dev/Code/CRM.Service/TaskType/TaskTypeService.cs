






using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.TaskType
{
	public class TaskTypeService : ITaskTypeService
	{
		private readonly Zerg.Common.Data.ICRMRepository<TaskTypeEntity> _tasktypeRepository;
		private readonly ILog _log;

		public TaskTypeService(Zerg.Common.Data.ICRMRepository<TaskTypeEntity> tasktypeRepository,ILog log)
		{
			_tasktypeRepository = tasktypeRepository;
			_log = log;
		}
		
		public TaskTypeEntity Create (TaskTypeEntity entity)
		{
			try
            {
                _tasktypeRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(TaskTypeEntity entity)
		{
			try
            {
                _tasktypeRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public TaskTypeEntity Update (TaskTypeEntity entity)
		{
			try
            {
                _tasktypeRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public TaskTypeEntity GetTaskTypeById (int id)
		{
			try
            {
                return _tasktypeRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<TaskTypeEntity> GetTaskTypesByCondition(TaskTypeSearchCondition condition)
		{
			var query = _tasktypeRepository.Table;
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



				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }




				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {

						case EnumTaskTypeSearchOrderBy.OrderById:
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

		public int GetTaskTypeCount (TaskTypeSearchCondition condition)
		{
			var query = _tasktypeRepository.Table;
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