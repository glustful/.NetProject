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
                return  PageHelper.toJson(PageHelper.ReturnValue(false, "ID ����Ϊ��")); 
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
                return PageHelper.toJson(PageHelper.ReturnValue(false, "����������")); 
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
                return PageHelper.toJson(PageHelper.ReturnValue(false, "����������")); 
            }
            var totalCount = _memberService.GetMemberCount(condition);
            return PageHelper.toJson(new { List = model,TotalCount = totalCount});
		}
         //����userId ��ȡ��Ա��Ϣ
        public HttpResponseMessage Get(string userId) 
        {
            if (string.IsNullOrEmpty(userId) || !PageHelper.ValidateNumber(userId)) 
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "������֤����"));
            }
            var model = _memberService.GetMemberByUserId(Convert.ToInt32(userId));
            if (model == null) return PageHelper.toJson(PageHelper.ReturnValue(false, "����������"));
            return PageHelper.toJson(model);

        }

        public HttpResponseMessage Post(MemberModel model)
		{
            if (model != null) 
            {
                //if (!PageHelper.IsMobilePhone(model.Phone)) { return PageHelper.toJson(PageHelper.ReturnValue(false, "�ֻ������绰���벻�Ϸ�")); }
               
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
                    return PageHelper.toJson(PageHelper.ReturnValue(true, "post �ɹ�"));
                }
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "post  ʧ��")); 
		}
        // /// <summary>
        // /// �����û�ID��ȡ��Ա��Ϣ
        // /// </summary>
        // /// <param name="userId"></param>
        // /// <returns></returns>
        // [HttpGet]
        //public HttpResponseMessage GetMemberByUserId(int userId) 
        //{
        //    if (userId == 0) 
        //    {
        //        return PageHelper.toJson(PageHelper.ReturnValue(false, "��ȡ��Ա��Ϣʧ��"));
        //    }
        //    var member = _memberService.GetMemberByUserId(userId);
        //    if (member == null) 
        //    {
        //        return PageHelper.toJson(PageHelper.ReturnValue(false, "�㻹���ǻ�Ա"));
        //    }
        //    return PageHelper.toJson(member);
        //}
        /// <summary>
        /// ���û�ע��
        /// </summary>
        //[HttpPost]
        //public HttpResponseMessage SignUp(MemberModel model)
        //{
        //    var user = _userService.GetUserByName(model.UserName);
        //    if (user != null)
        //    {
        //        return PageHelper.toJson(PageHelper.ReturnValue(false, "�û����Ѿ�����"));
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
        //        return PageHelper.toJson(PageHelper.ReturnValue(false, "ע���û�ʧ�ܣ�������"));
        //    }
        //    return PageHelper.toJson(PageHelper.ReturnValue(true, "ע��ɹ�"));
          
        //}
        /// <summary>
        /// ǰ��������û�
        /// </summary>
        /// <param name="memberModel">�����˲���</param>
        /// <returns>��Ӿ����˽��</returns>
        [HttpPost]
        public HttpResponseMessage AddMember([FromBody]MemberModel memberModel)
        {
            var validMsg = "";
            if (!memberModel.ValidateModel(out validMsg))
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "������֤��������������"));
            }

            if (memberModel.Password != memberModel.SecondPassword)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "�����������벻һ��"));
            }

            var user = _userService.GetUserByName(memberModel.UserName);
            if (user != null)
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "�û����Ѿ�����"));
            }
            var condition = new MemberSearchCondition
            {
                OrderBy = EnumMemberSearchOrderBy.OrderById,
                Phone = memberModel.Phone
            };

            //�ж�user���member�����Ƿ�����û���
            int user2 = _memberService.GetMemberCount(condition);
            if (user2 != 0) return PageHelper.toJson(PageHelper.ReturnValue(false, "�ֻ����Ѿ�����"));

            var memRole = _roleService.GetRoleByName("user");

            //UserȨ��ȱ��ʱ�Զ����
            if (memRole == null)
            {
                memRole = new Role
                {
                    RoleName = "user",
                    RolePermissions = null,
                    Status = RoleStatus.Normal,
                    Description = "��ע����û�Ĭ�Ϲ�Ϊ��ͨ�û�user"
                };
            }

            var newUser = new UserBase
            {
                UserName = memberModel.UserName,
                Password = memberModel.Password,
                RegTime = DateTime.Now,
                NormalizedName = memberModel.UserName.ToLower(),
                //ע���û����Ȩ��
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
            return PageHelper.toJson(PageHelper.ReturnValue(true, "ע��ɹ�"));
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
                        return PageHelper.toJson(PageHelper.ReturnValue(true, "�޸ĳɹ�"));
                    }
                    else
                    {
                        return PageHelper.toJson(PageHelper.ReturnValue(false, "�޸�ʧ��"));
                    }
                }
                return PageHelper.toJson(PageHelper.ReturnValue(false, "�޸�ʧ��"));
            }
            return PageHelper.toJson(PageHelper.ReturnValue(false, "�޸�ʧ��"));
		}

		public HttpResponseMessage Delete(int id)
		{
            if (id == 0) 
            {
                return PageHelper.toJson(PageHelper.ReturnValue(false, "ɾ��ʧ��"));
            }
			var entity = _memberService.GetMemberById(id);
			if(entity == null)
                return PageHelper.toJson(PageHelper.ReturnValue(false, "ɾ��ʧ��"));
			if(_memberService.Delete(entity))
                return PageHelper.toJson(PageHelper.ReturnValue(true, "ɾ���ɹ�"));
            return PageHelper.toJson(PageHelper.ReturnValue(false, "ɾ��ʧ��"));
		}
	}
}