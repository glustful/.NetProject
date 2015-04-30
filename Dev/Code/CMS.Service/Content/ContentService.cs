using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CMS.Entity.Model;
using Zerg.Common.Data;

namespace CMS.Service.Content
{
	public class ContentService : IContentService
	{
		private readonly ICMSRepository<ContentEntity> _contentRepository;
		private readonly ILog _log;

		public ContentService(ICMSRepository<ContentEntity> contentRepository,ILog log)
		{
			_contentRepository = contentRepository;
			_log = log;
		}
		
		public ContentEntity Create (ContentEntity entity)
		{
			try
            {
                _contentRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(ContentEntity entity)
		{
			try
            {
                _contentRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public ContentEntity Update (ContentEntity entity)
		{
			try
            {
                _contentRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public ContentEntity GetContentById (int id)
		{
			try
            {
                return _contentRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<ContentEntity> GetContentsByCondition(ContentSearchCondition condition)
		{
			var query = _contentRepository.Table;
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
				if (condition.UpdTimeBegin.HasValue)
                {
                    query = query.Where(q => q.UpdTime>= condition.UpdTimeBegin.Value);
                }
                if (condition.UpdTimeEnd.HasValue)
                {
                    query = query.Where(q => q.UpdTime < condition.UpdTimeEnd.Value);
                }
				if (condition.Status.HasValue)
                {
                    query = query.Where(q => q.Status == condition.Status.Value);
                }
				if (!string.IsNullOrEmpty(condition.Content))
                {
                    query = query.Where(q => q.Content.Contains(condition.Content));
                }
				if (!string.IsNullOrEmpty(condition.Title))
                {
                    query = query.Where(q => q.Title.Contains(condition.Title));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Addusers != null && condition.Addusers.Any())
                {
                    query = query.Where(q => condition.Addusers.Contains(q.Adduser));
                }
				if (condition.UpdUsers != null && condition.UpdUsers.Any())
                {
                    query = query.Where(q => condition.UpdUsers.Contains(q.UpdUser));
                }
				if (condition.Praises != null && condition.Praises.Any())
                {
                    query = query.Where(q => condition.Praises.Contains(q.Praise));
                }
				if (condition.Unpraises != null && condition.Unpraises.Any())
                {
                    query = query.Where(q => condition.Unpraises.Contains(q.Unpraise));
                }
				if (condition.Viewcounts != null && condition.Viewcounts.Any())
                {
                    query = query.Where(q => condition.Viewcounts.Contains(q.Viewcount));
                }
                if (!string.IsNullOrEmpty(condition.ChannelName))
                {
                    query = query.Where(q=>q.Channel.Name == condition.ChannelName);
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumContentSearchOrderBy.OrderById:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
						case EnumContentSearchOrderBy.OrderByTitle:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Title):query.OrderBy(q=>q.Title);
							break;
						case EnumContentSearchOrderBy.OrderByAddtime:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Addtime):query.OrderBy(q=>q.Addtime);
							break;
						case EnumContentSearchOrderBy.OrderByUpdTime:
							query = condition.IsDescending?query.OrderByDescending(q=>q.UpdTime):query.OrderBy(q=>q.UpdTime);
							break;
						case EnumContentSearchOrderBy.OrderByPraise:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Praise):query.OrderBy(q=>q.Praise);
							break;
						case EnumContentSearchOrderBy.OrderByUnpraise:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Unpraise):query.OrderBy(q=>q.Unpraise);
							break;
						case EnumContentSearchOrderBy.OrderByViewcount:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Viewcount):query.OrderBy(q=>q.Viewcount);
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

		public int GetContentCount (ContentSearchCondition condition)
		{
			var query = _contentRepository.Table;
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
				if (condition.UpdTimeBegin.HasValue)
                {
                    query = query.Where(q => q.UpdTime>= condition.UpdTimeBegin.Value);
                }
                if (condition.UpdTimeEnd.HasValue)
                {
                    query = query.Where(q => q.UpdTime < condition.UpdTimeEnd.Value);
                }
				if (condition.Status.HasValue)
                {
                    query = query.Where(q => q.Status == condition.Status.Value);
                }
				if (!string.IsNullOrEmpty(condition.Content))
                {
                    query = query.Where(q => q.Content.Contains(condition.Content));
                }
				if (!string.IsNullOrEmpty(condition.Title))
                {
                    query = query.Where(q => q.Title.Contains(condition.Title));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Addusers != null && condition.Addusers.Any())
                {
                    query = query.Where(q => condition.Addusers.Contains(q.Adduser));
                }
				if (condition.UpdUsers != null && condition.UpdUsers.Any())
                {
                    query = query.Where(q => condition.UpdUsers.Contains(q.UpdUser));
                }
				if (condition.Praises != null && condition.Praises.Any())
                {
                    query = query.Where(q => condition.Praises.Contains(q.Praise));
                }
				if (condition.Unpraises != null && condition.Unpraises.Any())
                {
                    query = query.Where(q => condition.Unpraises.Contains(q.Unpraise));
                }
				if (condition.Viewcounts != null && condition.Viewcounts.Any())
                {
                    query = query.Where(q => condition.Viewcounts.Contains(q.Viewcount));
                }
                if (string.IsNullOrEmpty(condition.ChannelName))
                {
                    query = query.Where(q => q.Channel.Name == condition.ChannelName);
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