using System.Linq;
using Community.Entity.Model.Member;
using YooPoon.Core.Autofac;

namespace Community.Service.Member
{
	public interface IMemberService : IDependency
	{
		MemberEntity Create (MemberEntity entity);

		bool Delete(MemberEntity entity);

		MemberEntity Update (MemberEntity entity);

		MemberEntity GetMemberById (int id);

		IQueryable<MemberEntity> GetMembersByCondition(MemberSearchCondition condition);

		int GetMemberCount (MemberSearchCondition condition);
	}
}