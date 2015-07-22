






using System;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using Trading.Entity.Model;

namespace Trading.Service.ProductBrand
{
    public class ProductBrandService : IProductBrandService
    {
        private readonly Zerg.Common.Data.ITradingRepository<ProductBrandEntity> _productbrandRepository;
        private readonly ILog _log;

        public ProductBrandService(Zerg.Common.Data.ITradingRepository<ProductBrandEntity> productbrandRepository, ILog log)
        {
            _productbrandRepository = productbrandRepository;
            _log = log;
        }

        public ProductBrandEntity Create(ProductBrandEntity entity)
        {
            try
            {
                _productbrandRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public bool Delete(ProductBrandEntity entity)
        {
            try
            {
                _productbrandRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return false;
            }
        }

        public ProductBrandEntity Update(ProductBrandEntity entity)
        {
            try
            {
                _productbrandRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public ProductBrandEntity GetProductBrandById(int id)
        {
            try
            {
                return _productbrandRepository.GetById(id);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }
        public IQueryable<ProductBrandEntity> GetProductBrandsByCondition(ProductBrandSearchCondition condition)
        {
            var query = _productbrandRepository.Table;
            try
            {

                if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime >= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }
                if (condition.Classify!=null)
                {
                    query = query.Where(q => q.ClassId == condition.Classify.Id);
                }

                if (condition.UpdtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Updtime >= condition.UpdtimeBegin.Value);
                }
                if (condition.UpdtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Updtime < condition.UpdtimeEnd.Value);
                }


                if (!string.IsNullOrEmpty(condition.Bname))
                {
                    query = query.Where(q => q.Bname.Contains(condition.Bname));
                }



                if (!string.IsNullOrEmpty(condition.Bimg))
                {
                    query = query.Where(q => q.Bimg.Contains(condition.Bimg));
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

                if (condition.ProductBrand != null)
                {
                    query = query.Where(q => q.Id == condition.ProductBrand);
                }


                if (condition.OrderBy.HasValue)
                {
                    switch (condition.OrderBy.Value)
                    {

                        case EnumProductBrandSearchOrderBy.OrderById:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.Id) : query.OrderBy(q => q.Id);
                            break;
                        case EnumProductBrandSearchOrderBy.OrderByAddtime:
                            query=condition.IsDescending ? query.OrderByDescending(q => q.Addtime) : query.OrderBy(q => q.Addtime);
                            break;
                    }

                }

                else
                {
                    query = query.OrderBy(q => q.Id);
                }

                if (condition.Page.HasValue && condition.PageCount.HasValue)
                {
                    query = query.Skip((condition.Page.Value - 1) * condition.PageCount.Value).Take(condition.PageCount.Value);
                }
                return query;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public int GetProductBrandCount(ProductBrandSearchCondition condition)
        {
            var query = _productbrandRepository.Table;
            try
            {

                if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime >= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }
                if (condition.Classify != null)
                {
                    query = query.Where(q => q.ClassId == condition.Classify.Id);
                }

                if (condition.UpdtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Updtime >= condition.UpdtimeBegin.Value);
                }
                if (condition.UpdtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Updtime < condition.UpdtimeEnd.Value);
                }


                if (!string.IsNullOrEmpty(condition.Bname))
                {
                    query = query.Where(q => q.Bname.Contains(condition.Bname));
                }

                if (condition.ProductBrand != null)
                {
                    query = query.Where(q => q.Id == condition.ProductBrand);
                }

                if (!string.IsNullOrEmpty(condition.Bimg))
                {
                    query = query.Where(q => q.Bimg.Contains(condition.Bimg));
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
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return -1;
            }
        }
    }
}