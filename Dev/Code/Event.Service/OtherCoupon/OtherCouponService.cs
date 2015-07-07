using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event.Entity.Entity.OtherCoupon;
using YooPoon.Core.Logging;

namespace Event.Service.OtherCoupon
{
    public class OtherCouponService : IOtherCouponService
    {
        private readonly Zerg.Common.Data.IEventRepository<Entity.Entity.OtherCoupon.OtherCoupon> _couponRepository;
        private readonly ILog _log;
        public OtherCouponService(Zerg.Common.Data.IEventRepository<Entity.Entity.OtherCoupon.OtherCoupon> couponRepository, ILog log)
        {
            _couponRepository = couponRepository;
            _log = log;
        }
        public Entity.Entity.OtherCoupon.OtherCoupon Create(Entity.Entity.OtherCoupon.OtherCoupon entity)
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

        public bool Delete(Entity.Entity.OtherCoupon.OtherCoupon entity)
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

        public Entity.Entity.OtherCoupon.OtherCoupon Update(Entity.Entity.OtherCoupon.OtherCoupon entity)
        {
            try
            {
                _couponRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public Entity.Entity.OtherCoupon.OtherCoupon GetCouponById(int id)
        {
            try
            {
                return _couponRepository.GetById(id);
            }
            catch (Exception e)
            {
                _log.Error(e, "操作数据库出错");
                return null;
            }
        }

        public IQueryable<Entity.Entity.OtherCoupon.OtherCoupon> GetCouponByCondition(OtherCouponSearchCondition condition)
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
                    query = query.Where(q => (EnumOtherCouponStatus)condition.Status == q.Status);
                }
                if (condition.OrderBy.HasValue)
                {
                    switch (condition.OrderBy.Value)
                    {
                        case EnumOtherCouponSearchOrderBy.OrderById:
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

        public int GetCouponCount(OtherCouponSearchCondition condition)
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
                    query = query.Where(q => (EnumOtherCouponStatus)condition.Status == q.Status);
                }
                return query.Count();
            }

            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return -1;
            }
        }

        public Entity.Entity.OtherCoupon.OtherCoupon GetCouponByNumber(string number)
        {
            try
            {
                return _couponRepository.Table.FirstOrDefault(c => c.Number == number);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public bool BulkCreate(List<Entity.Entity.OtherCoupon.OtherCoupon> entities)
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
