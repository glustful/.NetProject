using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.BrokerBill
{
	public class BrokerBillService : IBrokerBillService
	{
		private readonly Zerg.Common.Data.ICRMRepository<BrokerBillEntity> _brokerbillRepository;
		private readonly ILog _log;

		public BrokerBillService(Zerg.Common.Data.ICRMRepository<BrokerBillEntity> brokerbillRepository,ILog log)
		{
			_brokerbillRepository = brokerbillRepository;
			_log = log;
		}
		
		public BrokerBillEntity Create (BrokerBillEntity entity)
		{
			try
            {
                _brokerbillRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(BrokerBillEntity entity)
		{
			try
            {
                _brokerbillRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public BrokerBillEntity Update (BrokerBillEntity entity)
		{
			try
            {
                _brokerbillRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public BrokerBillEntity GetBrokerBillById (int id)
		{
			try
            {
                return _brokerbillRepository.GetById(id); ;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<BrokerBillEntity> GetBrokerBillsByCondition(BrokerBillSearchCondition condition)
		{
			var query = _brokerbillRepository.Table;
			try
			{
				if (condition.PaytimeBegin.HasValue)
                {
                    query = query.Where(q => q.Paytime>= condition.PaytimeBegin.Value);
                }
                if (condition.PaytimeEnd.HasValue)
                {
                    query = query.Where(q => q.Paytime < condition.PaytimeEnd.Value);
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
				if (condition.Billamount.HasValue)
                {
                    query = query.Where(q => q.Billamount == condition.Billamount.Value);
                }
				if (condition.Paidinamount.HasValue)
                {
                    query = query.Where(q => q.Paidinamount == condition.Paidinamount.Value);
                }
				if (!string.IsNullOrEmpty(condition.Cardnum))
                {
                    query = query.Where(q => q.Cardnum.Contains(condition.Cardnum));
                }
				if (!string.IsNullOrEmpty(condition.Merchantname))
                {
                    query = query.Where(q => q.Merchantname.Contains(condition.Merchantname));
                }
				if (!string.IsNullOrEmpty(condition.Payeeuser))
                {
                    query = query.Where(q => q.Payeeuser.Contains(condition.Payeeuser));
                }
				if (!string.IsNullOrEmpty(condition.Payeenum))
                {
                    query = query.Where(q => q.Payeenum.Contains(condition.Payeenum));
                }
				if (!string.IsNullOrEmpty(condition.Customername))
                {
                    query = query.Where(q => q.Customername.Contains(condition.Customername));
                }
				if (!string.IsNullOrEmpty(condition.Note))
                {
                    query = query.Where(q => q.Note.Contains(condition.Note));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Brokers != null && condition.Brokers.Any())
                {
                    query = query.Where(q => condition.Brokers.Contains(q.Broker));
                }
				if (condition.BillIds != null && condition.BillIds.Any())
                {
                    query = query.Where(q => condition.BillIds.Contains(q.BillId));
                }
				if (condition.Merchantids != null && condition.Merchantids.Any())
                {
                    query = query.Where(q => condition.Merchantids.Contains(q.Merchantid));
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
						case EnumBrokerBillSearchOrderBy.OrderById:
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

		public int GetBrokerBillCount (BrokerBillSearchCondition condition)
		{
			var query = _brokerbillRepository.Table;
			try
			{
				if (condition.PaytimeBegin.HasValue)
                {
                    query = query.Where(q => q.Paytime>= condition.PaytimeBegin.Value);
                }
                if (condition.PaytimeEnd.HasValue)
                {
                    query = query.Where(q => q.Paytime < condition.PaytimeEnd.Value);
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
				if (condition.Billamount.HasValue)
                {
                    query = query.Where(q => q.Billamount == condition.Billamount.Value);
                }
				if (condition.Paidinamount.HasValue)
                {
                    query = query.Where(q => q.Paidinamount == condition.Paidinamount.Value);
                }
				if (!string.IsNullOrEmpty(condition.Cardnum))
                {
                    query = query.Where(q => q.Cardnum.Contains(condition.Cardnum));
                }
				if (!string.IsNullOrEmpty(condition.Merchantname))
                {
                    query = query.Where(q => q.Merchantname.Contains(condition.Merchantname));
                }
				if (!string.IsNullOrEmpty(condition.Payeeuser))
                {
                    query = query.Where(q => q.Payeeuser.Contains(condition.Payeeuser));
                }
				if (!string.IsNullOrEmpty(condition.Payeenum))
                {
                    query = query.Where(q => q.Payeenum.Contains(condition.Payeenum));
                }
				if (!string.IsNullOrEmpty(condition.Customername))
                {
                    query = query.Where(q => q.Customername.Contains(condition.Customername));
                }
				if (!string.IsNullOrEmpty(condition.Note))
                {
                    query = query.Where(q => q.Note.Contains(condition.Note));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Brokers != null && condition.Brokers.Any())
                {
                    query = query.Where(q => condition.Brokers.Contains(q.Broker));
                }
				if (condition.BillIds != null && condition.BillIds.Any())
                {
                    query = query.Where(q => condition.BillIds.Contains(q.BillId));
                }
				if (condition.Merchantids != null && condition.Merchantids.Any())
                {
                    query = query.Where(q => condition.Merchantids.Contains(q.Merchantid));
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