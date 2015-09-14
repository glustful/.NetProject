using System.Linq;
using Community.Entity.Model.MemberAddress;
using YooPoon.Core.Autofac;

namespace Community.Service.MemberAddress
{
	public interface IMemberAddressService : IDependency
	{
		MemberAddressEntity Create (MemberAddressEntity entity);

		bool Delete(MemberAddressEntity entity);

		MemberAddressEntity Update (MemberAddressEntity entity);

		MemberAddressEntity GetMemberAddressById (int id);

		IQueryable<MemberAddressEntity> GetMemberAddresssByCondition(MemberAddressSearchCondition condition);

		int GetMemberAddressCount (MemberAddressSearchCondition condition);
	}
}