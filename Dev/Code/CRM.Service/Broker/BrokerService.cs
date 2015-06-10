using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.Broker
{
    public class BrokerService : IBrokerService
    {
        private readonly Zerg.Common.Data.ICRMRepository<BrokerEntity> _brokerRepository;
        private readonly ILog _log;

        public BrokerService(Zerg.Common.Data.ICRMRepository<BrokerEntity> brokerRepository, ILog log)
        {
            _brokerRepository = brokerRepository;
            _log = log;
        }

        public BrokerEntity Create(BrokerEntity entity)
        {
            try
            {
                _brokerRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public bool Delete(BrokerEntity entity)
        {
            try
            {
                _brokerRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return false;
            }
        }

        public BrokerEntity Update(BrokerEntity entity)
        {
            try
            {
                _brokerRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public BrokerEntity GetBrokerById(int id)
        {
            try
            {
                return _brokerRepository.GetById(id); ;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public IQueryable<BrokerEntity> GetBrokersByCondition(BrokerSearchCondition condition)
        {
            var query = _brokerRepository.Table;
            try
            {
                if (condition.RegtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Regtime >= condition.RegtimeBegin.Value);
                }
                if (condition.RegtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Regtime < condition.RegtimeEnd.Value);
                }
                if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime >= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }
                if (condition.UptimeBegin.HasValue)
                {
                    query = query.Where(q => q.Uptime >= condition.UptimeBegin.Value);
                }
                if (condition.UptimeEnd.HasValue)
                {
                    query = query.Where(q => q.Uptime < condition.UptimeEnd.Value);
                }
                if (condition.Amount.HasValue)
                {
                    query = query.Where(q => q.Amount == condition.Amount.Value);
                }
                if (condition.Delflag.HasValue)
                {
                    query = query.Where(q => q.State == condition.Delflag.Value);
                }
                if (!string.IsNullOrEmpty(condition.Brokername))
                {
                    query = query.Where(q => q.Brokername.Contains(condition.Brokername));
                }
                if (!string.IsNullOrEmpty(condition.Nickname))
                {
                    query = query.Where(q => q.Nickname.Contains(condition.Nickname));
                }
                if (!string.IsNullOrEmpty(condition.Realname))
                {
                    query = query.Where(q => q.Realname.Contains(condition.Realname));
                }
                if (!string.IsNullOrEmpty(condition.Sfz))
                {
                    query = query.Where(q => q.Sfz.Contains(condition.Sfz));
                }
                if (!string.IsNullOrEmpty(condition.Sexy))
                {
                    query = query.Where(q => q.Sexy.Contains(condition.Sexy));
                }
                if (!string.IsNullOrEmpty(condition.Phone))
                {
                    query = query.Where(q => q.Phone.Contains(condition.Phone));
                }
                if (!string.IsNullOrEmpty(condition.Email))
                {
                    query = query.Where(q => q.Email.Contains(condition.Email));
                }
                if (!string.IsNullOrEmpty(condition.Headphoto))
                {
                    query = query.Where(q => q.Headphoto.Contains(condition.Headphoto));
                }
                if (!string.IsNullOrEmpty(condition.Agentlevel))
                {
                    query = query.Where(q => q.Agentlevel.Contains(condition.Agentlevel));
                }
                if (!string.IsNullOrEmpty(condition.Address))
                {
                    query = query.Where(q => q.Address.Contains(condition.Address));
                }
                if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
                if (condition.Levels != null && condition.Levels.Any())
                {
                    query = query.Where(q => condition.Levels.Contains(q.Level));
                }
                if (condition.UserIds != null && condition.UserIds.Any())
                {
                    query = query.Where(q => condition.UserIds.Contains(q.UserId));
                }

                if (condition.Qqs != null && condition.Qqs.Any())
                {
                    query = query.Where(q => condition.Qqs.Contains(q.Qq));
                }
                if (condition.Zips != null && condition.Zips.Any())
                {
                    query = query.Where(q => condition.Zips.Contains(q.Zip));
                }
                if (condition.Totalpointss != null && condition.Totalpointss.Any())
                {
                    query = query.Where(q => condition.Totalpointss.Contains(q.Totalpoints));
                }
                if (condition.Addusers != null && condition.Addusers.Any())
                {
                    query = query.Where(q => condition.Addusers.Contains(q.Adduser));
                }
                if (condition.Upusers != null && condition.Upusers.Any())
                {
                    query = query.Where(q => condition.Upusers.Contains(q.Upuser));
                }
                if (condition.OrderBy.HasValue)
                {
                    switch (condition.OrderBy.Value)
                    {
                        case EnumBrokerSearchOrderBy.OrderById:
                            query = condition.isDescending ? query.OrderByDescending(q => q.Id) : query.OrderBy(q => q.Id);
                            break;
                    }

                }
                else
                {
                    query = query.OrderBy(q => q.Id);
                }

                if (condition.UserType.HasValue)
                {
                    query = query.Where(c => c.Usertype == condition.UserType);
                }
                else
                {
                    query = query.OrderBy(q => q.Usertype);
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

        public int GetBrokerCount(BrokerSearchCondition condition)
        {
            var query = _brokerRepository.Table;
            try
            {
                if (condition.RegtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Regtime >= condition.RegtimeBegin.Value);
                }
                if (condition.RegtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Regtime < condition.RegtimeEnd.Value);
                }
                if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime >= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }
                if (condition.UptimeBegin.HasValue)
                {
                    query = query.Where(q => q.Uptime >= condition.UptimeBegin.Value);
                }
                if (condition.UptimeEnd.HasValue)
                {
                    query = query.Where(q => q.Uptime < condition.UptimeEnd.Value);
                }
                if (condition.Amount.HasValue)
                {
                    query = query.Where(q => q.Amount == condition.Amount.Value);
                }
                if (condition.Delflag.HasValue)
                {
                    query = query.Where(q => q.State == condition.Delflag.Value);
                }
                if (!string.IsNullOrEmpty(condition.Brokername))
                {
                    query = query.Where(q => q.Brokername.Contains(condition.Brokername));
                }
                if (!string.IsNullOrEmpty(condition.Nickname))
                {
                    query = query.Where(q => q.Nickname.Contains(condition.Nickname));
                }
                if (!string.IsNullOrEmpty(condition.Realname))
                {
                    query = query.Where(q => q.Realname.Contains(condition.Realname));
                }
                if (!string.IsNullOrEmpty(condition.Sfz))
                {
                    query = query.Where(q => q.Sfz.Contains(condition.Sfz));
                }
                if (!string.IsNullOrEmpty(condition.Sexy))
                {
                    query = query.Where(q => q.Sexy.Contains(condition.Sexy));
                }
                if (!string.IsNullOrEmpty(condition.Phone))
                {
                    query = query.Where(q => q.Phone.Contains(condition.Phone));
                }
                if (!string.IsNullOrEmpty(condition.Headphoto))
                {
                    query = query.Where(q => q.Headphoto.Contains(condition.Headphoto));
                }
                if (!string.IsNullOrEmpty(condition.Agentlevel))
                {
                    query = query.Where(q => q.Agentlevel.Contains(condition.Agentlevel));
                }
                //if (!string.IsNullOrEmpty(condition.Usertype))
                //{
                //    query = query.Where(q => q.Usertype.Contains(condition.Usertype));
                //}
                if (!string.IsNullOrEmpty(condition.Address))
                {
                    query = query.Where(q => q.Address.Contains(condition.Address));
                }
                if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
                if (condition.Levels != null && condition.Levels.Any())
                {
                    query = query.Where(q => condition.Levels.Contains(q.Level));
                }
                if (condition.UserIds != null && condition.UserIds.Any())
                {
                    query = query.Where(q => condition.UserIds.Contains(q.UserId));
                }
                if (condition.Qqs != null && condition.Qqs.Any())
                {
                    query = query.Where(q => condition.Qqs.Contains(q.Qq));
                }
                if (condition.Zips != null && condition.Zips.Any())
                {
                    query = query.Where(q => condition.Zips.Contains(q.Zip));
                }
                if (condition.Totalpointss != null && condition.Totalpointss.Any())
                {
                    query = query.Where(q => condition.Totalpointss.Contains(q.Totalpoints));
                }
                if (condition.Addusers != null && condition.Addusers.Any())
                {
                    query = query.Where(q => condition.Addusers.Contains(q.Adduser));
                }
                if (condition.Upusers != null && condition.Upusers.Any())
                {
                    query = query.Where(q => condition.Upusers.Contains(q.Upuser));
                }
                return query.Count();
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return -1;
            }
        }


        /// <summary>
        /// 经纪人排行top10
        /// </summary>
        /// <returns></returns>
        public IQueryable<BrokerEntity> OrderbyBrokersList()
        {
            var query = _brokerRepository.Table;
            return query.Where(o => o.Amount>0).OrderByDescending(o => o.Amount).Take(10);
        }


        /// <summary>
        /// 所有经纪人排行
        /// </summary>
        /// <returns></returns>
        public IQueryable<BrokerEntity> OrderbyAllBrokersList()
        {
            var query = _brokerRepository.Table;
            return query.Where(o => o.Amount > 0).OrderByDescending(o => o.Amount);
        }

        public BrokerEntity GetBrokerByUserId(int userId)
        {
            try
            {
                return _brokerRepository.Table.FirstOrDefault(p => p.UserId == userId);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }
    }
}