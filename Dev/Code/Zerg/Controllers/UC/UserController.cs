using System;
using System.Net.Http;
using System.Web.Http;
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

        public UserController(IUserService userService,IAuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Login([FromBody]UserModel model)
        {
            var user = _userService.GetUserByName(model.UserName);
            if (user == null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "用户名或密码错误"));
            }
            if (PasswordHelper.ValidatePasswordHashed(user, model.Password))
            {
                _authenticationService.SignIn(user, model.Remember);
                return PageHelper.toJson(PageHelper.ReturnValue(true, "登陆成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "用户名或密码错误"));
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
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
        public HttpResponseMessage SignUp([FromBody] UserModel model)
        {
            var user = _userService.GetUserByName(model.UserName);
            if (user != null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "用户名已经存在"));
            }
            var  newUser = new UserBase
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
    }
}
