using System;
using System.Linq;
using Community.Entity.Model.Member;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Community.Service.Member
{
	public class MemberService : IMemberService
	{
		private readonly ICommunityRepository<MemberEntity> _memberRepository;
		private readonly ILog _log;

		public MemberService(ICommunityRepository<MemberEntity> memberRepository,ILog log)
		{
			_memberRepository = memberRepository;
			_log = log;
		}
		
		public MemberEntity Create (MemberEntity entity)
		{
			try
            {
                _memberRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(MemberEntity entity)
		{
			try
            {
                _memberRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public MemberEntity Update (MemberEntity entity)
		{
			try
            {
                _memberRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public MemberEntity GetMemberById (int id)
		{
			try
            {
                return _memberRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

        public MemberEntity GetMemberByUserId(int userId)
        {
            try
            {
                return _memberRepository.Table.FirstOrDefault(p => p.UserId == userId);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }


		public IQueryable<MemberEntity> GetMembersByCondition(MemberSearchCondition condition)
		{
			var query = _memberRepository.Table;
			try
			{
				if (!string.IsNullOrEmpty(condition.RealName))
                {
                    query = query.Where(q => q.RealName == condition.RealName);
                }
				if (!string.IsNullOrEmpty(condition.IdentityNo))
                {
                    query = query.Where(q => q.IdentityNo == condition.IdentityNo);
                }
				if (condition.Gender.HasValue)
                {
                    query = query.Where(q => q.Gender == condition.Gender.Value);
                }
				if (!string.IsNullOrEmpty(condition.Phone))
                {
                    query = query.Where(q => q.Phone == condition.Phone);
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumMemberSearchOrderBy.OrderById:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
						case EnumMemberSearchOrderBy.OrderByRealName:
							query = condition.IsDescending?query.OrderByDescending(q=>q.RealName):query.OrderBy(q=>q.RealName);
							break;
						case EnumMemberSearchOrderBy.OrderByGender:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Gender):query.OrderBy(q=>q.Gender);
							break;
						case EnumMemberSearchOrderBy.OrderByPhone:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Phone):query.OrderBy(q=>q.Phone);
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

		public int GetMemberCount (MemberSearchCondition condition)
		{
			var query = _memberRepository.Table;
			try
			{
				if (!string.IsNullOrEmpty(condition.RealName))
                {
                    query = query.Where(q => q.RealName == condition.RealName);
                }
				if (!string.IsNullOrEmpty(condition.IdentityNo))
                {
                    query = query.Where(q => q.IdentityNo == condition.IdentityNo);
                }
				if (condition.Gender.HasValue)
                {
                    query = query.Where(q => q.Gender == condition.Gender.Value);
                }
				if (!string.IsNullOrEmpty(condition.Phone))
                {
                    query = query.Where(q => q.Phone == condition.Phone);
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
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