using System.Linq;
using Trading.Entity.Model;
using YooPoon.Core.Autofac;

namespace Trading.Service.Parameter
{
	public interface IParameterService : IDependency
	{
		ParameterEntity Create (ParameterEntity entity);

		bool Delete(ParameterEntity entity);

		ParameterEntity Update (ParameterEntity entity);

		ParameterEntity GetParameterById (int id);

		IQueryable<ParameterEntity> GetParametersByCondition(ParameterSearchCondition condition);

		int GetParameterCount (ParameterSearchCondition condition);
        IQueryable<ParameterEntity> GetParameterEntitysByClassifyId(int classifyId);
	}
}