using System.Linq;
using CMS.Entity.Model;
using YooPoon.Core.Autofac;

namespace CMS.Service.Resource
{
	public interface IResourceService : IDependency
	{
		ResourceEntity Create (ResourceEntity entity);

		bool Delete(ResourceEntity entity);

		ResourceEntity Update (ResourceEntity entity);

		ResourceEntity GetResourceById (int id);

		IQueryable<ResourceEntity> GetResourcesByCondition(ResourceSearchCondition condition);

		int GetResourceCount (ResourceSearchCondition condition);
	}
}