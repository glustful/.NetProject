using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Entity.Entity.CommissionRatio;
using YooPoon.Core.Autofac;

namespace Trading.Service.CommissionRatio
{
    public interface ICommissionRatioService:IDependency
    {
        CommissionRatioEntity Create(CommissionRatioEntity entity);
        CommissionRatioEntity Update(CommissionRatioEntity entity);
        bool Delete(CommissionRatioEntity entity);
        CommissionRatioEntity GetById(int id);
        IQueryable<CommissionRatioEntity> GetCommissionRatioCondition(CommissionRatioSearchCondition condition);
        int GetCommissionRatioCount(CommissionRatioSearchCondition condition);
    }
}
