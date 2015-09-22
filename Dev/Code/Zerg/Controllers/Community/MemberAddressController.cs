using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Community.Entity.Model.MemberAddress;
using Community.Service.Area;
using Community.Service.Member;
using Community.Service.MemberAddress;
using YooPoon.Core.Site;
using Zerg.Common;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
	public class MemberAddressController : ApiController
	{
		private readonly IMemberAddressService _memberAddressService;
        private readonly IWorkContext _workContext;
        private readonly IMemberService _memberService;
        private readonly IAreaService _areaService;

        public MemberAddressController(IMemberAddressService memberAddressService,IWorkContext workContext,IMemberService memberService,IAreaService areaService)
        {
            _memberAddressService = memberAddressService;
            _workContext = workContext;
            _memberService = memberService;
            _areaService = areaService;
        }

        public HttpResponseMessage Get(string memberId)
        {
            MemberAddressEntity entity;
            if (string.IsNullOrEmpty(memberId))
            {
                var user = _workContext.CurrentUser;
                if (user == null)
                    return PageHelper.toJson(PageHelper.ReturnValue(false, "无法获取当前的用户信息"));
                entity = _memberAddressService.GetDefaultAddress(user.Id);
            }
            else
            {
                entity = _memberAddressService.GetDefaultAddress(memberId);
            }
            if (entity == null)
                return PageHelper.toJson(null);
            var model = new MemberAddressModel
            {
                Id = entity.Id,
                Member = entity.Member.Id,
                Address = entity.Address,
                Zip = entity.Zip,
                Linkman = entity.Linkman,
                Tel = entity.Tel,
                Adduser = entity.Adduser,
                Addtime = entity.Addtime,
                Upduser = entity.Upduser,
                Updtime = entity.Updtime
            };
            return PageHelper.toJson(model);
        }

        public HttpResponseMessage Get(int id)
		{
			var entity =_memberAddressService.GetMemberAddressById(id);
            if (entity == null)
                return null;
            var model = new MemberAddressModel
            {
                Id = entity.Id,
                Member = entity.Member.Id,
                Address = entity.Address,
                Zip = entity.Zip,
                Linkman = entity.Linkman,
                Tel = entity.Tel,
                Adduser = entity.Adduser,
                Addtime = entity.Addtime,
                Upduser = entity.Upduser,
                Updtime = entity.Updtime
            };
            return PageHelper.toJson(model);
		}

        public HttpResponseMessage Get([FromUri]MemberAddressSearchCondition condition)
		{
			var model =_memberAddressService.GetMemberAddresssByCondition(condition).Select(o=>o.Id).Count()>0? _memberAddressService.GetMemberAddresssByCondition(condition).Select(c=>new MemberAddressModel
			{
				Id = c.Id,
				Member = c.Member.Id,
				Address = c.Address,
				Zip = c.Zip,
				Linkman = c.Linkman,
				Tel = c.Tel,
				Adduser = c.Adduser,
				Addtime = c.Addtime,
				Upduser = c.Upduser,
				Updtime = c.Updtime,
			}).ToList():null;
            var totalCount = _memberAddressService.GetMemberAddressCount(condition);
            return PageHelper.toJson(new { List = model, Condition = condition, toTalCount = totalCount });
		}

        public HttpResponseMessage Post([FromBody]MemberAddressModel model)
		{
			var entity = new MemberAddressEntity
			{
				Member = _memberService.GetMemberByUserId(model.UserId),
				Address = model.Address,
				Zip = model.Zip,
				Linkman = model.Linkman,
				Tel = model.Tel,
				Adduser = _workContext.CurrentUser.Id,
				Addtime = DateTime.Now,
				Upduser = _workContext.CurrentUser.Id,
				Updtime = DateTime.Now,
                Area = _areaService.GetAreaById(model.AreaId)
			};
			if(_memberAddressService.Create(entity).Id > 0)
			{
                return PageHelper.toJson(PageHelper.ReturnValue(true, "post 成功"));
			}
            return PageHelper.toJson(PageHelper.ReturnValue(false, "post  失败")); 
		}

        public HttpResponseMessage Put([FromBody]MemberAddressModel model)
		{
			var entity = _memberAddressService.GetMemberAddressById(model.Id);
			if(entity == null)
                return PageHelper.toJson(PageHelper.ReturnValue(false, "修改失败"));
			entity.Member.Id = model.Member;
			entity.Address = model.Address;
			entity.Zip = model.Zip;
			entity.Linkman = model.Linkman;
			entity.Tel = model.Tel;
			entity.Adduser = model.Adduser;
			entity.Addtime = model.Addtime;
			entity.Upduser = model.Upduser;
			entity.Updtime = model.Updtime;
			if(_memberAddressService.Update(entity) != null)
                return PageHelper.toJson(PageHelper.ReturnValue(true, "修改成功"));
            return PageHelper.toJson(PageHelper.ReturnValue(false, "修改失败"));
		}

        public HttpResponseMessage Delete(int id)
		{
			var entity = _memberAddressService.GetMemberAddressById(id);
			if(entity == null)
                return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败"));
			if(_memberAddressService.Delete(entity))
                return PageHelper.toJson(PageHelper.ReturnValue(true, "删除成功"));
            return PageHelper.toJson(PageHelper.ReturnValue(false, "删除失败"));
		}
	}
}