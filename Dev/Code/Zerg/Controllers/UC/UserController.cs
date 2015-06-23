using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.UI.WebControls;
using CRM.Entity.Model;
using CRM.Service.Broker;
using CRM.Service.Level;
using YooPoon.Core.Site;
using YooPoon.WebFramework.Authentication;
using YooPoon.WebFramework.Authentication.Entity;
using YooPoon.WebFramework.User;
using YooPoon.WebFramework.User.Entity;
using YooPoon.WebFramework.User.Services;
using Zerg.Common;
using Zerg.Models.CRM;
using Zerg.Models.UC;
using YooPoon.Common.Encryption;
using CRM.Service.MessageDetail;
using CRM.Service.RecommendAgent;
using System.ComponentModel;

namespace Zerg.Controllers.UC
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
    [Description("用户管理类")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IWorkContext _workContext;
        private readonly IBrokerService _brokerService;
        private readonly IRoleService _roleService;
        private readonly ILevelService _levelService;
        private readonly IMessageDetailService _MessageService;
        private readonly IRecommendAgentService _recommendagentService;


        public UserController(IUserService userService,
            IAuthenticationService authenticationService,
            IWorkContext workContext,
            IBrokerService brokerService,
            IRoleService roleService,
            ILevelService levelService,
            IMessageDetailService MessageService,
            IRecommendAgentService recommendagentService
            )
        {
            _userService = userService;
            _authenticationService = authenticationService;
            _workContext = workContext;
            _brokerService = brokerService;
            _roleService = roleService;
            _levelService = levelService;
            _MessageService = MessageService;
            _recommendagentService = recommendagentService;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="model">登陆参数</param>
        /// <returns>登陆结果</returns>
        [Description("登陆")]
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
                user.Id,
                Roles = user.UserRoles.Select(r => new { r.Role.RoleName }).ToArray(),
                user.UserName
            }));
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns>注销结果</returns>
        [Description("用户注销")]
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
        /// <param name="model">用户参数</param>
        /// <returns>注册结果</returns>
        [Description("用户注册")]
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
        /// <param name="brokerModel">经纪人参数</param>
        /// <returns>添加经纪人结果</returns>
        [HttpPost]
        public HttpResponseMessage AddBroker([FromBody]BrokerModel brokerModel)
        {
            var validMsg = "";
            if (!brokerModel.ValidateModel(out validMsg))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, validMsg));
            }

            #region 验证码判断 解密
            var strDes = EncrypHelper.Decrypt(brokerModel.Hidm, "Hos2xNLrgfaYFY2MKuFf3g==");//解密
            string[] str = strDes.Split('$');
            string source = str[0];//获取验证码
            DateTime date = Convert.ToDateTime(str[1]);//获取发送验证码的时间
            DateTime dateNow = Convert.ToDateTime(DateTime.Now.ToLongTimeString());//获取当前时间
            TimeSpan ts = dateNow.Subtract(date);
            double secMinu = ts.TotalMinutes;//得到发送时间与现在时间的时间间隔分钟数
            if (secMinu > 3) //发送时间与接受时间是否大于3分钟
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "你已超过时间验证，请重新发送验证码！"));
            }
            else
            {
                if (brokerModel.MobileYzm != source)//判断验证码是否一致
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "验证码错误，请重新发送！"));
                }
            }

            #endregion

            #region 判断两次密码是否一致
            if (brokerModel.Password != brokerModel.SecondPassword)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "手机号不能为空"));
            }
            #endregion

            #region 判断邀请码是否存在真实  （brokerInfoController 中GetBrokerByInvitationCode方法也同一判断）
            MessageDetailEntity messageDetail = null;
            if (!string.IsNullOrEmpty(brokerModel.inviteCode))
            {

                MessageDetailSearchCondition messageSearchcondition = new MessageDetailSearchCondition
                {
                    InvitationCode = brokerModel.inviteCode,
                    Title = "推荐经纪人"
                };
                messageDetail = _MessageService.GetMessageDetailsByCondition(messageSearchcondition).FirstOrDefault();//判断邀请码是否存在
                if (messageDetail == null)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "邀请码错误！"));
                }
            }
            #endregion


            #region UC用户创建 杨定鹏 2015年5月28日14:52:48
            var user = _userService.GetUserByName(brokerModel.UserName);

            var condition = new BrokerSearchCondition
            {
                OrderBy = EnumBrokerSearchOrderBy.OrderById,
                Phone = brokerModel.Phone
            };

            //判断user表和Broker表中是否存在用户名
            int user2 = _brokerService.GetBrokerCount(condition);

            if (user2 != 0) return PageHelper.toJson(PageHelper.ReturnValue(false, "用户名已经存在"));

            var brokerRole = _roleService.GetRoleByName("user");

            //User权限缺少时自动添加
            if (brokerRole == null)
            {
                brokerRole = new Role
                {
                    RoleName = "user",
                    RolePermissions = null,
                    Status = RoleStatus.Normal,
                    Description = "刚注册的用户默认归为普通用户user"
                };
            }

            var newUser = new UserBase
            {
                UserName = brokerModel.UserName,
                Password = brokerModel.Password,
                RegTime = DateTime.Now,
                NormalizedName = brokerModel.UserName.ToLower(),
                //注册用户添加权限
                UserRoles = new List<UserRole>(){new UserRole()
                {
                    Role = brokerRole
                }},
                Status = 0
            };

            PasswordHelper.SetPasswordHashed(newUser, brokerModel.Password);

            #endregion

            #region Broker用户创建 杨定鹏 2015年5月28日14:53:32

            var model = new BrokerEntity();
            model.UserId = _userService.InsertUser(newUser).Id;
            model.Brokername = brokerModel.UserName;
            model.Phone = brokerModel.Phone;
            model.Totalpoints = 0;
            model.Amount = 0;
            model.Usertype = EnumUserType.普通用户;
            model.Regtime = DateTime.Now;
            model.State = 1;
            model.Adduser = 0;
            model.Addtime = DateTime.Now;
            model.Upuser = 0;
            model.Uptime = DateTime.Now;

            //判断初始等级是否存在,否则创建
            var level = _levelService.GetLevelsByCondition(new LevelSearchCondition { Name = "默认等级" }).FirstOrDefault();
            if (level == null)
            {
                var levelModel = new LevelEntity
                {
                    Name = "默认等级",
                    Describe = "系统默认初始创建",
                    Url = "",
                    Uptime = DateTime.Now,
                    Addtime = DateTime.Now,
                };
                _levelService.Create(levelModel);
            }

            model.Level = level;

            var newBroker = _brokerService.Create(model);



            #endregion

            #region 推荐经纪人
            if (!string.IsNullOrEmpty(brokerModel.inviteCode))
            {
                //添加经纪人
                var entity = new RecommendAgentEntity
                {
                    PresenteebId = newBroker.Id,
                    Qq = newBroker.Qq.ToString(),
                    Agentlevel = newBroker.Agentlevel,
                    Brokername = newBroker.Brokername,
                    Phone = newBroker.Phone,
                    Regtime = DateTime.Now,
                    Broker = _brokerService.GetBrokerById(Convert.ToInt32(messageDetail.InvitationId)),
                    Uptime = DateTime.Now,
                    Addtime = DateTime.Now,
                };

                _recommendagentService.Create(entity);
            }
            #endregion

            return PageHelper.toJson(PageHelper.ReturnValue(true, "注册成功"));
        }


        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns>当前用户列表</returns>
        [Description("获取当前用户")]
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
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页面数量</param>
        /// <returns>用户列表</returns>
        [Description("获取用户列表")]
        [HttpGet]
        public HttpResponseMessage GetUserList(string userName = null, int page = 1, int pageSize = 10)
        {
            var userCondition = new UserSearchCondition
            {
                UserName = userName,
                Page = page,
                PageSize = pageSize
            };

            var userList = _userService.GetUserByCondition(userCondition).Select(a => new UserModel
            {
                Id = a.Id,
                UserName = a.UserName,
                Status = a.Status
            }).ToList();
            var userCount = _userService.GetUserCountByCondition(userCondition);
            return PageHelper.toJson(new { List = userList, Condition = userCondition, TotalCount = userCount });
        }
        /// <summary>
        /// 查询用户详细信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>用户详细信息</returns>
        [Description("查询用户详细信息")]
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
                Status = user.Status
            };
            return PageHelper.toJson(userDetail);
        }

        /// <summary>
        /// 忘记密码找回密码
        /// </summary>
        /// <returns>返回密码找回结果状态信息</returns>
        [Description("忘记密码找回密码")]
        [HttpPost]
        public HttpResponseMessage ForgetPassword([FromBody]ForgetPasswordModel model)
        {
            //判断用户是否存在
            var sech = new BrokerSearchCondition
            {
                OrderBy = EnumBrokerSearchOrderBy.OrderById,
                Phone = model.Phone
            };
            var broker = _brokerService.GetBrokersByCondition(sech).FirstOrDefault();
            if (broker == null) return PageHelper.toJson(PageHelper.ReturnValue(false, "该用户不存在！"));

            #region 首先判断发送到手机的验证码是否正确
            var strDes = EncrypHelper.Decrypt(model.Hidm, "Hos2xNLrgfaYFY2MKuFf3g==");//解密
            string[] str = strDes.Split('$');
            string source = str[0];//获取验证码
            DateTime date = Convert.ToDateTime(str[1]);//获取发送验证码的时间
            DateTime dateNow = Convert.ToDateTime(DateTime.Now.ToLongTimeString());//获取当前时间
            TimeSpan ts = dateNow.Subtract(date);
            double secMinu = ts.TotalMinutes;//得到发送时间与现在时间的时间间隔分钟数
            if (secMinu > 3) //发送时间与接受时间是否大于3分钟
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "你已超过时间验证，请重新发送验证码！"));
            }
            else
            {
                if (model.Yzm != source)//判断验证码是否一致
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "验证码错误，请重新发送！"));
                }
            }


            #endregion

            //判断两次新密码是否一致
            if (model.first_password != model.second_password) return PageHelper.toJson(PageHelper.ReturnValue(true, "密码不一致！"));

            //密码修改
            var user = _userService.FindUser(broker.UserId);
            PasswordHelper.SetPasswordHashed(user, model.first_password);
            _userService.ModifyUser(user);
            return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功！"));
        }

        [HttpPost]
        public HttpResponseMessage ChangePassword([FromBody]ChangePasswordModel model)
        {

            #region 首先判断发送到手机的验证码是否正确
            var strDes = EncrypHelper.Decrypt(model.Hidm, "Hos2xNLrgfaYFY2MKuFf3g==");//解密
            string[] str = strDes.Split('$');
            string source = str[0];//获取验证码
            DateTime date = Convert.ToDateTime(str[1]);//获取发送验证码的时间
            DateTime dateNow = Convert.ToDateTime(DateTime.Now.ToLongTimeString());//获取当前时间
            TimeSpan ts = dateNow.Subtract(date);
            double secMinu = ts.TotalMinutes;//得到发送时间与现在时间的时间间隔分钟数
            if (secMinu > 3) //发送时间与接受时间是否大于3分钟
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "你已超过时间验证，请重新发送验证码！"));
            }
            else
            {
                if (model.MobileYzm != source)//判断验证码是否一致
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "验证码错误，请重新发送！"));
                }
            }


            #endregion



            //判断两次新密码是否一致
            if (model.Password != model.SecondPassword)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "密码不一致！"));
            }
            //判读旧密码
            var user = (UserBase)_workContext.CurrentUser;
            if (user != null && PasswordHelper.ValidatePasswordHashed(user, model.OldPassword))
            {
                PasswordHelper.SetPasswordHashed(user, model.Password);
                _userService.ModifyUser(user);
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功！"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败！"));
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns>删除结果状态信息</returns>
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
        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="model">用户参数</param>
        /// <returns>编辑结果状态信息</returns>
        [HttpPost]
        public HttpResponseMessage EditUser(UserModel model)
        {
            var user = _userService.FindUser(model.Id);
            var role = _roleService.GetRoleById(model.RoleId);
            user.Password = model.Password;
            user.Status = model.Status;

            if (user.UserRoles == null)
                user.UserRoles = new List<UserRole> { new UserRole() { Role = role, User = user } };
            else
            {
                user.UserRoles.First().Role = role;
            }
            PasswordHelper.SetPasswordHashed(user, model.Password);
            if (_userService.ModifyUser(user))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(true, "数据更新成功！"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "数据更新失败！"));
        }
    }
}
