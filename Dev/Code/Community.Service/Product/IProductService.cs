using System.Linq;
using Community.Entity.Model.Product;
using YooPoon.Core.Autofac;

namespace Community.Service.Product
{
	public interface IProductService : IDependency
	{
		ProductEntity Create (ProductEntity entity);

		bool Delete(ProductEntity entity);

		ProductEntity Update (ProductEntity entity);

		ProductEntity GetProductById (int id);

		IQueryable<ProductEntity> GetProductsByCondition(ProductSearchCondition condition);

		int GetProductCount (ProductSearchCondition condition);
	}
}