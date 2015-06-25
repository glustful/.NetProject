using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Event.Entity.Model;
using Event.Models;
using Event.Service.Crowd;
using Event.Service.Discount;
using Event.Service.Follower;
using Event.Service.PartImage;
using YooPoon.Core.Site;

namespace Zerg.Event.Controllers.Crowd
{
    public class CrowdController : Controller
    {
        private readonly ICrowdService _crowdService;
        private readonly IPartImageService _partImageService;
        private readonly IDiscountService _discountService;
        public CrowdController(ICrowdService crowdService,IPartImageService partImageService, IDiscountService discountService)
        {
            _discountService = discountService;
            _crowdService = crowdService;
            _partImageService = partImageService;
        }
        #region 加载获取项目信息
        [HttpGet]
        public ActionResult crowd()
        {
            //查询项目表所有的数据
            var sech = new CrowdSearchCondition
            {
                OrderBy = EnumCrowdSearchOrderBy.OrderById,
                //Statuss = new[] { status }
            };
            var list = _crowdService.GetCrowdsByCondition(sech).Select(p => new
            {
                //项目表
                p.Ttitle,
                p.Id,
                p.Status,
                p.Intro,
                p.Starttime,
                p.Endtime,
                p.Uptime,
                p.Upuser,
                p.Adduser,
                p.Addtime
            }).ToList().Select(a=>new CrowdModel
            {
                //返回数据
                Id = a.Id,
                Ttitle = a.Ttitle,
                Intro = a.Intro,
                Endtime = a.Endtime,
                Starttime = a.Starttime,
                Status = a.Status,
                ImgList = _partImageService.GetPartImageByCrowdId(a.Id),
                Dislist = _discountService.GetDiscountByCrowdId(a.Id),
            });

            return View(list);
        }
        #endregion
    }
}