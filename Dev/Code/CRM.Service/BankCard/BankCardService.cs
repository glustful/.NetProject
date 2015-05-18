using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.BankCard
{
	public class BankCardService : IBankCardService
	{
		private readonly Zerg.Common.Data.ICRMRepository<BankCardEntity> _bankcardRepository;
		private readonly ILog _log;

		public BankCardService(Zerg.Common.Data.ICRMRepository<BankCardEntity> bankcardRepository,ILog log)
		{
			_bankcardRepository = bankcardRepository;
			_log = log;
		}
		
		public BankCardEntity Create (BankCardEntity entity)
		{
			try
            {
                _bankcardRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(BankCardEntity entity)
		{
			try
            {
                _bankcardRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public BankCardEntity Update (BankCardEntity entity)
		{
			try
            {
                _bankcardRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public BankCardEntity GetBankCardById (int id)
		{
			try
            {
                return _bankcardRepository.GetById(id); ;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<BankCardEntity> GetBankCardsByCondition(BankCardSearchCondition condition)
		{
			var query = _bankcardRepository.Table;
			try
			{
				if (condition.DeadlineBegin.HasValue)
                {
                    query = query.Where(q => q.Deadline>= condition.DeadlineBegin.Value);
                }
                if (condition.DeadlineEnd.HasValue)
                {
                    query = query.Where(q => q.Deadline < condition.DeadlineEnd.Value);
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
				if (condition.Type.HasValue)
                {
                    query = query.Where(q => q.Type == condition.Type.Value);
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Banks != null)
                {
                    query = query.Where(q => condition.Banks==q.Bank);
                }
				if (condition.Brokers != null)
                {
                    query = query.Where(q => (q.Broker.Id == condition.Brokers.Id));
                }
				if (condition.Nums != null && condition.Nums.Any())
                {
                    query = query.Where(q => condition.Nums.Contains(q.Num));
                }
				if (condition.Addusers != null && condition.Addusers.Any())
                {
                    query = query.Where(q => condition.Addusers.Contains(q.Adduser));
                }
				if (condition.Upusers != null && condition.Upusers.Any())
                {
                    query = query.Where(q => condition.Upusers.Contains(q.Upuser));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumBankCardSearchOrderBy.OrderById:
							query = condition.isDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
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

		public int GetBankCardCount (BankCardSearchCondition condition)
		{
			var query = _bankcardRepository.Table;
			try
			{
				if (condition.DeadlineBegin.HasValue)
                {
                    query = query.Where(q => q.Deadline>= condition.DeadlineBegin.Value);
                }
                if (condition.DeadlineEnd.HasValue)
                {
                    query = query.Where(q => q.Deadline < condition.DeadlineEnd.Value);
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
				if (condition.Type.HasValue)
                {
                    query = query.Where(q => q.Type == condition.Type.Value);
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Banks != null )
                {
                    query = query.Where(q => condition.Banks==(q.Bank));
                }
				if (condition.Brokers != null )
                {
                    query = query.Where(q => condition.Brokers==(q.Broker));
                }
				if (condition.Nums != null && condition.Nums.Any())
                {
                    query = query.Where(q => condition.Nums.Contains(q.Num));
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