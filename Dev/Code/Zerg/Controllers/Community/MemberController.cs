using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.Member;
using Community.Service.Member;
using Zerg.Models.Community;
using System.Net.Http;
using Zerg.Common;
using System.Web.Http.Cors;
using YooPoon.WebFramework.User;
using YooPoon.WebFramework.User.Entity;
using YooPoon.WebFramework.User.Services;
using YooPoon.WebFramework.Authentication.Entity;
using YooPoon.Core.Site;
using CRM.Service.Level;
using CRM.Entity.Model;


namespace Zerg.Controllers.Community
{
     [AllowAnonymous]
     [EnableCors("*", "*", "*", SupportsCredentials = true)]
	public class MemberController : ApiController
	{
		private readonly IMemberService _memberService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ILevelService _levelService;
        private readonly IWorkContext _workContext;
        public MemberController(IMemberService memberService, IUserService userService, IRoleService roleService, ILevelService levelService)
		{
			_memberService = memberService;
            _userService = userService;
            _roleService = roleService;
            _levelService = levelService;
        }

        public HttpResponseMessage Get(int id)
		{
            if (id == 0) 
            {
                return  PageHelper.toJson(PageHelper.ReturnValue(false, "ID 不能为空")); 
            }
			var entity =_memberService.GetMemberById(id);
            if (entity != null)
            {
                var model = new MemberModel
                {
                    Id = entity.Id,
                    RealName = entity.RealName,
                    IdentityNo = entity.IdentityNo,
                    Gender = entity.Gender,
                    Phone = entity.Phone,
                    Icq = entity.Icq,
                    PostNo = entity.PostNo,
                    Thumbnail = entity.Thumbnail,
                    AccountNumber = entity.AccountNumber,
                    Points = entity.Points,
                    Level = entity.Level,
                    AddTime = entity.AddTime,
                    UpdUser = entity.UpdUser,
                    UpdTime = entity.UpdTime,
                };
                return PageHelper.toJson(model);
            }
            else 
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "不存在数据")); 
            }
			
		}

        public HttpResponseMessage Get([FromUri]MemberSearchCondition condition)
		{
			var model = _memberService.GetMembersByCondition(condition).Select(c=>new MemberModel
			{
				Id = c.Id,
				RealName = c.RealName,
				IdentityNo = c.IdentityNo,
				Gender = c.Gender,
				Phone = c.Phone,
				Icq = c.Icq,
				PostNo = c.PostNo,
				Thumbnail = c.Thumbnail,
				AccountNumber = c.AccountNumber,
				Points = c.Points,
				Level = c.Level,
				AddTime = c.AddTime,                                                              
				UpdUser = c.UpdUser,
				UpdTime = c.UpdTime,
			}).ToList();
            if (model == null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "不存在数据")); 
            }
            var totalCount = _memberService.GetMemberCount(condition);
            return PageHelper.toJson(new { List = model,TotalCount = totalCount});
		}
         //根据userId 获取会员信息
        public HttpResponseMessage Get(string userId) 
        {
            if (string.IsNullOrEmpty(userId) || !PageHelper.ValidateNumber(userId)) 
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误！"));
            }
            var model = _memberService.GetMemberByUserId(Convert.ToInt32(userId));
            if (model == null) return PageHelper.toJson(PageHelper.ReturnValue(false, "不存在数据"));
            return PageHelper.toJson(model);

        }

        public HttpResponseMessage Post(MemberModel model)
		{
            if (model != null) 
            {
                //if (!PageHelper.IsMobilePhone(model.Phone)) { return PageHelper.toJson(PageHelper.ReturnValue(false, "手机号码或电话号码不合法")); }
               
                var entity = new MemberEntity
                {
                    RealName = model.RealName,
                    IdentityNo = model.IdentityNo,
                    Gender = model.Gender,
                    Phone = model.Phone,
                    Icq = model.Icq,
                    PostNo = model.PostNo,
                    Thumbnail = model.Thumbnail,
                    AccountNumber = model.AccountNumber,
                    Points = model.Points,
                    Level = model.Level,
                    AddTime = model.AddTime,
                    UpdUser = model.UpdUser,
                    UpdTime = model.UpdTime,

                };
                if (_memberService.Create(entity).Id > 0)
                {
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "post 成功"));
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "post  失败")); 
		}
        // /// <summary>
        // /// 根据用户ID获取会员信息
        // /// </summary>
        // /// <param name="userId"></param>
        // /// <returns></returns>
        // [HttpGet]
        //public HttpResponseMessage GetMemberByUserId(int userId) 
        //{
        //    if (userId == 0) 
        //    {
        //        return PageHelper.toJson(PageHelper.ReturnValue(false, "获取会员信息失败"));
        //    }
        //    var member = _memberService.GetMemberByUserId(userId);
        //    if (member == null) 
        //    {
        //        return PageHelper.toJson(PageHelper.ReturnValue(false, "你还不是会员"));
        //    }
        //    return PageHelper.toJson(member);
        //}
        /// <summary>
        /// 新用户注册
        /// </summary>
        //[HttpPost]
        //public HttpResponseMessage SignUp(MemberModel model)
        //{
        //    var user = _userService.GetUserByName(model.UserName);
        //    if (user != null)
        //    {
        //        return PageHelper.toJson(PageHelper.ReturnValue(false, "用户名已经存在"));
        //    }
        //    var newUser = new UserBase
        //    {
        //        UserName = model.UserName,
        //        Password = model.Password,
        //        RegTime = DateTime.Now,
        //        NormalizedName = model.UserName.ToLower(),
        //        Status = 0
        //    };
        //    PasswordHelper.SetPasswordHashed(newUser, model.Password);
        //    if (_userService.InsertUser(newUser).Id <= 0)
        //    {
        //        return PageHelper.toJson(PageHelper.ReturnValue(false, "注册用户失败，请重试"));
        //    }
        //    return PageHelper.toJson(PageHelper.ReturnValue(true, "注册成功"));
          
        //}
        /// <summary>
        /// 前端添加新用户
        /// </summary>
        /// <param name="memberModel">经纪人参数</param>
        /// <returns>添加经纪人结果</returns>
        [HttpPost]
        public HttpResponseMessage AddMember([FromBody]MemberModel memberModel)
        {
            var validMsg = "";
            if (!memberModel.ValidateModel(out validMsg))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "数据验证错误，请重新输入"));
            }

            if (memberModel.Password != memberModel.SecondPassword)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "两次密码输入不一致"));
            }

            var user = _userService.GetUserByName(memberModel.UserName);
            if (user != null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "用户名已经存在"));
            }
            var condition = new MemberSearchCondition
            {
                OrderBy = EnumMemberSearchOrderBy.OrderById,
                Phone = memberModel.Phone
            };

            //判断user表和member表中是否存在用户名
            int user2 = _memberService.GetMemberCount(condition);
            if (user2 != 0) return PageHelper.toJson(PageHelper.ReturnValue(false, "手机号已经存在"));

            var memRole = _roleService.GetRoleByName("user");

            //User权限缺少时自动添加
            if (memRole == null)
            {
                memRole = new Role
                {
                    RoleName = "user",
                    RolePermissions = null,
                    Status = RoleStatus.Normal,
                    Description = "刚注册的用户默认归为普通用户user"
                };
            }

            var newUser = new UserBase
            {
                UserName = memberModel.UserName,
                Password = memberModel.Password,
                RegTime = DateTime.Now,
                NormalizedName = memberModel.UserName.ToLower(),
                //注册用户添加权限
                UserRoles = new List<UserRole>(){new UserRole()
                {
                    Role = memRole
                }},
                Status = 0
            };
            
            PasswordHelper.SetPasswordHashed(newUser, memberModel.Password);

            var model = new MemberEntity();
            model.UserId = _userService.InsertUser(newUser).Id;
            model.RealName = memberModel.UserName;
            model.UserName = memberModel.UserName;
            model.Phone = memberModel.Phone;
            model.Points=0;
            model.IdentityNo="";
            model.Icq="";
            model.PostNo="";
            model.AccountNumber=0;
            model.AddTime=DateTime.Now;
            model.Gender=EnumGender.Male;
            model.UpdTime =DateTime.Now;
            model.UpdUser=0;
            var newMember = _memberService.Create(model);
            return PageHelper.toJson(PageHelper.ReturnValue(true, "注册成功"));
        }
		public HttpResponseMessage Put(MemberModel model)
		{
            if (model != null)
            {
                var entity = _memberService.GetMemberById(model.Id);

                if (entity != null)
                {
                    entity.RealName = model.RealName;
                    entity.IdentityNo = model.IdentityNo;
                    entity.Gender = model.Gender;
                    entity.Phone = model.Phone;
                    entity.Icq = model.Icq;
                    entity.PostNo = model.PostNo;
                    entity.Thumbnail = model.Thumbnail;
                    entity.AccountNumber = model.AccountNumber;
                    entity.Points = model.Points;
                    entity.Level = model.Level;
                    entity.AddTime = DateTime.Now;
                    entity.UpdUser = model.UpdUser;
                    entity.UpdTime = DateTime.Now;
                    if (_memberService.Update(entity) != null)
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "修改成功"));
                    }
                    else
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "修改失败"));
                    }
                }
                return PageHelper.toJson(PageHelper.ReturnValue(false, "修改失败"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "修改失败"));
		}

		public HttpResponseMessage Delete(int id)
		{
            if (id == 0) 
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败"));
            }
			var entity = _memberService.GetMemberById(id);
			if(entity == null)
                return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败"));
			if(_memberService.Delete(entity))
                return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));
            return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败"));
		}
	}
}