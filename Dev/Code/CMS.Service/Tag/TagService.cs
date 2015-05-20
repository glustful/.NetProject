using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CMS.Entity.Model;
using Zerg.Common.Data;

namespace CMS.Service.Tag
{
	public class TagService : ITagService
	{
		private readonly ICMSRepository<TagEntity> _tagRepository;
		private readonly ILog _log;

		public TagService(ICMSRepository<TagEntity> tagRepository,ILog log)
		{
			_tagRepository = tagRepository;
			_log = log;
		}
		
		public TagEntity Create (TagEntity entity)
		{
			try
            {
                _tagRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(TagEntity entity)
		{
			try
            {
                _tagRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public TagEntity Update (TagEntity entity)
		{
			try
            {
                _tagRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}
		public TagEntity GetTagById (int id)
		{
			try
            {
                return _tagRepository.GetById(id); ;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<TagEntity> GetTagsByCondition(TagSearchCondition condition)
		{
			var query = _tagRepository.Table;
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
				if (!string.IsNullOrEmpty(condition.Tag))
                {
                    query = query.Where(q => q.Tag == condition.Tag);
                }
			    if (!string.IsNullOrEmpty(condition.LikeTag))
			    {
                    query = query.Where(q => q.Tag.Contains(condition.LikeTag));
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
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumTagSearchOrderBy.OrderById:
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

	    public int GetTagCount (TagSearchCondition condition)
		{
			var query = _tagRepository.Table;
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
				if (!string.IsNullOrEmpty(condition.Tag))
                {
                    query = query.Where(q => q.Tag == condition.Tag);
                }
                if (!string.IsNullOrEmpty(condition.LikeTag))
                {
                    query = query.Where(q => q.Tag.Contains(condition.LikeTag));
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