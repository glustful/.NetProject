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

        public CouponCategoryService(IEventRepository<CouponCategory> repository,ILog log)
        {
            _log = log;
            _repository = repository;
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
            if (condition.Page.HasValue && condition.PageSize.HasValue)
            {
                query = query.Skip((condition.Page.Value - 1) * condition.PageSize.Value).Take(condition.PageSize.Value);
            }
            return query;
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
            throw new System.NotImplementedException();
        }
    }
}