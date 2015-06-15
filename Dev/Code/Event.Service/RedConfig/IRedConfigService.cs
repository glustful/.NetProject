using System.Linq;
using Event.Entity.Model;
using YooPoon.Core.Autofac;

namespace Event.Service.RedConfig
{
	public interface IRedConfigService : IDependency
	{
		RedConfigEntity Create (RedConfigEntity entity);

		bool Delete(RedConfigEntity entity);

		RedConfigEntity Update (RedConfigEntity entity);

		RedConfigEntity GetRedConfigById (int id);

		IQueryable<RedConfigEntity> GetRedConfigsByCondition(RedConfigSearchCondition condition);

		int GetRedConfigCount (RedConfigSearchCondition condition);
	}
}