






using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using Trading.Entity.Model;

namespace Trading.Service.ProductParameter
{
	public class ProductParameterService : IProductParameterService
	{
		private readonly Zerg.Common.Data.ITradingRepository<ProductParameterEntity> _productparameterRepository;
		private readonly ILog _log;

		public ProductParameterService(Zerg.Common.Data.ITradingRepository<ProductParameterEntity> productparameterRepository,ILog log)
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
        /// <summary>
        /// 根据商品获取其所有的参数值列表；
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IQueryable<ProductParameterEntity> GetProductParametersByProduct(int  productId)
        {
            var query = _productparameterRepository.Table;
            try
            {
                query = query.Where(q=>q.Product.Id==productId);
                return query.OrderBy(q=>q.Id);
            }catch(Exception e){
                _log.Error(e,"数据库操作出错");
                return null;
            }
        }
		public IQueryable<ProductParameterEntity> GetProductParametersByCondition(ProductParameterSearchCondition condition)
		{
			var query = _productparameterRepository.Table;
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


				if (condition.UpdtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Updtime>= condition.UpdtimeBegin.Value);
                }
                if (condition.UpdtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Updtime < condition.UpdtimeEnd.Value);
                }


				if (!string.IsNullOrEmpty(condition.Adduser))
                {
                    query = query.Where(q => q.Adduser.Contains(condition.Adduser));
                }



				if (!string.IsNullOrEmpty(condition.Upduser))
                {
                    query = query.Where(q => q.Upduser.Contains(condition.Upduser));
                }



				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
                //=============================================彭贵飞 20150708 start=====================================

                if (condition.ProductId.HasValue)
                {
                    query = query.Where(q => condition.ProductId == q.Product.Id);
                }
                if (condition.ParameterId.HasValue)
                {
                    query = query.Where(q => condition.ParameterId == q.Parameter.Id);
                }
                //=============================================end                  =====================================

				if (condition.ParameterValues != null && condition.ParameterValues.Any())
                {
                    query = query.Where(q => condition.ParameterValues.Contains(q.ParameterValue));
                }


				if (condition.Parameters != null && condition.Parameters.Any())
                {
                    query = query.Where(q => condition.Parameters.Contains(q.Parameter));
                }


				if (condition.Products != null && condition.Products.Any())
                {
                    query = query.Where(q => condition.Products.Contains(q.Product));
                }


				if (condition.Sorts != null && condition.Sorts.Any())
                {
                    query = query.Where(q => condition.Sorts.Contains(q.Sort));
                }




				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {

						case EnumProductParameterSearchOrderBy.OrderById:
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

		public int GetProductParameterCount (ProductParameterSearchCondition condition)
		{
			var query = _productparameterRepository.Table;
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


				if (condition.UpdtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Updtime>= condition.UpdtimeBegin.Value);
                }
                if (condition.UpdtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Updtime < condition.UpdtimeEnd.Value);
                }


				if (!string.IsNullOrEmpty(condition.Adduser))
                {
                    query = query.Where(q => q.Adduser.Contains(condition.Adduser));
                }



				if (!string.IsNullOrEmpty(condition.Upduser))
                {
                    query = query.Where(q => q.Upduser.Contains(condition.Upduser));
                }

                

				if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
                //=============================================彭贵飞 20150708 start=====================================
               
                if (condition.ProductId.HasValue)
                {
                    query = query.Where(q => condition.ProductId == q.Product.Id);
                }
                if (condition.ParameterId.HasValue)
                {
                    query = query.Where(q => condition.ParameterId == q.Parameter.Id);
                }
                //=============================================end                  =====================================
				if (condition.ParameterValues != null && condition.ParameterValues.Any())
                {
                    query = query.Where(q => condition.ParameterValues.Contains(q.ParameterValue));
                }


				if (condition.Parameters != null && condition.Parameters.Any())
                {
                    query = query.Where(q => condition.Parameters.Contains(q.Parameter));
                }


				if (condition.Products != null && condition.Products.Any())
                {
                    query = query.Where(q => condition.Products.Contains(q.Product));
                }


				if (condition.Sorts != null && condition.Sorts.Any())
                {
                    query = query.Where(q => condition.Sorts.Contains(q.Sort));
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