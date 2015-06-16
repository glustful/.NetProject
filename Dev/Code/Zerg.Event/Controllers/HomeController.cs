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

        // GET: Home
        public ActionResult Index()
        {
            var model = new CrowdEntity();
            model.Ttitle = "asd";
            model.Intro = "asd";
            model.Starttime = DateTime.Now;
            model.Endtime = DateTime.Now;
            model.Status = 1;
            model.Adduser = 1;
            model.Addtime = DateTime.Now;
            model.Upuser = 1;
            model.Addtime = DateTime.Now;
            _crowdService.Create(model);
            return View();
        }
    }
}