using System.Linq;
using CRM.Entity.Model;
using YooPoon.Core.Autofac;

namespace CRM.Service.InvitedCode
{
    public interface IInviteCodeService : IDependency
    {
        InviteCodeEntity Create(InviteCodeEntity entity);

        bool Delete(InviteCodeEntity entity);

        InviteCodeEntity Update(InviteCodeEntity entity);

        InviteCodeEntity GetInviteCodeById(int id);

        IQueryable<InviteCodeEntity> GetInviteCodeByCondition(InviteCodeSearchCondition condition);
        int GetInviteCodeByCount(InviteCodeSearchCondition condition);
        InviteCodeEntity GetInviteCodebyBrokerId(int broker);
       
    }
}