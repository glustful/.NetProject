using System.Linq;
using Trading.Entity.Model;
using YooPoon.Core.Autofac;

namespace Trading.Service.Classify
{
	public interface IClassifyService : IDependency
	{
		ClassifyEntity Create (ClassifyEntity entity);

		bool Delete(ClassifyEntity entity);

		ClassifyEntity Update (ClassifyEntity entity);

		ClassifyEntity GetClassifyById (int id);

		IQueryable<ClassifyEntity> GetClassifysByCondition(ClassifySearchCondition condition);

		int GetClassifyCount (ClassifySearchCondition condition);

        IQueryable<ClassifyEntity> GetClassifysBySuperClassify(int ClassifyId);
	}
}