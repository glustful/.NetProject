using System.Linq;
using Trading.Entity.Model;
using YooPoon.Core.Autofac;

namespace Trading.Service.Product
{
	public interface IProductService : IDependency
	{
		ProductEntity Create (ProductEntity entity);

		bool Delete(ProductEntity entity);

		ProductEntity Update (ProductEntity entity);

		ProductEntity GetProductById (int id);

		IQueryable<ProductEntity> GetProductsByCondition(ProductSearchCondition condition);

		int GetProductCount (ProductSearchCondition condition);

        IQueryable<ProductEntity> GetProductsByClassify(int ClassifyId);

        IQueryable<ProductEntity> GetProductsByProductBrand(int BrandId);
	}
}