using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CRM.Entity.Model;

namespace CRM.Service.MessageDetail
{
	public class MessageDetailService : IMessageDetailService
	{
		private readonly Zerg.Common.Data.ICRMRepository<MessageDetailEntity> _messagedetailRepository;
		private readonly ILog _log;

		public MessageDetailService(Zerg.Common.Data.ICRMRepository<MessageDetailEntity> messagedetailRepository,ILog log)
		{
			_messagedetailRepository = messagedetailRepository;
			_log = log;
		}
		
		public MessageDetailEntity Create (MessageDetailEntity entity)
		{
			try
            {
                _messagedetailRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(MessageDetailEntity entity)
		{
			try
            {
                _messagedetailRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public MessageDetailEntity Update (MessageDetailEntity entity)
		{
			try
            {
                _messagedetailRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public MessageDetailEntity GetMessageDetailById (int id)
		{
			try
            {
                return _messagedetailRepository.GetById(id); ;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<MessageDetailEntity> GetMessageDetailsByCondition(MessageDetailSearchCondition condition)
		{
			var query = _messagedetailRepository.Table;
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
				if (!string.IsNullOrEmpty(condition.Title))
                {
                    query = query.Where(q => q.Title.Contains(condition.Title));
                }
				if (!string.IsNullOrEmpty(condition.Content))
                {
                    query = query.Where(q => q.Content.Contains(condition.Content));
                }
                if (!string.IsNullOrEmpty(condition.InvitationCode))
                {
                    query = query.Where(q => q.InvitationCode == (condition.InvitationCode));
                }
                if (!string.IsNullOrEmpty(condition.InvitationId))
                {
                    query = query.Where(q => q.InvitationId == (condition.InvitationId));
                }

				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Senders != null && condition.Senders.Any())
                {
                    query = query.Where(q => condition.Senders.Contains(q.Sender));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumMessageDetailSearchOrderBy.OrderById:
							query = condition.isDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
                    }
					
				}
				else
				{
					query = query.OrderByDescending(q=>q.Id);
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

		public int GetMessageDetailCount (MessageDetailSearchCondition condition)
		{
			var query = _messagedetailRepository.Table;
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
				if (!string.IsNullOrEmpty(condition.Title))
                {
                    query = query.Where(q => q.Title.Contains(condition.Title));
                }
				if (!string.IsNullOrEmpty(condition.Content))
                {
                    query = query.Where(q => q.Content.Contains(condition.Content));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
                if (!string.IsNullOrEmpty(condition.InvitationCode))
                {
                    query = query.Where(q => q.InvitationCode == (condition.InvitationCode));
                }
                if (!string.IsNullOrEmpty(condition.InvitationId))
                {
                    query = query.Where(q => q.InvitationId == (condition.InvitationId));
                }

				if (condition.Senders != null && condition.Senders.Any())
                {
                    query = query.Where(q => condition.Senders.Contains(q.Sender));
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