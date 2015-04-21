using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CMS.Entity.Model;

namespace CMS.Service.Advertisement
{
	public class AdvertisementService : IAdvertisementService
	{
		private readonly IRepository<AdvertisementEntity> _advertisementRepository;
		private readonly ILog _log;

		public AdvertisementService(IRepository<AdvertisementEntity> advertisementRepository,ILog log)
		{
			_advertisementRepository = advertisementRepository;
			_log = log;
		}
		
		public AdvertisementEntity Create (AdvertisementEntity entity)
		{
			try
            {
                _advertisementRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(AdvertisementEntity entity)
		{
			try
            {
                _advertisementRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public AdvertisementEntity Update (AdvertisementEntity entity)
		{
			try
            {
                _advertisementRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public AdvertisementEntity GetAdvertisementById (int id)
		{
			try
            {
                return _advertisementRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<AdvertisementEntity> GetAdvertisementsByCondition(AdvertisementSearchCondition condition)
		{
			var query = _advertisementRepository.Table;
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
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumAdvertisementSearchOrderBy.OrderById:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
						case EnumAdvertisementSearchOrderBy.OrderByTitle:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Title):query.OrderBy(q=>q.Title);
							break;
						case EnumAdvertisementSearchOrderBy.OrderByAddtime:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Addtime):query.OrderBy(q=>q.Addtime);
							break;
						case EnumAdvertisementSearchOrderBy.OrderByUpdTime:
							query = condition.IsDescending?query.OrderByDescending(q=>q.UpdTime):query.OrderBy(q=>q.UpdTime);
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

		public int GetAdvertisementCount (AdvertisementSearchCondition condition)
		{
			var query = _advertisementRepository.Table;
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