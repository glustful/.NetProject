using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using Event.Entity.Model;

namespace Event.Service.Phone
{
	public class PhoneService : IPhoneService
	{
		private readonly IRepository<PhoneEntity> _phoneRepository;
		private readonly ILog _log;

		public PhoneService(IRepository<PhoneEntity> phoneRepository,ILog log)
		{
			_phoneRepository = phoneRepository;
			_log = log;
		}
		
		public PhoneEntity Create (PhoneEntity entity)
		{
			try
            {
                _phoneRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(PhoneEntity entity)
		{
			try
            {
                _phoneRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public PhoneEntity Update (PhoneEntity entity)
		{
			try
            {
                _phoneRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public PhoneEntity GetPhoneById (int id)
		{
			try
            {
                return _phoneRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<PhoneEntity> GetPhonesByCondition(PhoneSearchCondition condition)
		{
			var query = _phoneRepository.Table;
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
				if (!string.IsNullOrEmpty(condition.Openid))
                {
                    query = query.Where(q => q.Openid.Contains(condition.Openid));
                }
				if (!string.IsNullOrEmpty(condition.Phone))
                {
                    query = query.Where(q => q.Phone.Contains(condition.Phone));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Followers != null && condition.Followers.Any())
                {
                    query = query.Where(q => condition.Followers.Contains(q.Follower));
                }
				if (condition.Addusers != null && condition.Addusers.Any())
                {
                    query = query.Where(q => condition.Addusers.Contains(q.Adduser));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumPhoneSearchOrderBy.OrderById:
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

		public int GetPhoneCount (PhoneSearchCondition condition)
		{
			var query = _phoneRepository.Table;
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
				if (!string.IsNullOrEmpty(condition.Openid))
                {
                    query = query.Where(q => q.Openid.Contains(condition.Openid));
                }
				if (!string.IsNullOrEmpty(condition.Phone))
                {
                    query = query.Where(q => q.Phone.Contains(condition.Phone));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Followers != null && condition.Followers.Any())
                {
                    query = query.Where(q => condition.Followers.Contains(q.Follower));
                }
				if (condition.Addusers != null && condition.Addusers.Any())
                {
                    query = query.Where(q => condition.Addusers.Contains(q.Adduser));
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