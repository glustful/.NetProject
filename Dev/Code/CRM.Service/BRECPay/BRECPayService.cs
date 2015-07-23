using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.BRECPay
{
	public class BRECPayService : IBRECPayService
	{
        private readonly Zerg.Common.Data.ICRMRepository<BRECPayEntity> _brecpayRepository;
		private readonly ILog _log;

        public BRECPayService(Zerg.Common.Data.ICRMRepository<BRECPayEntity> brecpayRepository, ILog log)
		{
			_brecpayRepository = brecpayRepository;
			_log = log;
		}
		
		public BRECPayEntity Create (BRECPayEntity entity)
		{
			try
            {
                _brecpayRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(BRECPayEntity entity)
		{
			try
            {
                _brecpayRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public BRECPayEntity Update (BRECPayEntity entity)
		{
			try
            {
                _brecpayRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public BRECPayEntity GetBRECPayById (int id)
		{
			try
            {
                return _brecpayRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<BRECPayEntity> GetBRECPaysByCondition(BRECPaySearchCondition condition)
		{
			var query = _brecpayRepository.Table;
			try
			{
				if (condition.AmountBegin.HasValue)
                {
                    query = query.Where(q => q.Amount>= condition.AmountBegin.Value);
                }
                if (condition.AmountEnd.HasValue)
                {
                    query = query.Where(q => q.Amount < condition.AmountEnd.Value);
                }
				if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime>= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }
				if (condition.UptimeBegin.HasValue)
                {
                    query = query.Where(q => q.Uptime>= condition.UptimeBegin.Value);
                }
                if (condition.UptimeEnd.HasValue)
                {
                    query = query.Where(q => q.Uptime < condition.UptimeEnd.Value);
                }
				if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
                }
                if (condition.Statusname.HasValue)
                {
                    query = query.Where(c => c.Statusname == condition.Statusname);
                }
				if (!string.IsNullOrEmpty(condition.Describe))
                {
                    query = query.Where(q => q.Describe.Contains(condition.Describe));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.BrokerRECClients != null && condition.BrokerRECClients.Any())
                {
                    query = query.Where(q => condition.BrokerRECClients.Contains(q.BrokerRECClient));
                }
				if (condition.Accountantids != null && condition.Accountantids.Any())
                {
                    query = query.Where(q => condition.Accountantids.Contains(q.Accountantid));
                }
				if (condition.Addusers != null && condition.Addusers.Any())
                {
                    query = query.Where(q => condition.Addusers.Contains(q.Adduser));
                }
				if (condition.Upusers != null && condition.Upusers.Any())
                {
                    query = query.Where(q => condition.Upusers.Contains(q.Upuser));
                }
                if (!string.IsNullOrEmpty(condition.BankCard))
                {
                    query = query.Where(c => c.BankCard.Contains(condition.BankCard));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumBRECPaySearchOrderBy.OrderById:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
                    }
					
				}
				else
				{
					query = query.OrderBy(q=>q.Id);
				}

				if (condition.Page.HasValue && condition.PageCount.HasValue)
                {
                    query = query.Skip((condition.Page.Value - 1)*condition.PageCount.Value).Take(condition.PageCount.Value);
                }

				return query;
			}
			catch(Exception e)
			{
				_log.Error(e,"数据库操作出错");
                return null;
			}
		}

		public int GetBRECPayCount (BRECPaySearchCondition condition)
		{
			var query = _brecpayRepository.Table;
			try
			{
				if (condition.AmountBegin.HasValue)
                {
                    query = query.Where(q => q.Amount>= condition.AmountBegin.Value);
                }
                if (condition.AmountEnd.HasValue)
                {
                    query = query.Where(q => q.Amount < condition.AmountEnd.Value);
                }
				if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime>= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }
				if (condition.UptimeBegin.HasValue)
                {
                    query = query.Where(q => q.Uptime>= condition.UptimeBegin.Value);
                }
                if (condition.UptimeEnd.HasValue)
                {
                    query = query.Where(q => q.Uptime < condition.UptimeEnd.Value);
                }
				if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
                }
                if (condition.Statusname.HasValue)
                {
                    query = query.Where(c => c.Statusname == condition.Statusname);
                }
				if (!string.IsNullOrEmpty(condition.Describe))
                {
                    query = query.Where(q => q.Describe.Contains(condition.Describe));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.BrokerRECClients != null && condition.BrokerRECClients.Any())
                {
                    query = query.Where(q => condition.BrokerRECClients.Contains(q.BrokerRECClient));
                }
				if (condition.Accountantids != null && condition.Accountantids.Any())
                {
                    query = query.Where(q => condition.Accountantids.Contains(q.Accountantid));
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
			catch(Exception e)
			{
				_log.Error(e,"数据库操作出错");
                return -1;
			}
		}
	}
}