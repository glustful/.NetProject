using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using Event.Entity.Model;

namespace Event.Service.Follower
{
	public class FollowerService : IFollowerService
	{
		private readonly IRepository<FollowerEntity> _followerRepository;
		private readonly ILog _log;

		public FollowerService(IRepository<FollowerEntity> followerRepository,ILog log)
		{
			_followerRepository = followerRepository;
			_log = log;
		}
		
		public FollowerEntity Create (FollowerEntity entity)
		{
			try
            {
                _followerRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(FollowerEntity entity)
		{
			try
            {
                _followerRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public FollowerEntity Update (FollowerEntity entity)
		{
			try
            {
                _followerRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public FollowerEntity GetFollowerById (int id)
		{
			try
            {
                return _followerRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<FollowerEntity> GetFollowersByCondition(FollowerSearchCondition condition)
		{
			var query = _followerRepository.Table;
			try
			{
				if (!string.IsNullOrEmpty(condition.Openid))
                {
                    query = query.Where(q => q.Openid.Contains(condition.Openid));
                }
				if (!string.IsNullOrEmpty(condition.Nickname))
                {
                    query = query.Where(q => q.Nickname.Contains(condition.Nickname));
                }
				if (!string.IsNullOrEmpty(condition.Sex))
                {
                    query = query.Where(q => q.Sex.Contains(condition.Sex));
                }
				if (!string.IsNullOrEmpty(condition.City))
                {
                    query = query.Where(q => q.City.Contains(condition.City));
                }
				if (!string.IsNullOrEmpty(condition.Country))
                {
                    query = query.Where(q => q.Country.Contains(condition.Country));
                }
				if (!string.IsNullOrEmpty(condition.Private))
                {
                    query = query.Where(q => q.Private.Contains(condition.Private));
                }
				if (!string.IsNullOrEmpty(condition.Language))
                {
                    query = query.Where(q => q.Language.Contains(condition.Language));
                }
				if (!string.IsNullOrEmpty(condition.Headimgurl))
                {
                    query = query.Where(q => q.Headimgurl.Contains(condition.Headimgurl));
                }
				if (!string.IsNullOrEmpty(condition.Subscribetime))
                {
                    query = query.Where(q => q.Subscribetime.Contains(condition.Subscribetime));
                }
				if (!string.IsNullOrEmpty(condition.Unioid))
                {
                    query = query.Where(q => q.Unioid.Contains(condition.Unioid));
                }
				if (!string.IsNullOrEmpty(condition.Remark))
                {
                    query = query.Where(q => q.Remark.Contains(condition.Remark));
                }
				if (!string.IsNullOrEmpty(condition.Groupid))
                {
                    query = query.Where(q => q.Groupid.Contains(condition.Groupid));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumFollowerSearchOrderBy.OrderById:
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

		public int GetFollowerCount (FollowerSearchCondition condition)
		{
			var query = _followerRepository.Table;
			try
			{
				if (!string.IsNullOrEmpty(condition.Openid))
                {
                    query = query.Where(q => q.Openid.Contains(condition.Openid));
                }
				if (!string.IsNullOrEmpty(condition.Nickname))
                {
                    query = query.Where(q => q.Nickname.Contains(condition.Nickname));
                }
				if (!string.IsNullOrEmpty(condition.Sex))
                {
                    query = query.Where(q => q.Sex.Contains(condition.Sex));
                }
				if (!string.IsNullOrEmpty(condition.City))
                {
                    query = query.Where(q => q.City.Contains(condition.City));
                }
				if (!string.IsNullOrEmpty(condition.Country))
                {
                    query = query.Where(q => q.Country.Contains(condition.Country));
                }
				if (!string.IsNullOrEmpty(condition.Private))
                {
                    query = query.Where(q => q.Private.Contains(condition.Private));
                }
				if (!string.IsNullOrEmpty(condition.Language))
                {
                    query = query.Where(q => q.Language.Contains(condition.Language));
                }
				if (!string.IsNullOrEmpty(condition.Headimgurl))
                {
                    query = query.Where(q => q.Headimgurl.Contains(condition.Headimgurl));
                }
				if (!string.IsNullOrEmpty(condition.Subscribetime))
                {
                    query = query.Where(q => q.Subscribetime.Contains(condition.Subscribetime));
                }
				if (!string.IsNullOrEmpty(condition.Unioid))
                {
                    query = query.Where(q => q.Unioid.Contains(condition.Unioid));
                }
				if (!string.IsNullOrEmpty(condition.Remark))
                {
                    query = query.Where(q => q.Remark.Contains(condition.Remark));
                }
				if (!string.IsNullOrEmpty(condition.Groupid))
                {
                    query = query.Where(q => q.Groupid.Contains(condition.Groupid));
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