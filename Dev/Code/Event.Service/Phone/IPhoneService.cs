using System.Linq;
using Event.Entity.Model;
using YooPoon.Core.Autofac;

namespace Event.Service.Phone
{
	public interface IPhoneService : IDependency
	{
		PhoneEntity Create (PhoneEntity entity);

		bool Delete(PhoneEntity entity);

		PhoneEntity Update (PhoneEntity entity);

		PhoneEntity GetPhoneById (int id);

		IQueryable<PhoneEntity> GetPhonesByCondition(PhoneSearchCondition condition);

		int GetPhoneCount (PhoneSearchCondition condition);
	}
}