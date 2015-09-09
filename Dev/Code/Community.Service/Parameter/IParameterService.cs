using System.Linq;
using Community.Entity.Model.Parameter;
using YooPoon.Core.Autofac;

namespace Community.Service.Parameter
{
	public interface IParameterService : IDependency
	{
		ParameterEntity Create (ParameterEntity entity);

		bool Delete(ParameterEntity entity);

		ParameterEntity Update (ParameterEntity entity);

		ParameterEntity GetParameterById (int id);

		IQueryable<ParameterEntity> GetParametersByCondition(ParameterSearchCondition condition);

		int GetParameterCount (ParameterSearchCondition condition);
	}
}