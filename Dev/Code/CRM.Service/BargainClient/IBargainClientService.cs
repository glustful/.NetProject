using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.BargainClient
{
	public interface IBargainClientService : IDependency
	{
		BargainClientEntity Create (BargainClientEntity entity);

		bool Delete(BargainClientEntity entity);

		BargainClientEntity Update (BargainClientEntity entity);

		BargainClientEntity GetBargainClientById (int id);

		IQueryable<BargainClientEntity> GetBargainClientsByCondition(BargainClientSearchCondition condition);

		int GetBargainClientCount (BargainClientSearchCondition condition);
	}
}