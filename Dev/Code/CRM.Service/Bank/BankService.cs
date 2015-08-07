using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.Bank
{
    public class BankService : IBankService
    {
        private readonly Zerg.Common.Data.ICRMRepository<BankEntity> _bankRepository;
        private readonly ILog _log;

        public BankService(Zerg.Common.Data.ICRMRepository<BankEntity> bankRepository, ILog log)
        {
            _bankRepository = bankRepository;
            _log = log;
        }

        public BankEntity Create(BankEntity entity)
        {
            try
            {
                _bankRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public bool Delete(BankEntity entity)
        {
            try
            {
                _bankRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return false;
            }
        }

        public BankEntity Update(BankEntity entity)
        {
            try
            {
                _bankRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public BankEntity GetBankById(int id)
        {
            try
            {
                return _bankRepository.GetById(id); ;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public IQueryable<BankEntity> GetBanksByCondition(BankSearchCondition condition)
        {
            var query = _bankRepository.Table;
            try
            {
                if (!string.IsNullOrEmpty(condition.Codeid))
                {
                    query = query.Where(q => q.Codeid.Contains(condition.Codeid));
                }
               
                if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
                if (condition.OrderBy.HasValue)
                {
                    switch (condition.OrderBy.Value)
                    {
                        case EnumBankSearchOrderBy.OrderById:
                            query = condition.isDescending ? query.OrderByDescending(q => q.Id) : query.OrderBy(q => q.Id);
                            break;
                        case EnumBankSearchOrderBy.OrderByCodeid:
                            query = condition.isDescending ? query.OrderByDescending(q => q.Codeid) : query.OrderBy(q => q.Codeid);
                            break;
                        case EnumBankSearchOrderBy.OrderByAddtime:
                            query = condition.isDescending ? query.OrderByDescending(q => q.Addtime) : query.OrderBy(q => q.Addtime);
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

        public int GetBankCount(BankSearchCondition condition)
        {
            var query = _bankRepository.Table;
            try
            {
                if (!string.IsNullOrEmpty(condition.Codeid))
                {
                    query = query.Where(q => q.Codeid.Contains(condition.Codeid));
                }
               
                if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
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