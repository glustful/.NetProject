using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.LevelConfig
{
	public interface ILevelConfigService : IDependency
	{
		LevelConfigEntity Create (LevelConfigEntity entity);

		bool Delete(LevelConfigEntity entity);

		LevelConfigEntity Update (LevelConfigEntity entity);

		LevelConfigEntity GetLevelConfigById (int id);

		IQueryable<LevelConfigEntity> GetLevelConfigsByCondition(LevelConfigSearchCondition condition);

		int GetLevelConfigCount (LevelConfigSearchCondition condition);
	}
}