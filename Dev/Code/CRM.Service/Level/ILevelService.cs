using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.Level
{
	public interface ILevelService : IDependency
	{
		LevelEntity Create (LevelEntity entity);

		bool Delete(LevelEntity entity);

		LevelEntity Update (LevelEntity entity);

		LevelEntity GetLevelById (int id);

		IQueryable<LevelEntity> GetLevelsByCondition(LevelSearchCondition condition);

		int GetLevelCount (LevelSearchCondition condition);
	}
}