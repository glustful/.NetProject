using System.Linq;
using Community.Entity.Model.ProductComment;
using YooPoon.Core.Autofac;

namespace Community.Service.ProductComment
{
	public interface IProductCommentService : IDependency
	{
		ProductCommentEntity Create (ProductCommentEntity entity);

		bool Delete(ProductCommentEntity entity);

		ProductCommentEntity Update (ProductCommentEntity entity);

		ProductCommentEntity GetProductCommentById (int id);

		IQueryable<ProductCommentEntity> GetProductCommentsByCondition(ProductCommentSearchCondition condition);

		int GetProductCommentCount (ProductCommentSearchCondition condition);
	}
}