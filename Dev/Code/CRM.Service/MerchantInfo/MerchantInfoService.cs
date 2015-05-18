using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.MerchantInfo
{
	public class MerchantInfoService : IMerchantInfoService
	{
		private readonly Zerg.Common.Data.ICRMRepository<MerchantInfoEntity> _merchantinfoRepository;
		private readonly ILog _log;

		public MerchantInfoService(Zerg.Common.Data.ICRMRepository<MerchantInfoEntity> merchantinfoRepository,ILog log)
		{
			_merchantinfoRepository = merchantinfoRepository;
			_log = log;
		}
		
		public MerchantInfoEntity Create (MerchantInfoEntity entity)
		{
			try
            {
                _merchantinfoRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(MerchantInfoEntity entity)
		{
			try
            {
                _merchantinfoRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public MerchantInfoEntity Update (MerchantInfoEntity entity)
		{
			try
            {
                _merchantinfoRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public MerchantInfoEntity GetMerchantInfoById (int id)
		{
			try
            {
                return _merchantinfoRepository.GetById(id); ;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<MerchantInfoEntity> GetMerchantInfosByCondition(MerchantInfoSearchCondition condition)
		{
			var query = _merchantinfoRepository.Table;
			try
			{
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
				if (!string.IsNullOrEmpty(condition.Merchantname))
                {
                    query = query.Where(q => q.Merchantname.Contains(condition.Merchantname));
                }
				if (!string.IsNullOrEmpty(condition.Mail))
                {
                    query = query.Where(q => q.Mail.Contains(condition.Mail));
                }
				if (!string.IsNullOrEmpty(condition.Address))
                {
                    query = query.Where(q => q.Address.Contains(condition.Address));
                }
				if (!string.IsNullOrEmpty(condition.Phone))
                {
                    query = query.Where(q => q.Phone.Contains(condition.Phone));
                }
				if (!string.IsNullOrEmpty(condition.License))
                {
                    query = query.Where(q => q.License.Contains(condition.License));
                }
				if (!string.IsNullOrEmpty(condition.Legalhuman))
                {
                    query = query.Where(q => q.Legalhuman.Contains(condition.Legalhuman));
                }
				if (!string.IsNullOrEmpty(condition.Legalsfz))
                {
                    query = query.Where(q => q.Legalsfz.Contains(condition.Legalsfz));
                }
				if (!string.IsNullOrEmpty(condition.Orgnum))
                {
                    query = query.Where(q => q.Orgnum.Contains(condition.Orgnum));
                }
				if (!string.IsNullOrEmpty(condition.Taxnum))
                {
                    query = query.Where(q => q.Taxnum.Contains(condition.Taxnum));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.UserIds != null && condition.UserIds.Any())
                {
                    query = query.Where(q => condition.UserIds.Contains(q.UserId));
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
						case EnumMerchantInfoSearchOrderBy.OrderById:
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

		public int GetMerchantInfoCount (MerchantInfoSearchCondition condition)
		{
			var query = _merchantinfoRepository.Table;
			try
			{
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
				if (!string.IsNullOrEmpty(condition.Merchantname))
                {
                    query = query.Where(q => q.Merchantname.Contains(condition.Merchantname));
                }
				if (!string.IsNullOrEmpty(condition.Mail))
                {
                    query = query.Where(q => q.Mail.Contains(condition.Mail));
                }
				if (!string.IsNullOrEmpty(condition.Address))
                {
                    query = query.Where(q => q.Address.Contains(condition.Address));
                }
				if (!string.IsNullOrEmpty(condition.Phone))
                {
                    query = query.Where(q => q.Phone.Contains(condition.Phone));
                }
				if (!string.IsNullOrEmpty(condition.License))
                {
                    query = query.Where(q => q.License.Contains(condition.License));
                }
				if (!string.IsNullOrEmpty(condition.Legalhuman))
                {
                    query = query.Where(q => q.Legalhuman.Contains(condition.Legalhuman));
                }
				if (!string.IsNullOrEmpty(condition.Legalsfz))
                {
                    query = query.Where(q => q.Legalsfz.Contains(condition.Legalsfz));
                }
				if (!string.IsNullOrEmpty(condition.Orgnum))
                {
                    query = query.Where(q => q.Orgnum.Contains(condition.Orgnum));
                }
				if (!string.IsNullOrEmpty(condition.Taxnum))
                {
                    query = query.Where(q => q.Taxnum.Contains(condition.Taxnum));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.UserIds != null && condition.UserIds.Any())
                {
                    query = query.Where(q => condition.UserIds.Contains(q.UserId));
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