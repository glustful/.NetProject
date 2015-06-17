using System;
using System.Linq;
using Trading.Entity.Entity.Area;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Trading.Service.Area
{
    public class AreaService:IAreaService
    {
        private readonly ITradingRepository<AreaEntity> _areaRepository;
		private readonly ILog _log;

        public AreaService(ITradingRepository<AreaEntity> areaRepository, ILog log)
		{
			_areaRepository = areaRepository;
			_log = log;
		}
        public AreaEntity Create(AreaEntity entity)
        {
            try
            {
                _areaRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public AreaEntity Update(AreaEntity entity)
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

        public AreaEntity GetAreaById(int id)
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
        public IQueryable<AreaEntity> GetBySuperArea(int parentId)
        {
            var query = _areaRepository.Table;
            try
            {
                query = query.Where(q => q.ParentId == parentId);
                return query.OrderBy(q => q.Id);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }
        public IQueryable<AreaEntity> GetAreaByCondition(AreaSearchCondition condition)
        {
            var query = _areaRepository.Table;
            try
            {
                if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
                if (condition.ParentId.HasValue)
                {
                    query = query.Where(q => q.ParentId == condition.ParentId);
                }
                if (!string.IsNullOrEmpty(condition.AreaName))
                {
                    query = query.Where(q => q.AreaName == condition.AreaName);
                }
                if (condition.OrderBy.HasValue)
                {
                    switch (condition.OrderBy.Value)
                    {
                        case EnumAreaSearchOrderBy.OrderById:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.Id) : query.OrderBy(q => q.Id);
                            break;
                    }
                }
                else
                {
                    query = query.OrderBy(q => q.Id);
                }

                if (condition.Page.HasValue && condition.PageCount.HasValue)
                {
                    query = query.Skip((condition.Page.Value - 1) * condition.PageCount.Value).Take(condition.PageCount.Value);
                }
                return query;
            }
            catch (Exception e)
            {
                _log.Error(e,"数据库操作出错");
                return null;
            }
        }

        public int GetAreaCount(AreaSearchCondition condition)
        {
            var query = _areaRepository.Table;
            try
            {
                if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
                
                if (!string.IsNullOrEmpty(condition.AreaName))
                {
                    query = query.Where(q => q.AreaName == condition.AreaName);
                }
                return query.Count();
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return -1;
            }
        }
    }
}
