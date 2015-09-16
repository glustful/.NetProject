using System.Linq;
using Community.Entity.Model.Category;
using YooPoon.Core.Autofac;

namespace Community.Service.Category
{
	public interface ICategoryService : IDependency
	{
		CategoryEntity Create (CategoryEntity entity);

		bool Delete(CategoryEntity entity);

		CategoryEntity Update (CategoryEntity entity);

		CategoryEntity GetCategoryById (int id);
        CategoryEntity GetCategoryByFatherId(int id);

		IQueryable<CategoryEntity> GetCategorysByCondition(CategorySearchCondition condition);

		int GetCategoryCount (CategorySearchCondition condition);
        IQueryable<CategoryEntity> GetCategorysBySuperFather(int father);
	}
}