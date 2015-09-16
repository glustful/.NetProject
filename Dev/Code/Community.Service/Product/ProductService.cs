using System;
using System.Linq;
using Community.Entity.Model.Product;
using YooPoon.Core.Logging;
using Zerg.Common.Data;

namespace Community.Service.Product
{
    public class ProductService : IProductService
    {
        private readonly ICommunityRepository<ProductEntity> _productRepository;
        private readonly ILog _log;

        public ProductService(ICommunityRepository<ProductEntity> productRepository, ILog log)
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

        public IQueryable<ProductEntity> GetProductsByCondition(ProductSearchCondition condition)
        {
            var query = _productRepository.Table;
            try
            {
                if (condition.PriceBegin.HasValue)
                {
                    query = query.Where(q => q.Price >= condition.PriceBegin.Value);
                }               
                if (condition.PriceEnd.HasValue)
                {
                    query = query.Where(q => q.Price < condition.PriceEnd.Value);
                }
                if (condition.StockBegin.HasValue)
                {
                    query = query.Where(q => q.Stock >= condition.StockBegin.Value);
                }
                if (condition.StockEnd.HasValue)
                {
                    query = query.Where(q => q.Stock < condition.StockEnd.Value);
                }
                if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.AddTime >= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.AddTime < condition.AddtimeEnd.Value);
                }
                if (condition.UpdTimeBegin.HasValue)
                {
                    query = query.Where(q => q.UpdTime >= condition.UpdTimeBegin.Value);
                }
                if (condition.UpdTimeEnd.HasValue)
                {
                    query = query.Where(q => q.UpdTime < condition.UpdTimeEnd.Value);
                }
                if (condition.Status.HasValue)
                {
                    query = query.Where(q => q.Status == condition.Status.Value);
                }
                if (condition.Adduser.HasValue)
                {
                    query = query.Where(q => q.AddUser == condition.Adduser.Value);
                }
                if (condition.UpdUser.HasValue)
                {
                    query = query.Where(q => q.UpdUser == condition.UpdUser.Value);
                }
                if (condition.Type.HasValue)
                {
                    query = query.Where(q => q.Type == condition.Type.Value);
                }
                if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
                }
                if (condition.IsRecommend.HasValue)
                {
                    query = query.Where(q => q.IsRecommend == condition.IsRecommend);
                }
                if (!string.IsNullOrEmpty(condition.Subtitte))
                {
                    query = query.Where(q => q.Subtitte.Contains(condition.Subtitte));
                }
                if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
                if (condition.Categorys != null )//传进来的是二级 查他下面的三级所有商品
                {
                    query = query.Where(q =>q.Category.Father.Id==condition.Categorys.Id);
                }


                if (condition.CategoryId.HasValue && condition.CategoryId!=0)// 传进来的是3级
                {                    
                    query = query.Where(q => q.Category.Id == condition.CategoryId);
                }


                if (condition.OrderBy.HasValue)
                {
                    switch (condition.OrderBy.Value)
                    {
                        case EnumProductSearchOrderBy.OrderById:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.Id) : query.OrderBy(q => q.Id);
                            break;
                        case EnumProductSearchOrderBy.OrderByPrice:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.Price) : query.OrderBy(q => q.Price);
                            break;
                        case EnumProductSearchOrderBy.OrderBySort:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.Sort) : query.OrderBy(q => q.Sort);
                            break;
                        case EnumProductSearchOrderBy.OrderByAddtime:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.AddTime) : query.OrderBy(q => q.AddTime);
                            break;
                        case EnumProductSearchOrderBy.OrderByOwner:
                            query = condition.IsDescending ? query.OrderByDescending(q => q.Owner) : query.OrderBy(q => q.Owner);
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
                if (condition.PriceBegin.HasValue)
                {
                    query = query.Where(q => q.Price >= condition.PriceBegin.Value);
                }
                if (condition.PriceEnd.HasValue)
                {
                    query = query.Where(q => q.Price < condition.PriceEnd.Value);
                }
                if (condition.StockBegin.HasValue)
                {
                    query = query.Where(q => q.Stock >= condition.StockBegin.Value);
                }
                if (condition.StockEnd.HasValue)
                {
                    query = query.Where(q => q.Stock < condition.StockEnd.Value);
                }
                if (condition.AddtimeBegin.HasValue)
                {
                    query = query.Where(q => q.AddTime >= condition.AddtimeBegin.Value);
                }
                if (condition.AddtimeEnd.HasValue)
                {
                    query = query.Where(q => q.AddTime < condition.AddtimeEnd.Value);
                }
                if (condition.UpdTimeBegin.HasValue)
                {
                    query = query.Where(q => q.UpdTime >= condition.UpdTimeBegin.Value);
                }
                if (condition.UpdTimeEnd.HasValue)
                {
                    query = query.Where(q => q.UpdTime < condition.UpdTimeEnd.Value);
                }
                if (condition.Status.HasValue)
                {
                    query = query.Where(q => q.Status == condition.Status.Value);
                }
                if (condition.Adduser.HasValue)
                {
                    query = query.Where(q => q.AddUser == condition.Adduser.Value);
                }
                if (condition.UpdUser.HasValue)
                {
                    query = query.Where(q => q.UpdUser == condition.UpdUser.Value);
                }
                if (condition.Type.HasValue)
                {
                    query = query.Where(q => q.Type == condition.Type.Value);
                }
                if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(q => q.Name.Contains(condition.Name));
                }
                if (condition.IsRecommend.HasValue)
                {
                    query = query.Where(q => q.IsRecommend == condition.IsRecommend.Value);
                }
                if (!string.IsNullOrEmpty(condition.Subtitte))
                {
                    query = query.Where(q => q.Subtitte.Contains(condition.Subtitte));
                }
                if (condition.Ids != null && condition.Ids.Any())
                {
                    query = query.Where(q => condition.Ids.Contains(q.Id));
                }
                if (condition.Categorys != null)//传进来的是二级 查他下面的三级所有商品
                {
                    query = query.Where(q => q.Category.Father.Id == condition.Categorys.Id);
                }


                if (condition.CategoryId.HasValue && condition.CategoryId != 0)// 传进来的是3级
                {
                    query = query.Where(q => q.Category.Id == condition.CategoryId);
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