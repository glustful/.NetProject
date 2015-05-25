






using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using Trading.Entity.Model;

namespace Trading.Service.OrderDetail
{
	public class OrderDetailService : IOrderDetailService
	{
		private readonly Zerg.Common.Data.ITradingRepository<OrderDetailEntity> _orderdetailRepository;
		private readonly ILog _log;

		public OrderDetailService(Zerg.Common.Data.ITradingRepository<OrderDetailEntity> orderdetailRepository,ILog log)
		{
			_orderdetailRepository = orderdetailRepository;
			_log = log;
		}
		
		public OrderDetailEntity Create (OrderDetailEntity entity)
		{
			try
            {
                _orderdetailRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(OrderDetailEntity entity)
		{
			try
            {
                _orderdetailRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public OrderDetailEntity Update (OrderDetailEntity entity)
		{
			try
            {
                _orderdetailRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public OrderDetailEntity GetOrderDetailById (int id)
		{
			try
            {
                return _orderdetailRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<OrderDetailEntity> GetOrderDetailsByCondition(OrderDetailSearchCondition condition)
		{
			var query = _orderdetailRepository.Table;
			try
			{

				if (condition.PriceBegin.HasValue)
                {
                    query = query.Where(q => q.Price>= condition.PriceBegin.Value);
                }
                if (condition.PriceEnd.HasValue)
                {
                    query = query.Where(q => q.Price < condition.PriceEnd.Value);
                }


				if (condition.CommissionBegin.HasValue)
                {
                    query = query.Where(q => q.Commission>= condition.CommissionBegin.Value);
                }
                if (condition.CommissionEnd.HasValue)
                {
                    query = query.Where(q => q.Commission < condition.CommissionEnd.Value);
                }


				if (condition.AdddateBegin.HasValue)
                {
                    query = query.Where(q => q.Adddate>= condition.AdddateBegin.Value);
                }
                if (condition.AdddateEnd.HasValue)
                {
                    query = query.Where(q => q.Adddate < condition.AdddateEnd.Value);
                }


				if (condition.UpddateBegin.HasValue)
                {
                    query = query.Where(q => q.Upddate>= condition.UpddateBegin.Value);
                }
                if (condition.UpddateEnd.HasValue)
                {
                    query = query.Where(q => q.Upddate < condition.UpddateEnd.Value);
                }


				if (!string.IsNullOrEmpty(condition.Productname))
                {
                    query = query.Where(q => q.Productname.Contains(condition.Productname));
                }



				if (!string.IsNullOrEmpty(condition.Snapshoturl))
                {
                    query = query.Where(q => q.Snapshoturl.Contains(condition.Snapshoturl));
                }



				if (!string.IsNullOrEmpty(condition.Remark))
                {
                    query = query.Where(q => q.Remark.Contains(condition.Remark));
                }



				if (!string.IsNullOrEmpty(condition.Adduser))
                {
                    query = query.Where(q => q.Adduser.Contains(condition.Adduser));
                }



				if (!string.IsNullOrEmpty(condition.Upduser))
                {
                    query = query.Where(q => q.Upduser.Contains(condition.Upduser));
                }



				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }


				if (condition.Products != null && condition.Products.Any())
                {
                    query = query.Where(q => condition.Products.Contains(q.Product));
                }




				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {

						case EnumOrderDetailSearchOrderBy.OrderById:
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

		public int GetOrderDetailCount (OrderDetailSearchCondition condition)
		{
			var query = _orderdetailRepository.Table;
			try
			{

				if (condition.PriceBegin.HasValue)
                {
                    query = query.Where(q => q.Price>= condition.PriceBegin.Value);
                }
                if (condition.PriceEnd.HasValue)
                {
                    query = query.Where(q => q.Price < condition.PriceEnd.Value);
                }


				if (condition.CommissionBegin.HasValue)
                {
                    query = query.Where(q => q.Commission>= condition.CommissionBegin.Value);
                }
                if (condition.CommissionEnd.HasValue)
                {
                    query = query.Where(q => q.Commission < condition.CommissionEnd.Value);
                }


				if (condition.AdddateBegin.HasValue)
                {
                    query = query.Where(q => q.Adddate>= condition.AdddateBegin.Value);
                }
                if (condition.AdddateEnd.HasValue)
                {
                    query = query.Where(q => q.Adddate < condition.AdddateEnd.Value);
                }


				if (condition.UpddateBegin.HasValue)
                {
                    query = query.Where(q => q.Upddate>= condition.UpddateBegin.Value);
                }
                if (condition.UpddateEnd.HasValue)
                {
                    query = query.Where(q => q.Upddate < condition.UpddateEnd.Value);
                }


				if (!string.IsNullOrEmpty(condition.Productname))
                {
                    query = query.Where(q => q.Productname.Contains(condition.Productname));
                }



				if (!string.IsNullOrEmpty(condition.Snapshoturl))
                {
                    query = query.Where(q => q.Snapshoturl.Contains(condition.Snapshoturl));
                }



				if (!string.IsNullOrEmpty(condition.Remark))
                {
                    query = query.Where(q => q.Remark.Contains(condition.Remark));
                }



				if (!string.IsNullOrEmpty(condition.Adduser))
                {
                    query = query.Where(q => q.Adduser.Contains(condition.Adduser));
                }



				if (!string.IsNullOrEmpty(condition.Upduser))
                {
                    query = query.Where(q => q.Upduser.Contains(condition.Upduser));
                }



				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
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