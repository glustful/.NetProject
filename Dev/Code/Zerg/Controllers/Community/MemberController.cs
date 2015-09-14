using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.Member;
using Community.Service.Member;
using Zerg.Models.Community;
using System.Net.Http;
using Zerg.Common;
using System.Web.Http.Cors;

namespace Zerg.Controllers.Community
{
     [AllowAnonymous]
     [EnableCors("*", "*", "*", SupportsCredentials = true)]
	public class MemberController : ApiController
	{
		private readonly IMemberService _memberService;

		public MemberController(IMemberService memberService)
		{
			_memberService = memberService;
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

        public HttpResponseMessage Post(MemberModel model)
		{
            if (model != null) 
            {
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
                    entity.AddTime = model.AddTime;
                    entity.UpdUser = model.UpdUser;
                    entity.UpdTime = model.UpdTime;
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