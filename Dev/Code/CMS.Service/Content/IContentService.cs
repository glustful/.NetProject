using System.Linq;
using CMS.Entity.Model;
using YooPoon.Core.Autofac;

namespace CMS.Service.Content
{
	public interface IContentService : IDependency
	{
		ContentEntity Create (ContentEntity entity);

		bool Delete(ContentEntity entity);

		ContentEntity Update (ContentEntity entity);
		ContentEntity GetContentById (int id);

		IQueryable<ContentEntity> GetContentsByCondition(ContentSearchCondition condition);

		int GetContentCount (ContentSearchCondition condition);
	}
}