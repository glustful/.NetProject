using System;
using System.Linq;
using Community.Entity.Model.ProductDetail;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Community.Service.ProductDetail
{
	public class ProductDetailService : IProductDetailService
	{
		private readonly ICommunityRepository<ProductDetailEntity> _productdetailRepository;
		private readonly ILog _log;

		public ProductDetailService(ICommunityRepository<ProductDetailEntity> productdetailRepository,ILog log)
		{
			_productdetailRepository = productdetailRepository;
			_log = log;
		}
		
		public ProductDetailEntity Create (ProductDetailEntity entity)
		{
			try
            {
                _productdetailRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(ProductDetailEntity entity)
		{
			try
            {
                _productdetailRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public ProductDetailEntity Update (ProductDetailEntity entity)
		{
			try
            {
                _productdetailRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public ProductDetailEntity GetProductDetailById (int id)
		{
			try
            {
                return _productdetailRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<ProductDetailEntity> GetProductDetailsByCondition(ProductDetailSearchCondition condition)
		{
			var query = _productdetailRepository.Table;
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
				if (condition.UpdTimeBegin.HasValue)
                {
                    query = query.Where(q => q.UpdTime>= condition.UpdTimeBegin.Value);
                }
                if (condition.UpdTimeEnd.HasValue)
                {
                    query = query.Where(q => q.UpdTime < condition.UpdTimeEnd.Value);
                }
				if (condition.AddUser.HasValue)
                {
                    query = query.Where(q => q.AddUser == condition.AddUser.Value);
                }
				if (condition.UpdUser.HasValue)
                {
                    query = query.Where(q => q.UpdUser == condition.UpdUser.Value);
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
						case EnumProductDetailSearchOrderBy.OrderById:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
						case EnumProductDetailSearchOrderBy.OrderByName:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Name):query.OrderBy(q=>q.Name);
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

		public int GetProductDetailCount (ProductDetailSearchCondition condition)
		{
			var query = _productdetailRepository.Table;
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
				if (condition.UpdTimeBegin.HasValue)
                {
                    query = query.Where(q => q.UpdTime>= condition.UpdTimeBegin.Value);
                }
                if (condition.UpdTimeEnd.HasValue)
                {
                    query = query.Where(q => q.UpdTime < condition.UpdTimeEnd.Value);
                }
				if (condition.AddUser.HasValue)
                {
                    query = query.Where(q => q.AddUser == condition.AddUser.Value);
                }
				if (condition.UpdUser.HasValue)
                {
                    query = query.Where(q => q.UpdUser == condition.UpdUser.Value);
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