






using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using Trading.Entity.Model;

namespace Trading.Service.BrandParameter
{
	public class BrandParameterService : IBrandParameterService
	{
		private readonly Zerg.Common.Data.ITradingRepository<BrandParameterEntity> _brandparameterRepository;
		private readonly ILog _log;

		public BrandParameterService(Zerg.Common.Data.ITradingRepository<BrandParameterEntity> brandparameterRepository,ILog log)
		{
			_brandparameterRepository = brandparameterRepository;
			_log = log;
		}
		
		public BrandParameterEntity Create (BrandParameterEntity entity)
		{
			try
            {
                _brandparameterRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public bool Delete(BrandParameterEntity entity)
		{
			try
            {
                _brandparameterRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return false;
            }
		}

		public BrandParameterEntity Update (BrandParameterEntity entity)
		{
			try
            {
                _brandparameterRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}

		public BrandParameterEntity GetBrandParameterById (int id)
		{
			try
            {
                return _brandparameterRepository.GetById(id);
            }
            catch (Exception e)
            {
				_log.Error(e,"数据库操作出错");
                return null;
            }
		}
        /// <summary>
        ///根据品牌id查参数；
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public IQueryable<BrandParameterEntity> GetBrandParametersByBrandId(int brandId)
        {
            var query = _brandparameterRepository.Table;
            try
            {
                query = query.Where(q => q.ProductBrand.Id==brandId);
                return query.OrderBy(q=>q.Id);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }
		public IQueryable<BrandParameterEntity> GetBrandParametersByCondition(BrandParameterSearchCondition condition)
		{
			var query = _brandparameterRepository.Table;
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


				if (!string.IsNullOrEmpty(condition.Parametername))
                {
                    query = query.Where(q => q.Parametername.Contains(condition.Parametername));
                }



				if (!string.IsNullOrEmpty(condition.Parametervaule))
                {
                    query = query.Where(q => q.Parametervaule.Contains(condition.Parametervaule));
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


				if (condition.ProductBrands != null && condition.ProductBrands.Any())
                {
                    query = query.Where(q => condition.ProductBrands.Contains(q.ProductBrand));
                }


				if (condition.Sorts != null && condition.Sorts.Any())
                {
                    query = query.Where(q => condition.Sorts.Contains(q.Sort));
                }




				if(condition.OrderBy.HasValue)
				{
					switch (condition.OrderBy.Value)
                    {

						case EnumBrandParameterSearchOrderBy.OrderById:
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

		public int GetBrandParameterCount (BrandParameterSearchCondition condition)
		{
			var query = _brandparameterRepository.Table;
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


				if (!string.IsNullOrEmpty(condition.Parametername))
                {
                    query = query.Where(q => q.Parametername.Contains(condition.Parametername));
                }



				if (!string.IsNullOrEmpty(condition.Parametervaule))
                {
                    query = query.Where(q => q.Parametervaule.Contains(condition.Parametervaule));
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


				if (condition.ProductBrands != null && condition.ProductBrands.Any())
                {
                    query = query.Where(q => condition.ProductBrands.Contains(q.ProductBrand));
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