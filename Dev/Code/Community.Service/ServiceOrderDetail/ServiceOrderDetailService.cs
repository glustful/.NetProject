using System;
using System.Linq;
using Community.Entity.Model.ServiceOrderDetail;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Community.Service.ServiceOrderDetail
{
	public class ServiceOrderDetailService : IServiceOrderDetailService
	{
		private readonly ICommunityRepository<ServiceOrderDetailEntity> _serviceorderdetailRepository;
		private readonly ILog _log;

		public ServiceOrderDetailService(ICommunityRepository<ServiceOrderDetailEntity> serviceorderdetailRepository,ILog log)
		{
			_serviceorderdetailRepository = serviceorderdetailRepository;
			_log = log;
		}
		
		public ServiceOrderDetailEntity Create (ServiceOrderDetailEntity entity)
		{
			try
            {
                _serviceorderdetailRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(ServiceOrderDetailEntity entity)
		{
			try
            {
                _serviceorderdetailRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public ServiceOrderDetailEntity Update (ServiceOrderDetailEntity entity)
		{
			try
            {
                _serviceorderdetailRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public ServiceOrderDetailEntity GetServiceOrderDetailById (int id)
		{
			try
            {
                return _serviceorderdetailRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<ServiceOrderDetailEntity> GetServiceOrderDetailsByCondition(ServiceOrderDetailSearchCondition condition)
		{
			var query = _serviceorderdetailRepository.Table;
			try
			{
				if (condition.CountBegin.HasValue)
                {
                    query = query.Where(q => q.Count>= condition.CountBegin.Value);
                }
                if (condition.CountEnd.HasValue)
                {
                    query = query.Where(q => q.Count < condition.CountEnd.Value);
                }
				if (condition.PriceBegin.HasValue)
                {
                    query = query.Where(q => q.Price>= condition.PriceBegin.Value);
                }
                if (condition.PriceEnd.HasValue)
                {
                    query = query.Where(q => q.Price < condition.PriceEnd.Value);
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.ServiceOrders != null && condition.ServiceOrders.Any())
                {
                    query = query.Where(q => condition.ServiceOrders.Contains(q.ServiceOrder));
                }
				if (condition.Products != null && condition.Products.Any())
                {
                    query = query.Where(q => condition.Products.Contains(q.Product));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumServiceOrderDetailSearchOrderBy.OrderById:
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

		public int GetServiceOrderDetailCount (ServiceOrderDetailSearchCondition condition)
		{
			var query = _serviceorderdetailRepository.Table;
			try
			{
				if (condition.CountBegin.HasValue)
                {
                    query = query.Where(q => q.Count>= condition.CountBegin.Value);
                }
                if (condition.CountEnd.HasValue)
                {
                    query = query.Where(q => q.Count < condition.CountEnd.Value);
                }
				if (condition.PriceBegin.HasValue)
                {
                    query = query.Where(q => q.Price>= condition.PriceBegin.Value);
                }
                if (condition.PriceEnd.HasValue)
                {
                    query = query.Where(q => q.Price < condition.PriceEnd.Value);
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.ServiceOrders != null && condition.ServiceOrders.Any())
                {
                    query = query.Where(q => condition.ServiceOrders.Contains(q.ServiceOrder));
                }
				if (condition.Products != null && condition.Products.Any())
                {
                    query = query.Where(q => condition.Products.Contains(q.Product));
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