using System.Linq;
using CMS.Entity.Model;
using YooPoon.Core.Autofac;

namespace CMS.Service.Setting
{
	public interface ISettingService : IDependency
	{
		SettingEntity Create (SettingEntity entity);

		bool Delete(SettingEntity entity);

		SettingEntity Update (SettingEntity entity);

		SettingEntity GetSettingById (int id);

	    SettingEntity GetSettingByKey(string key);

		IQueryable<SettingEntity> GetSettingsByCondition(SettingSearchCondition condition);

		int GetSettingCount (SettingSearchCondition condition);

	    bool CreateOrUpdateEntity(SettingEntity[] settings);
	}
}