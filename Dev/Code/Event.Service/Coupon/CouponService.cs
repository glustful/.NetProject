using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YooPoon.Core.Logging;
using Event.Entity.Entity.Coupon;

namespace Event.Service.Coupon
{
    public class CouponService:ICouponService
    {
        private readonly Zerg.Common.Data.IEventRepository<Entity.Entity.Coupon.Coupon> _couponRepository;
        private readonly ILog _log;
        public CouponService(Zerg.Common.Data.IEventRepository<Entity.Entity.Coupon.Coupon> couponRepository,ILog log)
		{
            _couponRepository = couponRepository;
			_log = log;
		}
        /// <summary>
        /// 单个新建
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Entity.Entity.Coupon.Coupon Create(Entity.Entity.Coupon.Coupon entity)
        {
            try
            {
                _couponRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }     
        public bool Delete(Entity.Entity.Coupon.Coupon entity)
        {
            try
            {
                _couponRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return false;
            }
        }

        public Entity.Entity.Coupon.Coupon Update(Entity.Entity.Coupon.Coupon entity)
        {
            try
            {
                _couponRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e,"数据库操作出错");
                return null;                
            }
        }

        public Entity.Entity.Coupon.Coupon GetCouponById(int id)
        {
            try
            {
                return _couponRepository.GetById(id);
            }
            catch (Exception e)
            {
                _log.Error(e,"操作数据库出错");
                return null;
            }
        }       
        public IQueryable<Entity.Entity.Coupon.Coupon> GetCouponByCondition(CouponSearchCondition condition)
        {
            var query = _couponRepository.Table;
            try
            {
                if (condition.Ids.HasValue)
                {
                    query = query.Where(q => condition.Ids==q.Id);
                }
                if (!string.IsNullOrEmpty(condition.Number))
                {
                    query = query.Where(q => q.Number== condition.Number);
                }
                if (condition.CouponCategoryId.HasValue)
                {
                    query = query.Where(q => condition.CouponCategoryId == q.CouponCategoryId);
                }
                if (condition.Status.HasValue)
                {
                    query = query.Where(q => (EnumCouponStatus)condition.Status == q.Status);
                }
                if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumCouponSearchOrderBy.OrderById:
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
                
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public int GetCouponCount(CouponSearchCondition condition)
        {
            var query = _couponRepository.Table;
            try
            {
                if (condition.Ids.HasValue)
                {
                    query = query.Where(q => condition.Ids == q.Id);
                }
                if (!string.IsNullOrEmpty(condition.Number))
                {
                    query = query.Where(q => q.Number == condition.Number);
                }
                if (condition.CouponCategoryId.HasValue)
                {
                    query = query.Where(q => condition.CouponCategoryId == q.CouponCategoryId);
                }
                if (condition.Status.HasValue)
                {
                    query = query.Where(q => (EnumCouponStatus)condition.Status == q.Status);
                }
                return query.Count();
            }

            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return -1;
            }
        }

        public Entity.Entity.Coupon.Coupon GetCouponByNumber(string number)
        {
            try
            {
               return  _couponRepository.Table.FirstOrDefault(c => c.Number == number);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public bool BulkCreate(List<Entity.Entity.Coupon.Coupon> entities)
        {            
            try
            {
                _couponRepository.BulkInsert(entities);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return false;
             }
        }
    }
}
