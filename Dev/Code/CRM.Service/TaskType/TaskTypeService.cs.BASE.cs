using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CMS.Entity.Model;

namespace CMS.Service.Channel
{
	public class ChannelService : IChannelService
	{
		private readonly IRepository<ChannelEntity> _channelRepository;
		private readonly ILog _log;

		public ChannelService(IRepository<ChannelEntity> channelRepository,ILog log)
		{
			_channelRepository = channelRepository;
			_log = log;
		}
		
		public ChannelEntity Create (ChannelEntity entity)
		{
			try
            {
                _channelRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(ChannelEntity entity)
		{
			try
            {
                _channelRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public ChannelEntity Update (ChannelEntity entity)
		{
			try
            {
                _channelRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public ChannelEntity GetChannelById (int id)
		{
			try
            {
                return _channelRepository.GetById(id); ;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<ChannelEntity> GetChannelsByCondition(ChannelSearchCondition condition)
		{
			var query = _channelRepository.Table;
			try
			{
				if (condition.Status.HasValue)
                {
                    query = query.Where(q => q.Status == condition.Status.Value);
                }
				if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumChannelSearchOrderBy.OrderById:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
						case EnumChannelSearchOrderBy.OrderByName:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Name):query.OrderBy(q=>q.Name);
							break;
						case EnumChannelSearchOrderBy.OrderByStatus:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Status):query.OrderBy(q=>q.Status);
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

		public int GetChannelCount (ChannelSearchCondition condition)
		{
			var query = _channelRepository.Table;
			try
			{
				if (condition.Status.HasValue)
                {
                    query = query.Where(q => q.Status == condition.Status.Value);
                }
				if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
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