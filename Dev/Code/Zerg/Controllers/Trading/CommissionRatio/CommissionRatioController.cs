using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Trading.Entity.Entity.CommissionRatio;
using Trading.Service.CommissionRatio;
using Zerg.Common;

namespace Zerg.Controllers.Trading.CommissionRatio
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    public class CommissionRatioController : ApiController
    {
        private readonly ICommissionRatioService _commissionRatioService;

        public CommissionRatioController(ICommissionRatioService commissionRatioService)
        {
            _commissionRatioService = commissionRatioService;
        }
        /// <summary>
        /// 获取佣金比例
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage Index()
        {
            var con=new CommissionRatioSearchCondition
            {
                Page = 1,
                PageSize = 1
            };
            var commissionRatio = _commissionRatioService.GetCommissionRatioCondition(con).Select(p=>new Models.Trading.CommissionRatio.CommissionRatio
            {
                Id = p.Id,
                RecCfbScale = p.RecCfbScale,
                RecAgentScale = p.RecAgentScale,
                TakeCfbScale = p.TakeCfbScale,
                TakeAgentScale = p.TakeAgentScale,
                RecPartnerScale = p.RecPartnerScale,
                TakePartnerScale = p.TakePartnerScale
            }).FirstOrDefault();
            if (commissionRatio==null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据不存在"));
            }
            return PageHelper.toJson(commissionRatio);
        }
        /// <summary>
        /// 修改佣金比例
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage CreateOrUpdate(Models.Trading.CommissionRatio.CommissionRatio model)
        {
            if (model.Id>0)
            {
                var oldModel = _commissionRatioService.GetById(model.Id);
                oldModel.RecAgentScale = model.RecAgentScale;
                oldModel.RecCfbScale = model.RecCfbScale;
                oldModel.TakeAgentScale = model.TakeAgentScale;
                oldModel.TakeCfbScale = model.TakeCfbScale;
                oldModel.RecPartnerScale = model.RecPartnerScale;
                oldModel.TakePartnerScale = model.TakePartnerScale;
                if (_commissionRatioService.Update(oldModel) != null)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功"));
                }
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败"));
            }
            else
            {
               var newModel=new CommissionRatioEntity
               {
                   RecCfbScale = model.RecCfbScale,
                   RecAgentScale = model.RecAgentScale,
                   TakeCfbScale = model.TakeCfbScale,
                   TakeAgentScale= model.TakeAgentScale,
                   TakePartnerScale =model.TakePartnerScale,
                   RecPartnerScale = model.RecPartnerScale
               };
                if (_commissionRatioService.Create(newModel) != null)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "数据添加成功"));
                }
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据添加失败"));
            }
        }
    }
}
