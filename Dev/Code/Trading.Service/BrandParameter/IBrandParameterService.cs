using System.Linq;
using Trading.Entity.Model;
using YooPoon.Core.Autofac;

namespace Trading.Service.BrandParameter
{
	public interface IBrandParameterService : IDependency
	{
		BrandParameterEntity Create (BrandParameterEntity entity);

		bool Delete(BrandParameterEntity entity);

		BrandParameterEntity Update (BrandParameterEntity entity);

		BrandParameterEntity GetBrandParameterById (int id);

		IQueryable<BrandParameterEntity> GetBrandParametersByCondition(BrandParameterSearchCondition condition);

		int GetBrandParameterCount (BrandParameterSearchCondition condition);

        IQueryable<BrandParameterEntity> GetBrandParametersByBrandId(int brandId);
	}
}