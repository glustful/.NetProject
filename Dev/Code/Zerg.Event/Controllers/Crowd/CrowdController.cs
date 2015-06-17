using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Event.Entity.Model;
using Event.Models;
using Event.Service.Crowd;
using Event.Service.Follower;
using Event.Service.PartImage;
using YooPoon.Core.Site;

namespace Zerg.Event.Controllers.Crowd
{
    public class CrowdController : Controller
    {
        private readonly ICrowdService _crowdService;
        private readonly IPartImageService _partImageService;
        public CrowdController(ICrowdService crowdService,
            IPartImageService partImageService
            )
        {
            _crowdService = crowdService;
            _partImageService = partImageService;
        }


        // GET: Crowd
        //public ActionResult Index()
        //{
        //    return View();
        //}
        [HttpGet]
        public ActionResult crowd()
        {
            //查询项目表所有的数据
            var sech = new CrowdSearchCondition
            {
                OrderBy = EnumCrowdSearchOrderBy.OrderById,
                //Statuss = new[] { status }
            };

            var list = _crowdService.GetCrowdsByCondition(sech).Select(p=>new CrowdModel
            {
                Ttitle = p.Ttitle
            });

            return View(list);
        }
    }
}