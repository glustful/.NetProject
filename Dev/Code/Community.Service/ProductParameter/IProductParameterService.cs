using System.Collections.Generic;
using System.Linq;
using Community.Entity.Model.ProductParameter;
using YooPoon.Core.Autofac;

namespace Community.Service.ProductParameter
{
	public interface IProductParameterService : IDependency
	{
		ProductParameterEntity Create (ProductParameterEntity entity);
        bool BulkCreate(List<ProductParameterEntity> entities);
		bool Delete(ProductParameterEntity entity);

		ProductParameterEntity Update (ProductParameterEntity entity);

		ProductParameterEntity GetProductParameterById (int id);

		IQueryable<ProductParameterEntity> GetProductParametersByCondition(ProductParameterSearchCondition condition);

		int GetProductParameterCount (ProductParameterSearchCondition condition);
	}
}