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
        /// ��ѯ�����������ĺϻ����б�
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<PartnerListEntity> GetInviteByBroker(int id);

        /// <summary>
        /// ��ѯ�������յ�������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<PartnerListEntity> GetInviteForBroker(int id,EnumPartnerType type);
	}
}