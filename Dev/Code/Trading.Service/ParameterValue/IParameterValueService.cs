using System.Linq;
using Trading.Entity.Model;
using YooPoon.Core.Autofac;

namespace Trading.Service.ParameterValue
{
    public interface IParameterValueService : IDependency
    {
        ParameterValueEntity Create(ParameterValueEntity entity);

        bool Delete(ParameterValueEntity entity);

        ParameterValueEntity Update(ParameterValueEntity entity);

        ParameterValueEntity GetParameterValueById(int id);

        IQueryable<ParameterValueEntity> GetParameterValuesByCondition(ParameterValueSearchCondition condition);

        int GetParameterValueCount(ParameterValueSearchCondition condition);

        IQueryable<ParameterValueEntity> GetParameterValuesByParameter(int ParameterId);
       
    }
}