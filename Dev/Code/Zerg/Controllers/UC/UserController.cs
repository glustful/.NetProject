using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using YooPoon.Core.Site;
using YooPoon.WebFramework.Authentication;
using YooPoon.WebFramework.User;
using YooPoon.WebFramework.User.Entity;
using YooPoon.WebFramework.User.Services;
using Zerg.Common;
using Zerg.Models.UC;

namespace Zerg.Controllers.UC
{
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IWorkContext _workContext;

        public UserController(IUserService userService, IAuthenticationService authenticationService, IWorkContext workContext)
        {
            _userService = userService;
            _authenticationService = authenticationService;
            _workContext = workContext;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage Login([FromBody]UserModel model)
        {
            var user = _userService.GetUserByName(model.UserName);
            if (user == null)
                return PageHelper.toJson(PageHelper.ReturnValue(false, "用户名或密码错误"));
            if (!PasswordHelper.ValidatePasswordHashed(user, model.Password))
                return PageHelper.toJson(PageHelper.ReturnValue(false, "用户名或密码错误"));
            _authenticationService.SignIn(user, model.Remember);
            return PageHelper.toJson(PageHelper.ReturnValue(true, "登陆成功", new
            {
                Roles = user.UserRoles.Select(r => new { r.Role.RoleName }).ToArray()
            }));
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage Logout()
        {
            _authenticationService.SignOut();
            return PageHelper.toJson(PageHelper.ReturnValue(true, "登出成功"));
        }
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage SignUp([FromBody] UserModel model)
        {
            var user = _userService.GetUserByName(model.UserName);
            if (user != null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "用户名已经存在"));
            }
            var newUser = new UserBase
            {
                UserName = model.UserName,
                Password = model.Password,
                RegTime = DateTime.Now,
                NormalizedName = model.UserName.ToLower(),
                Status = 0
            };
            PasswordHelper.SetPasswordHashed(newUser, model.Password);
            if (_userService.InsertUser(newUser).Id <= 0)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "注册用户失败，请重试"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(true, "注册成功"));
        }

        [HttpGet]
        [EnableCors("*", "*", "*", SupportsCredentials = true)]
        public HttpResponseMessage GetCurrentUser()
        {
            var user = (UserBase)_workContext.CurrentUser;
            return PageHelper.toJson(user == null ? PageHelper.ReturnValue(false, "获取用户失败，请检查是否登陆") :
                PageHelper.ReturnValue(true, "获取用户成功", new
                {
                    user.Id, 
                    user.UserName, 
                    Roles = user.UserRoles.Select(r => new
                    {
                        r.Role.RoleName
                    }).ToArray()
                }));
        }
    }
}
