using System;
using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

//by yangyue   2015/7/20   活动订单---------//
namespace CRM.Service.InvitedCode
{
    public class InviteCodeService : IInviteCodeService
    {
        private readonly ICRMRepository<InviteCodeEntity> _invitecodeRepository;
        private readonly ILog _log;

        public InviteCodeService(ICRMRepository<InviteCodeEntity> invitecodeRepository, ILog log)
        {
            _invitecodeRepository = invitecodeRepository;
            _log = log;
        }

        public InviteCodeEntity Create(InviteCodeEntity entity)
        {
            try
            {
                _invitecodeRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public bool Delete(InviteCodeEntity entity)
        {
            try
            {
                _invitecodeRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return false;
            }
        }

        public InviteCodeEntity Update(InviteCodeEntity entity)
        {
            try
            {
                _invitecodeRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public InviteCodeEntity GetInviteCodeById(int id)
        {
            try
            {
                return _invitecodeRepository.GetById(id); ;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }
        public InviteCodeEntity GetInviteCodeByBrokerId(BrokerEntity broker)
        {
            try
            {
                return _invitecodeRepository.GetById(broker); ;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }
      
        public IQueryable<InviteCodeEntity> GetInviteCodeByCondition(InviteCodeSearchCondition condition)
        {
            var query = _invitecodeRepository.Table;
            try
            {
                if (condition.NumUser!=null)
                {
                    query = query.Where(q => q.NumUser >= condition.NumUser);
                }
                if (condition.UseTime.HasValue)
                {
                    query = query.Where(q => q.UseTime >= condition.UseTime.Value);
                }
                if (condition.Brokers != null)
                {
                    query = query.Where(q => q.Broker.Id== condition.Brokers.Id);
                }
                if (condition.CreatTime.HasValue)
                {
                    query = query.Where(q => q.CreatTime < condition.CreatTime.Value);
                }
                if (!string.IsNullOrEmpty(condition.Number))
                {
                    query = query.Where(q => q.Number.Contains(condition.Number));
                }
                if (condition.BrokerId != null)
                {
                    query = query.Where(q => q.Broker.Id==condition.BrokerId);
                }

                return query;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }

        }

        public int GetInviteCodeByCount(InviteCodeSearchCondition condition)
        {
            var query = _invitecodeRepository.Table;
            try
            {
                if (condition.UseTime.HasValue)
                {
                    query = query.Where(q => q.UseTime >= condition.UseTime.Value);
                }
                if (condition.Brokers != null)
                {
                    query = query.Where(q => q.Broker == condition.Brokers);
                }
                if (condition.CreatTime.HasValue)
                {
                    query = query.Where(q => q.CreatTime < condition.CreatTime.Value);
                }
                if (!string.IsNullOrEmpty(condition.Number))
                {
                    query = query.Where(q => q.Number.Contains(condition.Number));
                }
                if ((condition.NumUser) != null)
                {
                    query = query.Where(q => q.NumUser == condition.NumUser);
                }

                return query.Count();
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return -1;
            }

        }

        public InviteCodeEntity GetInviteCodebyBrokerId(int broker)
        {
            try
            {
                return _invitecodeRepository.Table.FirstOrDefault(p =>broker == p.Broker.Id );
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }
    }
}
