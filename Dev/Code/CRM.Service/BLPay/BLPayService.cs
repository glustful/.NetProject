using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.BLPay
{
	public class BLPayService : IBLPayService
	{
		private readonly Zerg.Common.Data.ICRMRepository<BLPayEntity> _blpayRepository;
		private readonly ILog _log;

		public BLPayService(Zerg.Common.Data.ICRMRepository<BLPayEntity> blpayRepository,ILog log)
		{
			_blpayRepository = blpayRepository;
			_log = log;
		}
		
		public BLPayEntity Create (BLPayEntity entity)
		{
			try
            {
                _blpayRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(BLPayEntity entity)
		{
			try
            {
                _blpayRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public BLPayEntity Update (BLPayEntity entity)
		{
			try
            {
                _blpayRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public BLPayEntity GetBLPayById (int id)
		{
			try
            {
                return _blpayRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<BLPayEntity> GetBLPaysByCondition(BLPaySearchCondition condition)
		{
			var query = _blpayRepository.Table;
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
				if (!string.IsNullOrEmpty(condition.Statusname))
                {
                    query = query.Where(q => q.Statusname.Contains(condition.Statusname));
                }
				if (!string.IsNullOrEmpty(condition.Describe))
                {
                    query = query.Where(q => q.Describe.Contains(condition.Describe));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.BrokerLeadClients != null && condition.BrokerLeadClients.Any())
                {
                    query = query.Where(q => condition.BrokerLeadClients.Contains(q.BrokerLeadClient));
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
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumBLPaySearchOrderBy.OrderById:
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

		public int GetBLPayCount (BLPaySearchCondition condition)
		{
			var query = _blpayRepository.Table;
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
				if (!string.IsNullOrEmpty(condition.Statusname))
                {
                    query = query.Where(q => q.Statusname.Contains(condition.Statusname));
                }
				if (!string.IsNullOrEmpty(condition.Describe))
                {
                    query = query.Where(q => q.Describe.Contains(condition.Describe));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.BrokerLeadClients != null && condition.BrokerLeadClients.Any())
                {
                    query = query.Where(q => condition.BrokerLeadClients.Contains(q.BrokerLeadClient));
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