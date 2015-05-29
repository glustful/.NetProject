






using System;
using System.Linq;
using YooPoon.Core.Logging;
using Trading.Entity.Model;

namespace Trading.Service.Product
{
    public class ProductService : IProductService
    {
        private readonly Zerg.Common.Data.ITradingRepository<ProductEntity> _productRepository;
        private readonly ILog _log;

        public ProductService(Zerg.Common.Data.ITradingRepository<ProductEntity> productRepository, ILog log)
        {
            _productRepository = productRepository;
            _log = log;
        }

        public ProductEntity Create(ProductEntity entity)
        {
            try
            {
                _productRepository.Insert(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public bool Delete(ProductEntity entity)
        {
            try
            {
                _productRepository.Delete(entity);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return false;
            }
        }

        public ProductEntity Update(ProductEntity entity)
        {
            try
            {
                _productRepository.Update(entity);
                return entity;
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public ProductEntity GetProductById(int id)
        {
            try
            {
                return _productRepository.GetById(id);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public IQueryable<ProductEntity> GetProductsByProductBrand(int BrandId)
        {
            var query = _productRepository.Table;
            try
            {
                query = query.Where(q => q.ProductBrand.Id == BrandId);
                return query.OrderBy(q=>q.Id);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }
        public IQueryable<ProductEntity> GetProductsByClassify(int ClassifyId)
        {
            var query = _productRepository.Table;
            try
            {
                query = query.Where(q => q.Classify.Id == ClassifyId);
                return query.OrderBy(q=>q.Id);
            }
            catch (Exception e)
            {
                _log.Error(e, "数据库操作出错");
                return null;
            }
        }

        public IQueryable<ProductEntity> GetProductsByCondition(ProductSearchCondition condition)
        {
            var query = _productRepository.Table;
            try
            {
                if (!string.IsNullOrEmpty(condition.AreaName))
                {
                    query = query.Where(q=>q.ProductParameter.Exists(pp=>pp.ParameterValue.Parametervalue == condition.AreaName));
                }
                if (condition.TypeId.HasValue)
                {
                    query = query.Where(q=>q.ProductParameter.Exists(pp=>pp.ParameterValue.Id ==condition.TypeId));
                }
                if (condition.CommissionBegin.HasValue)
                {
                    query = query.Where(q => q.Commission >= condition.CommissionBegin.Value);
                }
                if (condition.CommissionEnd.HasValue)
                {
                    query = query.Where(q => q.Commission < condition.CommissionEnd.Value);
                }


                if (condition.PriceBegin.HasValue)
                {
                    query = query.Where(q => q.Price >= condition.PriceBegin.Value);
                }
                if (condition.PriceEnd.HasValue)
                {
                    query = query.Where(q => q.Price < condition.PriceEnd.Value);
                }


                if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime >= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }


                if (condition.UpdtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Updtime >= condition.UpdtimeBegin.Value);
                }
                if (condition.UpdtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Updtime < condition.UpdtimeEnd.Value);
                }


                if (condition.Status.HasValue)
                {
                    query = query.Where(q => q.Status == condition.Status.Value);
                }



                if (condition.Recommend.HasValue)
                {
                    query = query.Where(q => q.Recommend == condition.Recommend.Value);
                }



                if (!string.IsNullOrEmpty(condition.Productname))
                {
                    query = query.Where(q => q.Productname.Contains(condition.Productname));
                }



                if (!string.IsNullOrEmpty(condition.Productimg))
                {
                    query = query.Where(q => q.Productimg.Contains(condition.Productimg));
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


                if (condition.ProductDetails != null && condition.ProductDetails.Any())
                {
                    query = query.Where(q => condition.ProductDetails.Contains(q.ProductDetail));
                }


                if (condition.Classifys != null && condition.Classifys.Any())
                {
                    query = query.Where(q => condition.Classifys.Contains(q.Classify));
                }


                if (condition.ProductBrands != null && condition.ProductBrands.Any())
                {
                    query = query.Where(q => condition.ProductBrands.Contains(q.ProductBrand));
                }


                if (condition.Bussnessid.HasValue)
                {
                    query = query.Where(q => q.Bussnessid==condition.Bussnessid);
                }


                if (condition.Sorts != null && condition.Sorts.Any())
                {
                    query = query.Where(q => condition.Sorts.Contains(q.Sort));
                }


                if (condition.Stockrules != null && condition.Stockrules.Any())
                {
                    query = query.Where(q => condition.Stockrules.Contains(q.Stockrule));
                }




                if (condition.OrderBy.HasValue)
                {
                    switch (condition.OrderBy.Value)
                    {

                        case EnumProductSearchOrderBy.OrderById:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.Id) : query.OrderBy(q => q.Id);
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

        public int GetProductCount(ProductSearchCondition condition)
        {
            var query = _productRepository.Table;
            try
            {
                if (!string.IsNullOrEmpty(condition.AreaName))
                {
                    query = query.Where(q => q.ProductParameter.Exists(pp => pp.ParameterValue.Parametervalue == condition.AreaName.ToString()));
                }
                if (condition.TypeId.HasValue)
                {
                    query = query.Where(q => q.ProductParameter.Exists(pp => pp.ParameterValue.Id == condition.TypeId));
                }
                if (condition.CommissionBegin.HasValue)
                {
                    query = query.Where(q => q.Commission >= condition.CommissionBegin.Value);
                }
                if (condition.CommissionEnd.HasValue)
                {
                    query = query.Where(q => q.Commission < condition.CommissionEnd.Value);
                }


                if (condition.PriceBegin.HasValue)
                {
                    query = query.Where(q => q.Price >= condition.PriceBegin.Value);
                }
                if (condition.PriceEnd.HasValue)
                {
                    query = query.Where(q => q.Price < condition.PriceEnd.Value);
                }


                if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Addtime >= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Addtime < condition.AddtimeEnd.Value);
                }


                if (condition.UpdtimeBegin.HasValue)
                {
                    query = query.Where(q => q.Updtime >= condition.UpdtimeBegin.Value);
                }
                if (condition.UpdtimeEnd.HasValue)
                {
                    query = query.Where(q => q.Updtime < condition.UpdtimeEnd.Value);
                }


                if (condition.Status.HasValue)
                {
                    query = query.Where(q => q.Status == condition.Status.Value);
                }



                if (condition.Recommend.HasValue)
                {
                    query = query.Where(q => q.Recommend == condition.Recommend.Value);
                }



                if (!string.IsNullOrEmpty(condition.Productname))
                {
                    query = query.Where(q => q.Productname.Contains(condition.Productname));
                }



                if (!string.IsNullOrEmpty(condition.Productimg))
                {
                    query = query.Where(q => q.Productimg.Contains(condition.Productimg));
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


                if (condition.ProductDetails != null && condition.ProductDetails.Any())
                {
                    query = query.Where(q => condition.ProductDetails.Contains(q.ProductDetail));
                }


                if (condition.Classifys != null && condition.Classifys.Any())
                {
                    query = query.Where(q => condition.Classifys.Contains(q.Classify));
                }


                if (condition.ProductBrands != null && condition.ProductBrands.Any())
                {
                    query = query.Where(q => condition.ProductBrands.Contains(q.ProductBrand));
                }


                if (condition.Bussnessid.HasValue)
                {
                    query = query.Where(q => q.Bussnessid == condition.Bussnessid);
                }



                if (condition.Sorts != null && condition.Sorts.Any())
                {
                    query = query.Where(q => condition.Sorts.Contains(q.Sort));
                }


                if (condition.Stockrules != null && condition.Stockrules.Any())
                {
                    query = query.Where(q => condition.Stockrules.Contains(q.Stockrule));
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