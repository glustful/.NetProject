using System;
using System.Linq;
using Community.Entity.Model.Parameter;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Community.Service.Parameter
{
	public class ParameterService : IParameterService
	{
		private readonly ICommunityRepository<ParameterEntity> _parameterRepository;
		private readonly ILog _log;

		public ParameterService(ICommunityRepository<ParameterEntity> parameterRepository,ILog log)
		{
			_parameterRepository = parameterRepository;
			_log = log;
		}
		
		public ParameterEntity Create (ParameterEntity entity)
		{
			try
            {
                _parameterRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(ParameterEntity entity)
		{
			try
            {
                _parameterRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public ParameterEntity Update (ParameterEntity entity)
		{
			try
            {
                _parameterRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public ParameterEntity GetParameterById (int id)
		{
			try
            {
                return _parameterRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<ParameterEntity> GetParametersByCondition(ParameterSearchCondition condition)
		{
			var query = _parameterRepository.Table;
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
				if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Categorys != null && condition.Categorys.Any())
                {
                    query = query.Where(q => condition.Categorys.Contains(q.Category));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumParameterSearchOrderBy.OrderById:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
						case EnumParameterSearchOrderBy.OrderBySort:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Sort):query.OrderBy(q=>q.Sort);
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

		public int GetParameterCount (ParameterSearchCondition condition)
		{
			var query = _parameterRepository.Table;
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
				if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Categorys != null && condition.Categorys.Any())
                {
                    query = query.Where(q => condition.Categorys.Contains(q.Category));
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