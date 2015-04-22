using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.MerchantClient
{
	public class MerchantClientService : IMerchantClientService
	{
		private readonly IRepository<MerchantClientEntity> _merchantclientRepository;
		private readonly ILog _log;

		public MerchantClientService(IRepository<MerchantClientEntity> merchantclientRepository,ILog log)
		{
			_merchantclientRepository = merchantclientRepository;
			_log = log;
		}
		
		public MerchantClientEntity Create (MerchantClientEntity entity)
		{
			try
            {
                _merchantclientRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(MerchantClientEntity entity)
		{
			try
            {
                _merchantclientRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public MerchantClientEntity Update (MerchantClientEntity entity)
		{
			try
            {
                _merchantclientRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public MerchantClientEntity GetMerchantClientById (int id)
		{
			try
            {
                return _merchantclientRepository.GetById(id); ;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<MerchantClientEntity> GetMerchantClientsByCondition(MerchantClientSearchCondition condition)
		{
			var query = _merchantclientRepository.Table;
			try
			{
				if (condition.AppointmenttimeBegin.HasValue)
                {
                    query = query.Where(q => q.Appointmenttime>= condition.AppointmenttimeBegin.Value);
                }
                if (condition.AppointmenttimeEnd.HasValue)
                {
                    query = query.Where(q => q.Appointmenttime < condition.AppointmenttimeEnd.Value);
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
				if (!string.IsNullOrEmpty(condition.Appointmentstatus))
                {
                    query = query.Where(q => q.Appointmentstatus.Contains(condition.Appointmentstatus));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.MerchantInfos != null && condition.MerchantInfos.Any())
                {
                    query = query.Where(q => condition.MerchantInfos.Contains(q.MerchantInfo));
                }
				if (condition.ClientInfos != null && condition.ClientInfos.Any())
                {
                    query = query.Where(q => condition.ClientInfos.Contains(q.ClientInfo));
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
						case EnumMerchantClientSearchOrderBy.OrderById:
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

		public int GetMerchantClientCount (MerchantClientSearchCondition condition)
		{
			var query = _merchantclientRepository.Table;
			try
			{
				if (condition.AppointmenttimeBegin.HasValue)
                {
                    query = query.Where(q => q.Appointmenttime>= condition.AppointmenttimeBegin.Value);
                }
                if (condition.AppointmenttimeEnd.HasValue)
                {
                    query = query.Where(q => q.Appointmenttime < condition.AppointmenttimeEnd.Value);
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
				if (!string.IsNullOrEmpty(condition.Appointmentstatus))
                {
                    query = query.Where(q => q.Appointmentstatus.Contains(condition.Appointmentstatus));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.MerchantInfos != null && condition.MerchantInfos.Any())
                {
                    query = query.Where(q => condition.MerchantInfos.Contains(q.MerchantInfo));
                }
				if (condition.ClientInfos != null && condition.ClientInfos.Any())
                {
                    query = query.Where(q => condition.ClientInfos.Contains(q.ClientInfo));
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