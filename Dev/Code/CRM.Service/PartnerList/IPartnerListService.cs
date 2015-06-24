using System.Collections.Generic;
using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.PartnerList
{
	public interface IPartnerListService : IDependency
	{
		PartnerListEntity Create (PartnerListEntity entity);

		bool Delete(PartnerListEntity entity);

		PartnerListEntity Update (PartnerListEntity entity);

		PartnerListEntity GetPartnerListById (int id);

		IQueryable<PartnerListEntity> GetPartnerListsByCondition(PartnerListSearchCondition condition);

		int GetPartnerListCount (PartnerListSearchCondition condition);

        /// <summary>
        /// 查询经纪人下属的合伙人列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<PartnerListEntity> GetInviteByBroker(int id);

        /// <summary>
        /// 查询经纪人收到的邀请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<PartnerListEntity> GetInviteForBroker(int id,EnumPartnerType type);
	}
}