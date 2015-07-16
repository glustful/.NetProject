using System;
using System.Collections.Generic;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using Event.Entity.Model;
using Zerg.Common.Data;

namespace Event.Service.PartImage
{
	public class PartImageService : IPartImageService
	{
		private readonly IEventRepository<PartImageEntity> _partimageRepository;
		private readonly ILog _log;

		public PartImageService(IEventRepository<PartImageEntity> partimageRepository,ILog log)
		{
			_partimageRepository = partimageRepository;
			_log = log;
		}
		
		public PartImageEntity Create (PartImageEntity entity)
		{
			try
            {
                _partimageRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(PartImageEntity entity)
		{
			try
            {
                _partimageRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public PartImageEntity Update (PartImageEntity entity)
		{
			try
            {
                _partimageRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public PartImageEntity GetPartImageById (int id)
		{
			try
            {
                return _partimageRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<PartImageEntity> GetPartImagesByCondition(PartImageSearchCondition condition)
		{
			var query = _partimageRepository.Table;
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
				if (!string.IsNullOrEmpty(condition.Imgurl))
                {
                    query = query.Where(q => q.Imgurl.Contains(condition.Imgurl));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Crowds != null && condition.Crowds.Any())
                {
                    query = query.Where(q => condition.Crowds.Contains(q.Crowd));
                }
				if (condition.Orderbys != null && condition.Orderbys.Any())
                {
                    query = query.Where(q => condition.Orderbys.Contains(q.Orderby));
                }
				if (condition.Addusers != null && condition.Addusers.Any())
                {
                    query = query.Where(q => condition.Addusers.Contains(q.Adduser));
                }
				if (condition.Upusers != null && condition.Upusers.Any())
                {
                    query = query.Where(q => condition.Upusers.Contains(q.Upuser));
                }
                if (condition.CrowdId != null) {
                    query = query.Where(q => q.Crowd.Id == condition.CrowdId);
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumPartImageSearchOrderBy.OrderById:
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

		public int GetPartImageCount (PartImageSearchCondition condition)
		{
			var query = _partimageRepository.Table;
			try
			{
                if (condition.CrowdId != null)
                {
                    query = query.Where(q => q.Crowd.Id == condition.CrowdId);
                }
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
				if (!string.IsNullOrEmpty(condition.Imgurl))
                {
                    query = query.Where(q => q.Imgurl.Contains(condition.Imgurl));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Crowds != null && condition.Crowds.Any())
                {
                    query = query.Where(q => condition.Crowds.Contains(q.Crowd));
                }
				if (condition.Orderbys != null && condition.Orderbys.Any())
                {
                    query = query.Where(q => condition.Orderbys.Contains(q.Orderby));
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


        //public IQueryable<PartImageEntity> GetPartImageByCrowdId(int crowdId)
        //{
        //    try
        //    {
        //        var query = _partimageRepository.Table;
        //        if (crowdId != 0)
        //        {
        //            return query.Where(q => q.Crowd.Id == crowdId);
        //        }
        //        return null;
        //    }
        //    catch (Exception e)
        //    {
        //        _log.Error(e, "数据库操作出错");
        //        return null;
        //    }
        //}
        public List<PartImageEntity> GetPartImageByCrowdId(int crowdId)
        {
            try
            {
                return _partimageRepository.Table.Where(p => p.Crowd.Id==crowdId).ToList();
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }
    }
}