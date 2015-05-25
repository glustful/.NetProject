






using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using Trading.Entity.Model;

namespace Trading.Service.ProductDetail
{
	public class ProductDetailService : IProductDetailService
	{
		private readonly Zerg.Common.Data.ITradingRepository<ProductDetailEntity> _productdetailRepository;
		private readonly ILog _log;

		public ProductDetailService(Zerg.Common.Data.ITradingRepository<ProductDetailEntity> productdetailRepository,ILog log)
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


				if (!string.IsNullOrEmpty(condition.Productname))
                {
                    query = query.Where(q => q.Productname.Contains(condition.Productname));
                }



				if (!string.IsNullOrEmpty(condition.Productdetail))
                {
                    query = query.Where(q => q.Productdetail.Contains(condition.Productdetail));
                }



				if (!string.IsNullOrEmpty(condition.Productimg))
                {
                    query = query.Where(q => q.Productimg.Contains(condition.Productimg));
                }



				if (!string.IsNullOrEmpty(condition.Productimg1))
                {
                    query = query.Where(q => q.Productimg1.Contains(condition.Productimg1));
                }



				if (!string.IsNullOrEmpty(condition.Productimg2))
                {
                    query = query.Where(q => q.Productimg2.Contains(condition.Productimg2));
                }



				if (!string.IsNullOrEmpty(condition.Productimg3))
                {
                    query = query.Where(q => q.Productimg3.Contains(condition.Productimg3));
                }



				if (!string.IsNullOrEmpty(condition.Productimg4))
                {
                    query = query.Where(q => q.Productimg4.Contains(condition.Productimg4));
                }



				if (!string.IsNullOrEmpty(condition.Sericeinstruction))
                {
                    query = query.Where(q => q.Sericeinstruction.Contains(condition.Sericeinstruction));
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





				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {

						case EnumProductDetailSearchOrderBy.OrderById:
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

		public int GetProductDetailCount (ProductDetailSearchCondition condition)
		{
			var query = _productdetailRepository.Table;
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


				if (!string.IsNullOrEmpty(condition.Productname))
                {
                    query = query.Where(q => q.Productname.Contains(condition.Productname));
                }



				if (!string.IsNullOrEmpty(condition.Productdetail))
                {
                    query = query.Where(q => q.Productdetail.Contains(condition.Productdetail));
                }



				if (!string.IsNullOrEmpty(condition.Productimg))
                {
                    query = query.Where(q => q.Productimg.Contains(condition.Productimg));
                }



				if (!string.IsNullOrEmpty(condition.Productimg1))
                {
                    query = query.Where(q => q.Productimg1.Contains(condition.Productimg1));
                }



				if (!string.IsNullOrEmpty(condition.Productimg2))
                {
                    query = query.Where(q => q.Productimg2.Contains(condition.Productimg2));
                }



				if (!string.IsNullOrEmpty(condition.Productimg3))
                {
                    query = query.Where(q => q.Productimg3.Contains(condition.Productimg3));
                }



				if (!string.IsNullOrEmpty(condition.Productimg4))
                {
                    query = query.Where(q => q.Productimg4.Contains(condition.Productimg4));
                }



				if (!string.IsNullOrEmpty(condition.Sericeinstruction))
                {
                    query = query.Where(q => q.Sericeinstruction.Contains(condition.Sericeinstruction));
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