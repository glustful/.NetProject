using System.Linq;
using CMS.Entity.Model;
using YooPoon.Core.Autofac;

namespace CMS.Service.Advertisement
{
	public interface IAdvertisementService : IDependency
	{
		AdvertisementEntity Create (AdvertisementEntity entity);

		bool Delete(AdvertisementEntity entity);

		AdvertisementEntity Update (AdvertisementEntity entity);

		AdvertisementEntity GetAdvertisementById (int id);

		IQueryable<AdvertisementEntity> GetAdvertisementsByCondition(AdvertisementSearchCondition condition);

		int GetAdvertisementCount (AdvertisementSearchCondition condition);
	}
}