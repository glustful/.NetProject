using System;
using System.Linq;
using Trading.Entity.Entity.CommissionRatio;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Trading.Service.CommissionRatio
{
    public class CommissionRatioService:ICommissionRatioService
    {
        private readonly ITradingRepository<CommissionRatioEntity> _commissionRatioRepository;
        private readonly ILog _log;

        public CommissionRatioService(ITradingRepository<CommissionRatioEntity> commissionRatioRepository,ILog log)
        {
            _commissionRatioRepository = commissionRatioRepository;
            _log = log;
        }
        public CommissionRatioEntity Create(CommissionRatioEntity entity)
        {
            try
            {
                _commissionRatioRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e,"数据库操作出错");
                return null;
            }
        }

        public CommissionRatioEntity Update(CommissionRatioEntity entity)
        {
            try
            {
                _commissionRatioRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e,"数据库操作出错");
                return null;
            }
        }

        public bool Delete(CommissionRatioEntity entity)
        {
            try
            {
                _commissionRatioRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e,"数据库操作出错");
                return false;
            }
        }

        public CommissionRatioEntity GetById(int id)
        {
            try
            {
                return _commissionRatioRepository.GetById(id);
            }
            catch (Exception e)
            {
                _log.Error(e,"数据库操作出错");
                throw;
            }
        }

        public IQueryable<CommissionRatioEntity> GetCommissionRatioCondition(CommissionRatioSearchCondition condition)
        {
            var query = _commissionRatioRepository.Table;
            try
            {
                if (condition.Id != null && condition.Id.Any())
                {
                    query = query.Where(q => condition.Id.Contains(q.Id));
                }

                if (condition.OrderBy.HasValue)
                {
                    switch (condition.OrderBy.Value)
                    {
                        case EnumCommissionSearchOrderBy.OrderById:
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
                    query =
                        query.Skip((condition.Page.Value - 1)*condition.PageSize.Value).Take(condition.PageSize.Value);
                }
                return query;
            }
            catch (Exception e)
            {
                _log.Error(e,"数据库操作出错");
                return null;
            }
        }

        public int GetCommissionRatioCount(CommissionRatioSearchCondition condition)
        {
            var query = _commissionRatioRepository.Table;
            try
            {
                if (condition.Id != null && condition.Id.Any())
                {
                    query = query.Where(q => condition.Id.Contains(q.Id));
                }
                if (condition.Page.HasValue && condition.PageSize.HasValue)
                {
                    query =
                        query.Skip((condition.Page.Value - 1) * condition.PageSize.Value).Take(condition.PageSize.Value);
                }
                return query.Count();
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return -1;
            }
        }
    }
}
