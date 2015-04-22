using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.ClientInfo
{
	public class ClientInfoService : IClientInfoService
	{
		private readonly IRepository<ClientInfoEntity> _clientinfoRepository;
		private readonly ILog _log;

		public ClientInfoService(IRepository<ClientInfoEntity> clientinfoRepository,ILog log)
		{
			_clientinfoRepository = clientinfoRepository;
			_log = log;
		}
		
		public ClientInfoEntity Create (ClientInfoEntity entity)
		{
			try
            {
                _clientinfoRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(ClientInfoEntity entity)
		{
			try
            {
                _clientinfoRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public ClientInfoEntity Update (ClientInfoEntity entity)
		{
			try
            {
                _clientinfoRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public ClientInfoEntity GetClientInfoById (int id)
		{
			try
            {
                return _clientinfoRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<ClientInfoEntity> GetClientInfosByCondition(ClientInfoSearchCondition condition)
		{
			var query = _clientinfoRepository.Table;
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
				if (!string.IsNullOrEmpty(condition.Clientname))
                {
                    query = query.Where(q => q.Clientname.Contains(condition.Clientname));
                }
				if (!string.IsNullOrEmpty(condition.Phone))
                {
                    query = query.Where(q => q.Phone.Contains(condition.Phone));
                }
				if (!string.IsNullOrEmpty(condition.Housetype))
                {
                    query = query.Where(q => q.Housetype.Contains(condition.Housetype));
                }
				if (!string.IsNullOrEmpty(condition.Houses))
                {
                    query = query.Where(q => q.Houses.Contains(condition.Houses));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
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
						case EnumClientInfoSearchOrderBy.OrderById:
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

		public int GetClientInfoCount (ClientInfoSearchCondition condition)
		{
			var query = _clientinfoRepository.Table;
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
				if (!string.IsNullOrEmpty(condition.Clientname))
                {
                    query = query.Where(q => q.Clientname.Contains(condition.Clientname));
                }
				if (!string.IsNullOrEmpty(condition.Phone))
                {
                    query = query.Where(q => q.Phone.Contains(condition.Phone));
                }
				if (!string.IsNullOrEmpty(condition.Housetype))
                {
                    query = query.Where(q => q.Housetype.Contains(condition.Housetype));
                }
				if (!string.IsNullOrEmpty(condition.Houses))
                {
                    query = query.Where(q => q.Houses.Contains(condition.Houses));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
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