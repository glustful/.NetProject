using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.ClientInfo
{
	public interface IClientInfoService : IDependency
	{
		ClientInfoEntity Create (ClientInfoEntity entity);

		bool Delete(ClientInfoEntity entity);

		ClientInfoEntity Update (ClientInfoEntity entity);

		ClientInfoEntity GetClientInfoById (int id);

		IQueryable<ClientInfoEntity> GetClientInfosByCondition(ClientInfoSearchCondition condition);

		int GetClientInfoCount (ClientInfoSearchCondition condition);
	}
}