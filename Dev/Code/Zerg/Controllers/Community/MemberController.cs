using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.Member;
using Community.Service.Member;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
	public class MemberController : ApiController
	{
		private readonly IMemberService _memberService;

		public MemberController(IMemberService memberService)
		{
			_memberService = memberService;
		}

		public MemberModel Get(int id)
		{
			var entity =_memberService.GetMemberById(id);
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
			return model;
		}

		public List<MemberModel> Get(MemberSearchCondition condition)
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
			return model;
		}

		public bool Post(MemberModel model)
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
			if(_memberService.Create(entity).Id > 0)
			{
				return true;
			}
			return false;
		}

		public bool Put(MemberModel model)
		{
			var entity = _memberService.GetMemberById(model.Id);
			if(entity == null)
				return false;
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
			if(_memberService.Update(entity) != null)
				return true;
			return false;
		}

		public bool Delete(int id)
		{
			var entity = _memberService.GetMemberById(id);
			if(entity == null)
				return false;
			if(_memberService.Delete(entity))
				return true;
			return false;
		}
	}
}