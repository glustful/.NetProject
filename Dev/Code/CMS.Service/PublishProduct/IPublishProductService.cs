using System.Linq;
using CMS.Entity.Model;
using YooPoon.Core.Autofac;

namespace CMS.Service.PublishProduct
{
	public interface IPublishProductService : IDependency
	{
		PublishProductEntity Create (PublishProductEntity entity);

		bool Delete(PublishProductEntity entity);

		PublishProductEntity Update (PublishProductEntity entity);

		PublishProductEntity GetPublishProductById (int id);

		IQueryable<PublishProductEntity> GetPublishProductsByCondition(PublishProductSearchCondition condition);

		int GetPublishProductCount (PublishProductSearchCondition condition);
	}
}