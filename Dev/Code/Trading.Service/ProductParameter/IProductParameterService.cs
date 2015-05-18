using System.Linq;
using Trading.Entity.Model;
using YooPoon.Core.Autofac;

namespace Trading.Service.ProductParameter
{
	public interface IProductParameterService : IDependency
	{
		ProductParameterEntity Create (ProductParameterEntity entity);

		bool Delete(ProductParameterEntity entity);

		ProductParameterEntity Update (ProductParameterEntity entity);

		ProductParameterEntity GetProductParameterById (int id);

		IQueryable<ProductParameterEntity> GetProductParametersByCondition(ProductParameterSearchCondition condition);

		int GetProductParameterCount (ProductParameterSearchCondition condition);

        IQueryable<ProductParameterEntity> GetProductParametersByProduct(int productId);
	}
}