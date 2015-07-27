using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Event.Entity.Model;
using Event.Service.Crowd;
using Event.Service.Follower;
using YooPoon.Core.Site;

namespace Zerg.Event.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFollowerService _followerService;
        private readonly IWorkContext _workContext;
        private readonly ICrowdService _crowdService;

        public HomeController(IFollowerService followerService,
            ICrowdService crowdService,
            IWorkContext workContext
            )
        {
            _workContext = workContext;
            _followerService = followerService;
            _crowdService = crowdService;
        }

        /// <summary>
        /// 微信服务器交互入口，用于微信服务器和本程序交互
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            #region 微信首次接入认证 杨定鹏 2015年7月7日15:53:10
            
            #endregion

            return View();
        }
    }
}