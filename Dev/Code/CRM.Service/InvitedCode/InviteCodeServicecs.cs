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
        

        public IQueryable<InviteCodeEntity> GetInviteCodeByCondition(InviteCodeSearchCondition condition)
        {
            var query = _invitecodeRepository.Table;
            try
            {
                if (condition.UseTime.HasValue)
                {
                    query = query.Where(q => q.UseTime >= condition.UseTime.Value);
                }
                if (condition.CreatTime.HasValue)
                {
                    query = query.Where(q => q.CreatTime < condition.CreatTime.Value);
                }
                if (!string.IsNullOrEmpty(condition.Number))
                {
                    query = query.Where(q => q.Number.Contains(condition.Number));
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
