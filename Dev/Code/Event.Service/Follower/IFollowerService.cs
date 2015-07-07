using System.Linq;
using Event.Entity.Model;
using YooPoon.Core.Autofac;

namespace Event.Service.Follower
{
	public interface IFollowerService : IDependency
	{
		FollowerEntity Create (FollowerEntity entity);

		bool Delete(FollowerEntity entity);

		FollowerEntity Update (FollowerEntity entity);

		FollowerEntity GetFollowerById (int id);

		IQueryable<FollowerEntity> GetFollowersByCondition(FollowerSearchCondition condition);

		int GetFollowerCount (FollowerSearchCondition condition);
	}
}