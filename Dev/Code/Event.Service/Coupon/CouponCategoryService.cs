using System;
using System.Linq;
using Event.Entity.Entity.Coupon;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Event.Service.Coupon
{
    public class CouponCategoryService:ICouponCategoryService
    {
        private readonly IEventRepository<CouponCategory> _repository;
        private readonly ILog _log;
        private readonly IEventRepository<Entity.Entity.Coupon.Coupon> _couponService;

        public CouponCategoryService(IEventRepository<CouponCategory> repository,ILog log,IEventRepository<Entity.Entity.Coupon.Coupon> couponService)
        {
            _log = log;
            _repository = repository;
            _couponService = couponService;
        }

        public bool CreateCouponCategory(CouponCategory entity)
        {
            try
            {
                _repository.Insert(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e,"数据库操作出错");
                return false;
            }
        }

        public IQueryable<CouponCategory> GetCouponCategoriesByCondition(CouponCategorySearchCondition condition)
        {
            var query = _repository.Table;
            if (condition.BrandId.HasValue)
            {
                query = query.Where(c => c.BrandId == condition.BrandId);
            }
            if (!string.IsNullOrEmpty(condition.Name))
            {
                query = query.Where(c => condition.Name == c.Name);
            }
            if (condition.OrderBy.HasValue)
            {
                switch (condition.OrderBy.Value)
                {
                    case EnumCouponCategorySearchOrderBy.OrderById:
                        query = condition.IsDescending ? query.OrderByDescending(q => q.Id) : query.OrderBy(q => q.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(q => q.Id);
            }
            if (condition.Page.HasValue && condition.PageSize.HasValue)
            {
                query = query.Skip((condition.Page.Value - 1) * condition.PageSize.Value).Take(condition.PageSize.Value);
            }
            return query;
        }

        public int GetCouponCategoriesCountByCondition(CouponCategorySearchCondition condition)
        {
            var query = _repository.Table;
            if (condition.BrandId.HasValue)
            {
                query = query.Where(c => c.BrandId == condition.BrandId);
            }
            if (!string.IsNullOrEmpty(condition.Name))
            {
                query = query.Where(c => condition.Name == c.Name);
            }
            return query.Count();
        }

        public CouponCategory GetCouponCategoryById(int id)
        {
            try
            {
                return _repository.GetById(id);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public bool UpdateCouponCategory(CouponCategory entity)
        {
            try
            {
                _repository.Update(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return false;
            }
        }

        public bool DeleteCouponCategory(int id)
        {
            try
            {
                var entity = GetCouponCategoryById(id);
                if (entity == null)
                    return false;
                _repository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return false;
            }
        }

        public int GetCouponNums(int categoryId)
        {
            try
            {
                return _couponService.Table.Count(c => c.CouponCategoryId == categoryId);
            }
            catch (Exception e)
            {
                _log.Error(e,"数据库操作出错");
                return -1;
            }
        }
    }
}