﻿using System;
using System.Linq;
using Event.Entity.Entity.Coupon;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Event.Service.Coupon
{
    public class CouponOwnerService : ICouponOwnerService
    {
        private readonly IEventRepository<CouponOwner> _repository;
        private readonly ILog _log;

        public CouponOwnerService(IEventRepository<CouponOwner> repository, ILog log)
        {
            _log = log;
            _repository = repository;
        }
        public CouponOwner CreateRecord(int userId, int couponId)
        {
            try
            {
                var entity = new CouponOwner
                {
                    CouponId = couponId,
                    UserId = userId
                };
                _repository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e,"数据操作错误");
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
    }
}