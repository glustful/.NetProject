using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event.Entity.Entity.Coupon;
using Event.Entity.Entity.OtherCoupon;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Event.Service.OtherCoupon
{
    public class OtherCouponOwnerService : IOtherCouponOwnerService
    {
        private readonly IEventRepository<OtherCouponOwner> _repository;
        private readonly ILog _log;

        public OtherCouponOwnerService(IEventRepository<OtherCouponOwner> repository, ILog log)
        {
            _log = log;
            _repository = repository;
        }
        public OtherCouponOwner CreateRecord(int userId, int couponId)
        {
            try
            {
                var entity = new OtherCouponOwner
                {
                    CouponId = couponId,
                    UserId = userId
                };
                _repository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据操作错误");
                return null;
            }
        }

        public bool DeleteRecordById(int id)
        {
            try
            {
                var entity = _repository.GetById(id);
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

        public bool DeleteRecordByCouponId(int couponId)
        {
            try
            {
                var entity = _repository.Table.FirstOrDefault(c => c.CouponId == couponId);
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

        public IQueryable<OtherCouponOwner> GetCouponByUserId(int userid)
        {
            try
            {
                return _repository.Table.Where(u => u.UserId == userid);
            }
            catch (Exception e)
            {
                _log.Error(e, "获取用户失败");
                return null;
            }
        }

        public IQueryable<OtherCouponOwner> GetCouponOwnByCondition(OtherCouponOwnerSearchCondition condition)
        {
            var query = _repository.Table;
            try
            {
                if (condition.userId.HasValue)
                {
                    query = query.Where(q => q.UserId == condition.userId);
                }
                if (condition.couponId.HasValue)
                {
                    query = query.Where(q => q.CouponId == condition.couponId);
                }
                return query;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public IQueryable<OtherCouponOwner> GetCouponOwnCountCondition(CouponOwnerSearchCondition condition)
        {
            var query = _repository.Table;
            try
            {
                if (condition.userId.HasValue)
                {
                    query = query.Where(q => q.UserId == condition.userId);
                }
                if (condition.couponId.HasValue)
                {
                    query = query.Where(q => q.CouponId == condition.couponId);
                }
                return query;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }
    }
}
