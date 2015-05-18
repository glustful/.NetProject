using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CMS.Entity.Model;
using Zerg.Common.Data;

namespace CMS.Service.Resource
{
	public class ResourceService : IResourceService
	{
		private readonly ICMSRepository<ResourceEntity> _resourceRepository;
		private readonly ILog _log;

		public ResourceService(ICMSRepository<ResourceEntity> resourceRepository,ILog log)
		{
			_resourceRepository = resourceRepository;
			_log = log;
		}
		
		public ResourceEntity Create (ResourceEntity entity)
		{
			try
            {
                _resourceRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(ResourceEntity entity)
		{
			try
            {
                _resourceRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public ResourceEntity Update (ResourceEntity entity)
		{
			try
            {
                _resourceRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public ResourceEntity GetResourceById (int id)
		{
			try
            {
                return _resourceRepository.GetById(id); ;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<ResourceEntity> GetResourcesByCondition(ResourceSearchCondition condition)
		{
			var query = _resourceRepository.Table;
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
				if (string.IsNullOrEmpty(condition.Type))
                {
                    query = query.Where(q => q.Type == condition.Type);
                }
				if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Guids != null && condition.Guids.Any())
                {
                    query = query.Where(q => condition.Guids.Contains(q.Guid));
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
						case EnumResourceSearchOrderBy.OrderById:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
						case EnumResourceSearchOrderBy.OrderByGuid:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Guid):query.OrderBy(q=>q.Guid);
							break;
						case EnumResourceSearchOrderBy.OrderByName:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Name):query.OrderBy(q=>q.Name);
							break;
						case EnumResourceSearchOrderBy.OrderByType:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Type):query.OrderBy(q=>q.Type);
							break;
						case EnumResourceSearchOrderBy.OrderByLength:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Length):query.OrderBy(q=>q.Length);
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

		public int GetResourceCount (ResourceSearchCondition condition)
		{
			var query = _resourceRepository.Table;
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
				if (string.IsNullOrEmpty(condition.Type))
                {
                    query = query.Where(q => q.Type == condition.Type);
                }
				if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Guids != null && condition.Guids.Any())
                {
                    query = query.Where(q => condition.Guids.Contains(q.Guid));
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