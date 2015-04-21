using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using CMS.Entity.Model;

namespace CMS.Service.PublishProduct
{
	public class PublishProductService : IPublishProductService
	{
		private readonly IRepository<PublishProductEntity> _publishproductRepository;
		private readonly ILog _log;

		public PublishProductService(IRepository<PublishProductEntity> publishproductRepository,ILog log)
		{
			_publishproductRepository = publishproductRepository;
			_log = log;
		}
		
		public PublishProductEntity Create (PublishProductEntity entity)
		{
			try
            {
                _publishproductRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(PublishProductEntity entity)
		{
			try
            {
                _publishproductRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public PublishProductEntity Update (PublishProductEntity entity)
		{
			try
            {
                _publishproductRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public PublishProductEntity GetPublishProductById (int id)
		{
			try
            {
                return _publishproductRepository.GetById(id); ;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<PublishProductEntity> GetPublishProductsByCondition(PublishProductSearchCondition condition)
		{
			var query = _publishproductRepository.Table;
			try
			{
				if (condition.PublishtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Publishtime>= condition.PublishtimeBegin.Value);
                }
                if (condition.PublishtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Publishtime < condition.PublishtimeEnd.Value);
                }
				if (!string.IsNullOrEmpty(condition.ProductName))
                {
                    query = query.Where(q => q.ProductName.Contains(condition.ProductName));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.ProductIds != null && condition.ProductIds.Any())
                {
                    query = query.Where(q => condition.ProductIds.Contains(q.ProductId));
                }
				if (condition.PublishUsers != null && condition.PublishUsers.Any())
                {
                    query = query.Where(q => condition.PublishUsers.Contains(q.PublishUser));
                }
				if (condition.Tagss != null && condition.Tagss.Any())
                {
                    query = query.Where(q => condition.Tagss.Contains(q.Tags));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumPublishProductSearchOrderBy.OrderById:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
						case EnumPublishProductSearchOrderBy.OrderByPublishtime:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Publishtime):query.OrderBy(q=>q.Publishtime);
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

		public int GetPublishProductCount (PublishProductSearchCondition condition)
		{
			var query = _publishproductRepository.Table;
			try
			{
				if (condition.PublishtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Publishtime>= condition.PublishtimeBegin.Value);
                }
                if (condition.PublishtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Publishtime < condition.PublishtimeEnd.Value);
                }
				if (!string.IsNullOrEmpty(condition.ProductName))
                {
                    query = query.Where(q => q.ProductName.Contains(condition.ProductName));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.ProductIds != null && condition.ProductIds.Any())
                {
                    query = query.Where(q => condition.ProductIds.Contains(q.ProductId));
                }
				if (condition.PublishUsers != null && condition.PublishUsers.Any())
                {
                    query = query.Where(q => condition.PublishUsers.Contains(q.PublishUser));
                }
				if (condition.Tagss != null && condition.Tagss.Any())
                {
                    query = query.Where(q => condition.Tagss.Contains(q.Tags));
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