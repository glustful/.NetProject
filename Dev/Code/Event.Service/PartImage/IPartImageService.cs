using System.Collections.Generic;
using System.Linq;
using Event.Entity.Model;
using YooPoon.Core.Autofac;

namespace Event.Service.PartImage
{
	public interface IPartImageService : IDependency
	{
		PartImageEntity Create (PartImageEntity entity);

		bool Delete(PartImageEntity entity);

		PartImageEntity Update (PartImageEntity entity);

		PartImageEntity GetPartImageById (int id);

		IQueryable<PartImageEntity> GetPartImagesByCondition(PartImageSearchCondition condition);

		int GetPartImageCount (PartImageSearchCondition condition);

        List<PartImageEntity> GetPartImageByCrowdId(int crowdId);
	}
}