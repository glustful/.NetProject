using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event.Entity.Entity.OtherCoupon;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Event.Service.OtherCoupon
{
    public class OtherCouponCategoryService : IOtherCouponCategoryService
    {
        private readonly IEventRepository<OtherCouponCategory> _repository;
        private readonly ILog _log;
        private readonly IEventRepository<Entity.Entity.OtherCoupon.OtherCoupon> _couponService;

        public OtherCouponCategoryService(IEventRepository<OtherCouponCategory> repository, ILog log, IEventRepository<Entity.Entity.OtherCoupon.OtherCoupon> couponService)
        {
            _log = log;
            _repository = repository;
            _couponService = couponService;
        }
        public bool CreateCouponCategory(OtherCouponCategory entity)
        {
            try
            {
                _repository.Insert(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return false;
            }
        }

        public IQueryable<OtherCouponCategory> GetCouponCategoriesByCondition(OtherCouponCategorySearchCondition condition)
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

                    case EnumOtherCouponCategorySearchOrderBy.OrderById:
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

        public int GetCouponCategoriesCountByCondition(OtherCouponCategorySearchCondition condition)
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

        public OtherCouponCategory GetCouponCategoryById(int id)
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

        public bool UpdateCouponCategory(OtherCouponCategory entity)
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
                _log.Error(e, "数据库操作出错");
                return -1;
            }
        }
    }
}
