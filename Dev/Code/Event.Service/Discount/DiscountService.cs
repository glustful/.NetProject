using System;
using System.Collections.Generic;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using Event.Entity.Model;
using Zerg.Common.Data;

namespace Event.Service.Discount
{
	public class DiscountService : IDiscountService
	{
		private readonly IEventRepository<DiscountEntity> _discountRepository;
		private readonly ILog _log;

		public DiscountService(IEventRepository<DiscountEntity> discountRepository,ILog log)
		{
			_discountRepository = discountRepository;
			_log = log;
		}
		
		public DiscountEntity Create (DiscountEntity entity)
		{
			try
            {
                _discountRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(DiscountEntity entity)
		{
			try
            {
                _discountRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public DiscountEntity Update (DiscountEntity entity)
		{
			try
            {
                _discountRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public DiscountEntity GetDiscountById (int id)
		{
			try
            {
                return _discountRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<DiscountEntity> GetDiscountsByCondition(DiscountSearchCondition condition)
		{
			var query = _discountRepository.Table;
			try
			{
				if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime>= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }
				if (condition.UptimeBegin.HasValue)
                {
                    query = query.Where(q => q.Uptime>= condition.UptimeBegin.Value);
                }
                if (condition.UptimeEnd.HasValue)
                {
                    query = query.Where(q => q.Uptime < condition.UptimeEnd.Value);
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Crowds != null && condition.Crowds.Any())
                {
                    query = query.Where(q => condition.Crowds.Contains(q.Crowd));
                }
				if (condition.Numbers != null && condition.Numbers.Any())
                {
                    query = query.Where(q => condition.Numbers.Contains(q.Number));
                }
				if (condition.Discounts != null && condition.Discounts.Any())
                {
                    query = query.Where(q => condition.Discounts.Contains(q.Discount));
                }
				if (condition.Addusers != null && condition.Addusers.Any())
                {
                    query = query.Where(q => condition.Addusers.Contains(q.Adduser));
                }
				if (condition.Upusers != null && condition.Upusers.Any())
                {
                    query = query.Where(q => condition.Upusers.Contains(q.Upuser));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumDiscountSearchOrderBy.OrderById:
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

		public int GetDiscountCount (DiscountSearchCondition condition)
		{
			var query = _discountRepository.Table;
			try
			{
				if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime>= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }
				if (condition.UptimeBegin.HasValue)
                {
                    query = query.Where(q => q.Uptime>= condition.UptimeBegin.Value);
                }
                if (condition.UptimeEnd.HasValue)
                {
                    query = query.Where(q => q.Uptime < condition.UptimeEnd.Value);
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Crowds != null && condition.Crowds.Any())
                {
                    query = query.Where(q => condition.Crowds.Contains(q.Crowd));
                }
				if (condition.Numbers != null && condition.Numbers.Any())
                {
                    query = query.Where(q => condition.Numbers.Contains(q.Number));
                }
				if (condition.Discounts != null && condition.Discounts.Any())
                {
                    query = query.Where(q => condition.Discounts.Contains(q.Discount));
                }
				if (condition.Addusers != null && condition.Addusers.Any())
                {
                    query = query.Where(q => condition.Addusers.Contains(q.Adduser));
                }
				if (condition.Upusers != null && condition.Upusers.Any())
                {
                    query = query.Where(q => condition.Upusers.Contains(q.Upuser));
                }
				return query.Count();
			}
			catch(Exception e)
			{
				_log.Error(e,"数据库操作出错");
                return -1;
			}
		}
        public List<DiscountEntity> GetDiscountByCrowdId(int crowdId)
        {
            try
            {
                return _discountRepository.Table.Where(p => p.Crowd.Id == crowdId).OrderBy (p=>p.Number).ToList();
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }
        public int  GetDiscountMaxCountByCrowdId(int crowdId)
        {
            try
            {
                return _discountRepository.Table.Where(p => p.Crowd.Id == crowdId).OrderByDescending (p=>p.Number).First ().Number;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return 0;
            }
        }
	}
}