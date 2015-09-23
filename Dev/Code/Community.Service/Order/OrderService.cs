using System;
using System.Linq;
using Community.Entity.Model.Order;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Community.Service.Order
{
	public class OrderService : IOrderService
	{
		private readonly ICommunityRepository<OrderEntity> _orderRepository;
		private readonly ILog _log;

		public OrderService(ICommunityRepository<OrderEntity> orderRepository,ILog log)
		{
			_orderRepository = orderRepository;
			_log = log;
		}
		
		public OrderEntity Create (OrderEntity entity)
		{
			try
            {
                _orderRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(OrderEntity entity)
		{
			try
            {
                _orderRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public OrderEntity Update (OrderEntity entity)
		{
			try
            {
                _orderRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public OrderEntity GetOrderById (int id)
		{
			try
            {
                return _orderRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<OrderEntity> GetOrdersByCondition(OrderSearchCondition condition)
		{
			var query = _orderRepository.Table;
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
				if (condition.UpddateBegin.HasValue)
                {
                    query = query.Where(q => q.UpdDate>= condition.UpddateBegin.Value);
                }
                if (condition.UpddateEnd.HasValue)
                {
                    query = query.Where(q => q.UpdDate < condition.UpddateEnd.Value);
                }
				if (condition.TotalpriceBegin.HasValue)
                {
                    query = query.Where(q => q.Totalprice>= condition.TotalpriceBegin.Value);
                }
                if (condition.TotalpriceEnd.HasValue)
                {
                    query = query.Where(q => q.Totalprice < condition.TotalpriceEnd.Value);
                }
				if (condition.ActualpriceBegin.HasValue)
                {
                    query = query.Where(q => q.Actualprice>= condition.ActualpriceBegin.Value);
                }
                if (condition.ActualpriceEnd.HasValue)
                {
                    query = query.Where(q => q.Actualprice < condition.ActualpriceEnd.Value);
                }
				if (!string.IsNullOrEmpty(condition.No))
                {
                    query = query.Where(q => q.No == condition.No);
                }
				if (condition.Status.HasValue)
                {
                    query = query.Where(q => q.Status == condition.Status.Value);
                }
				if (!string.IsNullOrEmpty(condition.CustomerName))
                {
                    query = query.Where(q => q.CustomerName.Contains(condition.CustomerName));
                }
				if (!string.IsNullOrEmpty(condition.Remark))
                {
                    query = query.Where(q => q.Remark.Contains(condition.Remark));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Addusers != null && condition.Addusers.Any())
                {
                    query = query.Where(q => condition.Addusers.Contains(q.AddUser));
                }
				if (condition.Updusers != null && condition.Updusers.Any())
                {
                    query = query.Where(q => condition.Updusers.Contains(q.UpdUser));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumOrderSearchOrderBy.OrderById:
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

		public int GetOrderCount (OrderSearchCondition condition)
		{
			var query = _orderRepository.Table;
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
				if (condition.UpddateBegin.HasValue)
                {
                    query = query.Where(q => q.UpdDate>= condition.UpddateBegin.Value);
                }
                if (condition.UpddateEnd.HasValue)
                {
                    query = query.Where(q => q.UpdDate < condition.UpddateEnd.Value);
                }
				if (condition.TotalpriceBegin.HasValue)
                {
                    query = query.Where(q => q.Totalprice>= condition.TotalpriceBegin.Value);
                }
                if (condition.TotalpriceEnd.HasValue)
                {
                    query = query.Where(q => q.Totalprice < condition.TotalpriceEnd.Value);
                }
				if (condition.ActualpriceBegin.HasValue)
                {
                    query = query.Where(q => q.Actualprice>= condition.ActualpriceBegin.Value);
                }
                if (condition.ActualpriceEnd.HasValue)
                {
                    query = query.Where(q => q.Actualprice < condition.ActualpriceEnd.Value);
                }
				if (!string.IsNullOrEmpty(condition.No))
                {
                    query = query.Where(q => q.No == condition.No);
                }
				if (condition.Status.HasValue)
                {
                    query = query.Where(q => q.Status == condition.Status.Value);
                }
				if (!string.IsNullOrEmpty(condition.CustomerName))
                {
                    query = query.Where(q => q.CustomerName.Contains(condition.CustomerName));
                }
				if (!string.IsNullOrEmpty(condition.Remark))
                {
                    query = query.Where(q => q.Remark.Contains(condition.Remark));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Addusers != null && condition.Addusers.Any())
                {
                    query = query.Where(q => condition.Addusers.Contains(q.AddUser));
                }
				if (condition.Updusers != null && condition.Updusers.Any())
                {
                    query = query.Where(q => condition.Updusers.Contains(q.UpdUser));
                }
				return query.Count();
			}
			catch(Exception e)
			{
				_log.Error(e,"数据库操作出错");
                return -1;
			}
		}

	    public OrderEntity GetOrderByNo(string orderNo)
	    {
	        try
	        {
	            return _orderRepository.Get(o => o.No == orderNo);
	        }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
	    }
	}
}