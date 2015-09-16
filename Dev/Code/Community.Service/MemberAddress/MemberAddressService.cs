using System;
using System.Linq;
using Community.Entity.Model.MemberAddress;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Community.Service.MemberAddress
{
	public class MemberAddressService : IMemberAddressService
	{
		private readonly ICommunityRepository<MemberAddressEntity> _memberaddressRepository;
		private readonly ILog _log;

		public MemberAddressService(ICommunityRepository<MemberAddressEntity> memberaddressRepository,ILog log)
		{
			_memberaddressRepository = memberaddressRepository;
			_log = log;
		}
		
		public MemberAddressEntity Create (MemberAddressEntity entity)
		{
			try
            {
                _memberaddressRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(MemberAddressEntity entity)
		{
			try
            {
                _memberaddressRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public MemberAddressEntity Update (MemberAddressEntity entity)
		{
			try
            {
                _memberaddressRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public MemberAddressEntity GetMemberAddressById (int id)
		{
			try
            {
                return _memberaddressRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<MemberAddressEntity> GetMemberAddresssByCondition(MemberAddressSearchCondition condition)
		{
			var query = _memberaddressRepository.Table;
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
				if (condition.UpdtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Updtime>= condition.UpdtimeBegin.Value);
                }
                if (condition.UpdtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Updtime < condition.UpdtimeEnd.Value);
                }
				if (condition.Adduser.HasValue)
                {
                    query = query.Where(q => q.Adduser == condition.Adduser.Value);
                }
				if (condition.Upduser.HasValue)
                {
                    query = query.Where(q => q.Upduser == condition.Upduser.Value);
                }
				if (!string.IsNullOrEmpty(condition.Address))
                {
                    query = query.Where(q => q.Address.Contains(condition.Address));
                }
				if (!string.IsNullOrEmpty(condition.Zip))
                {
                    query = query.Where(q => q.Zip.Contains(condition.Zip));
                }
				if (!string.IsNullOrEmpty(condition.Linkman))
                {
                    query = query.Where(q => q.Linkman.Contains(condition.Linkman));
                }
				if (!string.IsNullOrEmpty(condition.Tel))
                {
                    query = query.Where(q => q.Tel.Contains(condition.Tel));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
                if (!string.IsNullOrEmpty(condition.UserName))
                {
                    query = query.Where(q => q.Member.UserName == condition.UserName);
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumMemberAddressSearchOrderBy.OrderById:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
						case EnumMemberAddressSearchOrderBy.OrderByMember:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Member):query.OrderBy(q=>q.Member);
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

		public int GetMemberAddressCount (MemberAddressSearchCondition condition)
		{
			var query = _memberaddressRepository.Table;
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
				if (condition.UpdtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Updtime>= condition.UpdtimeBegin.Value);
                }
                if (condition.UpdtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Updtime < condition.UpdtimeEnd.Value);
                }
				if (condition.Adduser.HasValue)
                {
                    query = query.Where(q => q.Adduser == condition.Adduser.Value);
                }
				if (condition.Upduser.HasValue)
                {
                    query = query.Where(q => q.Upduser == condition.Upduser.Value);
                }
				if (!string.IsNullOrEmpty(condition.Address))
                {
                    query = query.Where(q => q.Address.Contains(condition.Address));
                }
				if (!string.IsNullOrEmpty(condition.Zip))
                {
                    query = query.Where(q => q.Zip.Contains(condition.Zip));
                }
				if (!string.IsNullOrEmpty(condition.Linkman))
                {
                    query = query.Where(q => q.Linkman.Contains(condition.Linkman));
                }
				if (!string.IsNullOrEmpty(condition.Tel))
                {
                    query = query.Where(q => q.Tel.Contains(condition.Tel));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
                if (!string.IsNullOrEmpty(condition.UserName))
                {
                    query = query.Where(q => q.Member.UserName == condition.UserName);
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