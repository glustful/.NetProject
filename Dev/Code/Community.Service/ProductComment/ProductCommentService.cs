using System;
using System.Linq;
using Community.Entity.Model.Product;
using Community.Entity.Model.ProductComment;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Community.Service.ProductComment
{
	public class ProductCommentService : IProductCommentService
	{
		private readonly ICommunityRepository<ProductCommentEntity> _productcommentRepository;
		private readonly ILog _log;

		public ProductCommentService(ICommunityRepository<ProductCommentEntity> productcommentRepository,ILog log)
		{
			_productcommentRepository = productcommentRepository;
			_log = log;
		}
		
		public ProductCommentEntity Create (ProductCommentEntity entity)
		{
			try
            {
                _productcommentRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(ProductCommentEntity entity)
		{
			try
            {
                _productcommentRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public ProductCommentEntity Update (ProductCommentEntity entity)
		{
			try
            {
                _productcommentRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public ProductCommentEntity GetProductCommentById (int id)
		{
			try
            {
                return _productcommentRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}
      

		public IQueryable<ProductCommentEntity> GetProductCommentsByCondition(ProductCommentSearchCondition condition)
		{
			var query = _productcommentRepository.Table;
			try
			{
				if (condition.AddTimeBegin.HasValue)
                {
                    query = query.Where(q => q.AddTime>= condition.AddTimeBegin.Value);
                }
                if (condition.AddTimeEnd.HasValue)
                {
                    query = query.Where(q => q.AddTime < condition.AddTimeEnd.Value);
                }
				if (condition.StarsBegin.HasValue)
                {
                    query = query.Where(q => q.Stars>= condition.StarsBegin.Value);
                }
                if (condition.StarsEnd.HasValue)
                {
                    query = query.Where(q => q.Stars < condition.StarsEnd.Value);
                }
			   
                //if (condition.AddUser.HasValue)
                //{
                //    query = query.Where(q => q.AddUser==condition.AddUser.Value);
                //}
				if (!string.IsNullOrEmpty(condition.Content))
                {
                    query = query.Where(q => q.Content.Contains(condition.Content));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Products != null && condition.Products.Any())
                {
                    query = query.Where(q => condition.Products.Contains(q.Product));
                }
                if (condition.ProductId!=null)
                {
                    query = query.Where(q => q.Product.Id == condition.ProductId);
                }
			    if (condition.Stars.HasValue)
			    {
			        query = query.Where(q => q.Stars == condition.Stars);
			    }
			    if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumProductCommentSearchOrderBy.OrderById:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
						case EnumProductCommentSearchOrderBy.OrderByAddTime:
							query = condition.IsDescending?query.OrderByDescending(q=>q.AddTime):query.OrderBy(q=>q.AddTime);
							break;
						case EnumProductCommentSearchOrderBy.OrderByStars:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Stars):query.OrderBy(q=>q.Stars);
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

		public int GetProductCommentCount (ProductCommentSearchCondition condition)
		{
			var query = _productcommentRepository.Table;
			try
			{
				if (condition.AddTimeBegin.HasValue)
                {
                    query = query.Where(q => q.AddTime>= condition.AddTimeBegin.Value);
                }
                if (condition.AddTimeEnd.HasValue)
                {
                    query = query.Where(q => q.AddTime < condition.AddTimeEnd.Value);
                }
				if (condition.StarsBegin.HasValue)
                {
                    query = query.Where(q => q.Stars>= condition.StarsBegin.Value);
                }
                if (condition.StarsEnd.HasValue)
                {
                    query = query.Where(q => q.Stars < condition.StarsEnd.Value);
                }
                //if (condition.AddUser.HasValue)
                //{
                //    query = query.Where(q => q.AddUser==condition.AddUser.Value);
                //}
				if (!string.IsNullOrEmpty(condition.Content))
                {
                    query = query.Where(q => q.Content.Contains(condition.Content));
                }
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if (condition.Products != null && condition.Products.Any())
                {
                    query = query.Where(q => condition.Products.Contains(q.Product));
                }
			    if (condition.ProductId.HasValue)
			    {
			        query = query.Where(q => q.Product.Id == condition.ProductId);
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