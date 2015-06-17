using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Event.Entity.Model;
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
            ////遍历出项目表中的所有项目
            List<CrowdEntity> arr = _crowdService.GetCrowdsByCondition(sech).ToList();
            foreach (var corwdlist in arr)
            {
                //这里是获取到每个项目表对应图片表中的图片list
                var CrowdDataImgList = _partImageService.GetPartImageByCrowdId(corwdlist.Id);
                //这是取得图片list中的对应项目数据
                var CrowdData = CrowdDataImgList[0].Crowd;
            }
            return View();
        }
    }
}