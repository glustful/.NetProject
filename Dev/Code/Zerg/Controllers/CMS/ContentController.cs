using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using CMS.Service.Content;
using YooPoon.Core.Site;
using YooPoon.WebFramework.User.Services;
using CMS.Entity.Model;

namespace Zerg.Controllers.CMS
{
    public class ContentController : ApiController
    {
        private readonly IContentService _contentService;
        private readonly IUserService _userService;


        public ContentController(IContentService contentService,IUserService userService)
        {
            _contentService = contentService;
            _userService = userService;
        }


        public string Get()
        {
            _contentService.GetContentById(1);
            _userService.GetUserByName("a");
            return "0";
        }
    }
}
