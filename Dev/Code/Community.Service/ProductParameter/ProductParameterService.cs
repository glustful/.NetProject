using System;
using System.Collections.Generic;
using System.Linq;
using Community.Entity.Model.ProductParameter;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Community.Service.ProductParameter
{
	public class ProductParameterService : IProductParameterService
	{
		private readonly ICommunityRepository<ProductParameterEntity> _productparameterRepository;
		private readonly ILog _log;

		public ProductParameterService(ICommunityRepository<ProductParameterEntity> productparameterRepository,ILog log)
		{
			_productparameterRepository = productparameterRepository;
			_log = log;
		}
		
		public ProductParameterEntity Create (ProductParameterEntity entity)
		{
			try
            {
                _productparameterRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}
        public bool BulkCreate(List<ProductParameterEntity> entities)
        {
            try
            {
                _productparameterRepository.BulkInsert(entities);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return false;
            }
        }

		public bool Delete(ProductParameterEntity entity)
		{
			try
            {
                _productparameterRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public ProductParameterEntity Update (ProductParameterEntity entity)
		{
			try
            {
                _productparameterRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public ProductParameterEntity GetProductParameterById (int id)
		{
			try
            {
                return _productparameterRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public IQueryable<ProductParameterEntity> GetProductParametersByCondition(ProductParameterSearchCondition condition)
		{
			var query = _productparameterRepository.Table;
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
				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {
						case EnumProductParameterSearchOrderBy.OrderById:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Id):query.OrderBy(q=>q.Id);
							break;
						case EnumProductParameterSearchOrderBy.OrderBySort:
							query = condition.IsDescending?query.OrderByDescending(q=>q.Sort):query.OrderBy(q=>q.Sort);
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

		public int GetProductParameterCount (ProductParameterSearchCondition condition)
		{
			var query = _productparameterRepository.Table;
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