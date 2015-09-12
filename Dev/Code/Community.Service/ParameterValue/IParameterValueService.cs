using System.Linq;
using Community.Entity.Model.ParameterValue;
using YooPoon.Core.Autofac;

namespace Community.Service.ParameterValue
{
	public interface IParameterValueService : IDependency
	{
		ParameterValueEntity Create (ParameterValueEntity entity);

		bool Delete(ParameterValueEntity entity);

		ParameterValueEntity Update (ParameterValueEntity entity);

		ParameterValueEntity GetParameterValueById (int id);

		IQueryable<ParameterValueEntity> GetParameterValuesByCondition(ParameterValueSearchCondition condition);

		int GetParameterValueCount (ParameterValueSearchCondition condition);
	}
}