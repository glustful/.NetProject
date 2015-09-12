using System;
using System.Linq;
using Community.Entity.Model.ParameterValue;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Community.Service.ParameterValue
{
	public class ParameterValueService : IParameterValueService
	{
		private readonly ICommunityRepository<ParameterValueEntity> _parametervalueRepository;
		private readonly ILog _log;

		public ParameterValueService(ICommunityRepository<ParameterValueEntity> parametervalueRepository,ILog log)
		{
			_parametervalueRepository = parametervalueRepository;
			_log = log;
		}
		
		public ParameterValueEntity Create (ParameterValueEntity entity)
		{
			try
            {
                _parametervalueRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(ParameterValueEntity entity)
		{
			try
            {
                _parametervalueRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public ParameterValueEntity Update (ParameterValueEntity entity)
		{
			try
            {
                _parametervalueRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public ParameterValueEntity GetParameterValueById (int id)
		{
			try
            {
                return _parametervalueRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<ParameterValueEntity> GetParameterValuesByCondition(ParameterValueSearchCondition condition)
		{
			var query = _parametervalueRepository.Table;
			try
			{
				if (condition.AddTimeBegin.HasValue)
                {
                    query = query.Where(q => q.AddTime>= condition.AddTimeBegin.Value);
                }
                if (condition.AddTimeEnd.HasValue)
                {
                    query = query.Where(q => q.AddTime < condition.AddTimeEnd.Value);
                }
				if (condition.UpdTimeBegin.HasValue)
                {
                    query = query.Where(q => q.UpdTime>= condition.UpdTimeBegin.Value);
                }
                if (condition.UpdTimeEnd.HasValue)
                {
                    query = query.Where(q => q.UpdTime < condition.UpdTimeEnd.Value);
                }
				if (condition.AddUser.HasValue)
                {
                    query = query.Where(q => q.AddUser == condition.AddUser.Value);
                }
				if (condition.UpdUser.HasValue)
                {
                    query = query.Where(q => q.UpdUser == condition.UpdUser.Value);
                }
				if (!string.IsNullOrEmpty(condition.ParameterValue))
                {
                    query = query.Where(q => q.Value.Contains(condition.ParameterValue));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Parameters != null && condition.Parameters.Any())
                {
                    query = query.Where(q => condition.Parameters.Contains(q.Parameter));
                }
				if (condition.Sorts != null && condition.Sorts.Any())
                {
                    query = query.Where(q => condition.Sorts.Contains(q.Sort));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumParameterValueSearchOrderBy.OrderById:
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

		public int GetParameterValueCount (ParameterValueSearchCondition condition)
		{
			var query = _parametervalueRepository.Table;
			try
			{
				if (condition.AddTimeBegin.HasValue)
                {
                    query = query.Where(q => q.AddTime>= condition.AddTimeBegin.Value);
                }
                if (condition.AddTimeEnd.HasValue)
                {
                    query = query.Where(q => q.AddTime < condition.AddTimeEnd.Value);
                }
				if (condition.UpdTimeBegin.HasValue)
                {
                    query = query.Where(q => q.UpdTime>= condition.UpdTimeBegin.Value);
                }
                if (condition.UpdTimeEnd.HasValue)
                {
                    query = query.Where(q => q.UpdTime < condition.UpdTimeEnd.Value);
                }
				if (condition.AddUser.HasValue)
                {
                    query = query.Where(q => q.AddUser == condition.AddUser.Value);
                }
				if (condition.UpdUser.HasValue)
                {
                    query = query.Where(q => q.UpdUser == condition.UpdUser.Value);
                }
				if (!string.IsNullOrEmpty(condition.ParameterValue))
                {
                    query = query.Where(q => q.Value.Contains(condition.ParameterValue));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Parameters != null && condition.Parameters.Any())
                {
                    query = query.Where(q => condition.Parameters.Contains(q.Parameter));
                }
				if (condition.Sorts != null && condition.Sorts.Any())
                {
                    query = query.Where(q => condition.Sorts.Contains(q.Sort));
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