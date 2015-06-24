using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.MessageConfig
{
	public interface IMessageConfigService : IDependency
	{
		MessageConfigEntity Create (MessageConfigEntity entity);

		bool Delete(MessageConfigEntity entity);

		MessageConfigEntity Update (MessageConfigEntity entity);

		MessageConfigEntity GetMessageConfigById (int id);

        MessageConfigEntity GetMessageConfigByName(string  name);

		IQueryable<MessageConfigEntity> GetMessageConfigsByCondition(MessageConfigSearchCondition condition);

		int GetMessageConfigCount (MessageConfigSearchCondition condition);
	}
}