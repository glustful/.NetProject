using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.MessageDetail
{
	public interface IMessageDetailService : IDependency
	{
		MessageDetailEntity Create (MessageDetailEntity entity);

		bool Delete(MessageDetailEntity entity);

		MessageDetailEntity Update (MessageDetailEntity entity);

		MessageDetailEntity GetMessageDetailById (int id);

		IQueryable<MessageDetailEntity> GetMessageDetailsByCondition(MessageDetailSearchCondition condition);

		int GetMessageDetailCount (MessageDetailSearchCondition condition);
	}
}