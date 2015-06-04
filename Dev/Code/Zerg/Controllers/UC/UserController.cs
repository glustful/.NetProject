using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CRM.Entity.Model;
using CRM.Service.Broker;
using YooPoon.Core.Site;
using YooPoon.WebFramework.Authentication;
using YooPoon.WebFramework.User;
using YooPoon.WebFramework.User.Entity;
using YooPoon.WebFramework.User.Services;
using Zerg.Common;
using Zerg.Models.CRM;
using Zerg.Models.UC;

namespace Zerg.Controllers.UC
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IWorkContext _workContext;
        private readonly IBrokerService _brokerService;

        public UserController(IUserService userService, IAuthenticationService authenticationService, IWorkContext workContext,IBrokerService brokerService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
            _workContext = workContext;
            _brokerService = brokerService;
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

        /// <summary>
        /// 前端添加新用户
        /// </summary>
        /// <param name="brokerModel"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddBroker([FromBody]BrokerModel brokerModel)
        {
            #region UC用户创建 杨定鹏 2015年5月28日14:52:48
            var user = _userService.GetUserByName(brokerModel.UserName);

            var condition = new BrokerSearchCondition
            {
                OrderBy = EnumBrokerSearchOrderBy.OrderById,
                Phone = brokerModel.Phone
            };

            //判断user表和Broker表中是否存在用户名
            var user2 = _brokerService.GetBrokerCount(condition);
            if (user != null || user2 != 0)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "用户名已经存在"));
            }
            var newUser = new UserBase
            {
                UserName = brokerModel.UserName,
                Password = brokerModel.Password,
                RegTime = DateTime.Now,
                NormalizedName = brokerModel.UserName.ToLower(),
                Status = 0
            };
            PasswordHelper.SetPasswordHashed(newUser, brokerModel.Password);
            if (_userService.InsertUser(newUser).Id <= 0)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "注册用户失败，请重试"));
            }

            #endregion

            #region Broker用户创建 杨定鹏 2015年5月28日14:53:32

            var model = new BrokerEntity();
            model.UserId = _userService.GetUserByName(brokerModel.UserName).Id;
            model.Brokername = brokerModel.Brokername;
            model.Phone = brokerModel.Phone;
            model.Totalpoints = 0;
            model.Amount = 0;
            model.Usertype = brokerModel.UserType;
            model.Regtime = DateTime.Now;
            model.State = 1;
            model.Adduser = 0;
            model.Addtime = DateTime.Now;
            model.Upuser = 0;
            model.Uptime = DateTime.Now;

            //缺少等级

            _brokerService.Create(model);

            #endregion

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
        [HttpGet]
        
        public HttpResponseMessage GetUserList(string userName=null,int page=1,int pageSize=10)
        {
            var userCondition = new UserSearchCondition
            {             
                UserName = userName,
                Page = page,
                PageSize = pageSize
            };
           
            var userList = _userService.GetUserByCondition(userCondition).Select(a=>new UserModel
            {
                Id = a.Id,
                UserName = a.UserName,
                Status =a.Status
            }).ToList();
            var userCount = _userService.GetUserCountByCondition(userCondition);
            return PageHelper.toJson(new{ List=userList, Condition=userCondition, TotalCount=userCount});
        }
        [HttpGet]
        public HttpResponseMessage Detailed(int id)
        {
            var user = _userService.FindUser(id);
            if (user == null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "该数据不存在！"));
            }
            var userDetail = new UserModel
            {
                Id = user.Id,
                UserName = user.UserName,              
                Status =user.Status
            };
            return PageHelper.toJson(userDetail);
        }

        [HttpPost]
        public HttpResponseMessage ChangePassword([FromBody]ChangePasswordModel model)
        {
            var user =(UserBase) _workContext.CurrentUser;
            if (user!=null && PasswordHelper.ValidatePasswordHashed(user,model.OldPassword))
            {
                PasswordHelper.SetPasswordHashed(user, model.NewPassword);
                _userService.ModifyUser(user);
                return PageHelper.toJson(PageHelper.ReturnValue(true,"数据更新成功！"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败！"));
        }
        [HttpGet]
        public HttpResponseMessage Delete(int id)
        {
            var user = _userService.FindUser(id);
            if (_userService.DeleteUser(user))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据删除成功"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据删除失败！"));
        }
    }
}
