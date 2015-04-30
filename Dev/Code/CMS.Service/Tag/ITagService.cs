using System.Linq;
using CMS.Entity.Model;
using YooPoon.Core.Autofac;

namespace CMS.Service.Tag
{
	public interface ITagService : IDependency
	{
		TagEntity Create (TagEntity entity);

		bool Delete(TagEntity entity);

		TagEntity Update (TagEntity entity);
		TagEntity GetTagById (int id);

		IQueryable<TagEntity> GetTagsByCondition(TagSearchCondition condition);

		int GetTagCount (TagSearchCondition condition);
	}
}