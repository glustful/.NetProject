using System.Linq;
using Trading.Entity.Model;
using YooPoon.Core.Autofac;

namespace Trading.Service.ProductBrand
{
	public interface IProductBrandService : IDependency
	{
		ProductBrandEntity Create (ProductBrandEntity entity);

		bool Delete(ProductBrandEntity entity);

		ProductBrandEntity Update (ProductBrandEntity entity);

		ProductBrandEntity GetProductBrandById (int id);

		IQueryable<ProductBrandEntity> GetProductBrandsByCondition(ProductBrandSearchCondition condition);

		int GetProductBrandCount (ProductBrandSearchCondition condition);
	}
}