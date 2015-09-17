using System;
using System.Linq;
using Community.Entity.Model.Area;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Community.Service.Area
{
	public class AreaService : IAreaService
	{
		private readonly ICommunityRepository<AreaEntity> _areaRepository;
		private readonly ILog _log;

        public AreaService(ICommunityRepository<AreaEntity> areaRepository, ILog log)
		{
			_areaRepository = areaRepository;
			_log = log;
		}
		
		public AreaEntity Create (AreaEntity entity)
		{
			try
            {
                _areaRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(AreaEntity entity)
		{
			try
            {
                _areaRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public AreaEntity Update (AreaEntity entity)
		{
			try
            {
                _areaRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public AreaEntity GetAreaById (int id)
		{
			try
            {
                return _areaRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<AreaEntity> GetAreasByCondition(AreaSearchCondition condition)
		{
			var query = _areaRepository.Table;
			try
			{
                if (!string.IsNullOrEmpty(condition.Parent_Id))
                {
                    query = query.Where(q => q.Parent.Id.ToString() == condition.Parent_Id);
                }
                if (condition.AdddateBegin.HasValue)
                {
                    query = query.Where(q => q.AddDate>= condition.AdddateBegin.Value);
                }
                if (condition.AdddateEnd.HasValue)
                {
                    query = query.Where(q => q.AddDate < condition.AdddateEnd.Value);
                }
				if (!string.IsNullOrEmpty(condition.Codeid))
                {
                    query = query.Where(q => q.CodeId == condition.Codeid);
                }
				if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumAreaSearchOrderBy.OrderById:
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

		public int GetAreaCount (AreaSearchCondition condition)
		{
			var query = _areaRepository.Table;
			try
			{
				if (condition.AdddateBegin.HasValue)
                {
                    query = query.Where(q => q.AddDate>= condition.AdddateBegin.Value);
                }
                if (condition.AdddateEnd.HasValue)
                {
                    query = query.Where(q => q.AddDate < condition.AdddateEnd.Value);
                }
				if (!string.IsNullOrEmpty(condition.Codeid))
                {
                    query = query.Where(q => q.CodeId == condition.Codeid);
                }
				if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
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