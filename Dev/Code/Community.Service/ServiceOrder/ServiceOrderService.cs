using System;
using System.Linq;
using Community.Entity.Model.ServiceOrder;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Community.Service.ServiceOrder
{
	public class ServiceOrderService : IServiceOrderService
	{
		private readonly ICommunityRepository<ServiceOrderEntity> _serviceorderRepository;
		private readonly ILog _log;

		public ServiceOrderService(ICommunityRepository<ServiceOrderEntity> serviceorderRepository,ILog log)
		{
			_serviceorderRepository = serviceorderRepository;
			_log = log;
		}
		
		public ServiceOrderEntity Create (ServiceOrderEntity entity)
		{
			try
            {
                _serviceorderRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(ServiceOrderEntity entity)
		{
			try
            {
                _serviceorderRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public ServiceOrderEntity Update (ServiceOrderEntity entity)
		{
			try
            {
                _serviceorderRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public ServiceOrderEntity GetServiceOrderById (int id)
		{
			try
            {
                return _serviceorderRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<ServiceOrderEntity> GetServiceOrdersByCondition(ServiceOrderSearchCondition condition)
		{
			var query = _serviceorderRepository.Table;
			try
			{
				if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.AddTime>= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.AddTime < condition.AddtimeEnd.Value);
                }
				if (condition.FleeBegin.HasValue)
                {
                    query = query.Where(q => q.Flee>= condition.FleeBegin.Value);
                }
                if (condition.FleeEnd.HasValue)
                {
                    query = query.Where(q => q.Flee < condition.FleeEnd.Value);
                }
				if (condition.ServicetimeBegin.HasValue)
                {
                    query = query.Where(q => q.Servicetime>= condition.ServicetimeBegin.Value);
                }
                if (condition.ServicetimeEnd.HasValue)
                {
                    query = query.Where(q => q.Servicetime < condition.ServicetimeEnd.Value);
                }
				if (!string.IsNullOrEmpty(condition.OrderNo))
                {
                    query = query.Where(q => q.OrderNo == condition.OrderNo);
                }
//				if (!string.IsNullOrEmpty(condition.Address))
//                {
//                    query = query.Where(q => q.Address.Contains(condition.Address));
//                }
				if (!string.IsNullOrEmpty(condition.Remark))
                {
                    query = query.Where(q => q.Remark.Contains(condition.Remark));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.AddUsers != null && condition.AddUsers.Any())
                {
                    query = query.Where(q => condition.AddUsers.Contains(q.AddUser));
                }
			    if (condition.Status != null)
			    {
			        query = query.Where(c => c.Status == condition.Status);
			    }
                
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumServiceOrderSearchOrderBy.OrderById:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
						case EnumServiceOrderSearchOrderBy.OrderByOrderNo:
							query = condition.IsDescending?query.OrderByDescending(q=>q.OrderNo):query.OrderBy(q=>q.OrderNo);
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

		public int GetServiceOrderCount (ServiceOrderSearchCondition condition)
		{
			var query = _serviceorderRepository.Table;
			try
			{
				if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.AddTime>= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.AddTime < condition.AddtimeEnd.Value);
                }
				if (condition.FleeBegin.HasValue)
                {
                    query = query.Where(q => q.Flee>= condition.FleeBegin.Value);
                }
                if (condition.FleeEnd.HasValue)
                {
                    query = query.Where(q => q.Flee < condition.FleeEnd.Value);
                }
				if (condition.ServicetimeBegin.HasValue)
                {
                    query = query.Where(q => q.Servicetime>= condition.ServicetimeBegin.Value);
                }
                if (condition.ServicetimeEnd.HasValue)
                {
                    query = query.Where(q => q.Servicetime < condition.ServicetimeEnd.Value);
                }
				if (!string.IsNullOrEmpty(condition.OrderNo))
                {
                    query = query.Where(q => q.OrderNo == condition.OrderNo);
                }
//				if (!string.IsNullOrEmpty(condition.Address))
//                {
//                    query = query.Where(q => q.Address.Contains(condition.Address));
//                }
				if (!string.IsNullOrEmpty(condition.Remark))
                {
                    query = query.Where(q => q.Remark.Contains(condition.Remark));
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
			catch(Exception e)
			{
				_log.Error(e,"数据库操作出错");
                return -1;
			}
		}

	    public ServiceOrderEntity GetServiceOrderByNo(string orderNo)
	    {
	        try
	        {
	            return _serviceorderRepository.Get(c => c.OrderNo == orderNo);
	        }
	        catch (Exception e)
	        {
                _log.Error(e, "数据库操作出错");
                return null;
	        }
	    }
	}
}