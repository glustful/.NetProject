using System;
using System.Linq;
using Community.Entity.Model.Service;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Community.Service.Service
{
    public class ServiceSerivce:IServiceService
    {
        private readonly ICommunityRepository<ServiceEntity> _serviceRepository;
		private readonly ILog _log;

        public ServiceSerivce(ICommunityRepository<ServiceEntity> serviceRepository, ILog log)
		{
			_serviceRepository = serviceRepository;
			_log = log;
		}
        public ServiceEntity Create(ServiceEntity entity)
        {
            try
            {
                _serviceRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public bool Delete(ServiceEntity entity)
        {
            try
            {
                _serviceRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return false;
            }
        }

        public ServiceEntity Update(ServiceEntity entity)
        {
            try
            {
                _serviceRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public ServiceEntity GetServiceById(int id)
        {
            try
            {
                return _serviceRepository.GetById(id);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public IQueryable<ServiceEntity> GetServiceByCondition(ServiceSearchCondition condition)
        {
            var query = _serviceRepository.Table;
            try
            {
                if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.AddTime >= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.AddTime < condition.AddtimeEnd.Value);
                }            
                if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
                if (condition.AddUsers != null && condition.AddUsers.Any())
                {
                    query = query.Where(q => condition.AddUsers.Contains(q.AddUser));
                }               

                if (condition.OrderBy.HasValue)
                {
                    switch (condition.OrderBy.Value)
                    {
                        case EnumServiceSearchOrderBy.OrderById:
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
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public int GetServiceCount(ServiceSearchCondition condition)
        {
            var query = _serviceRepository.Table;
            try
            {
                if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.AddTime >= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.AddTime < condition.AddtimeEnd.Value);
                }                                                         
                if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
                if (condition.AddUsers != null && condition.AddUsers.Any())
                {
                    query = query.Where(q => condition.AddUsers.Contains(q.AddUser));
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
