using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.MemberAddress;
using Community.Service.MemberAddress;
using Zerg.Models.Community;

namespace Zerg.Controllers.Community
{
	public class MemberAddressController : ApiController
	{
		private readonly IMemberAddressService _memberAddressService;

		public MemberAddressController(IMemberAddressService memberAddressService)
		{
			_memberAddressService = memberAddressService;
		}

		public MemberAddressModel Get(int id)
		{
			var entity =_memberAddressService.GetMemberAddressById(id);
			var model = new MemberAddressModel
			{
				Id = entity.Id,
//                Member = entity.Member,		
                Address = entity.Address,	
                Zip = entity.Zip,	
                Linkman = entity.Linkman,	
                Tel = entity.Tel,		
                Adduser = entity.Adduser,	
                Addtime = entity.Addtime,		
                Upduser = entity.Upduser,		
                Updtime = entity.Updtime
            };
			return model;
		}

		public List<MemberAddressModel> Get(MemberAddressSearchCondition condition)
		{
			var model = _memberAddressService.GetMemberAddresssByCondition(condition).Select(c=>new MemberAddressModel
			{
				Id = c.Id,
//				Member = c.Member,
				Address = c.Address,
				Zip = c.Zip,
				Linkman = c.Linkman,
				Tel = c.Tel,
				Adduser = c.Adduser,
				Addtime = c.Addtime,
				Upduser = c.Upduser,
				Updtime = c.Updtime,
			}).ToList();
			return model;
		}

		public bool Post(MemberAddressModel model)
		{
			var entity = new MemberAddressEntity
			{
//				Member = model.Member,
				Address = model.Address,
				Zip = model.Zip,
				Linkman = model.Linkman,
				Tel = model.Tel,
				Adduser = model.Adduser,
				Addtime = model.Addtime,
				Upduser = model.Upduser,
				Updtime = model.Updtime,
			};
			if(_memberAddressService.Create(entity).Id > 0)
			{
				return true;
			}
			return false;
		}

		public bool Put(MemberAddressModel model)
		{
			var entity = _memberAddressService.GetMemberAddressById(model.Id);
			if(entity == null)
				return false;
//			entity.Member = model.Member;
			entity.Address = model.Address;
			entity.Zip = model.Zip;
			entity.Linkman = model.Linkman;
			entity.Tel = model.Tel;
			entity.Adduser = model.Adduser;
			entity.Addtime = model.Addtime;
			entity.Upduser = model.Upduser;
			entity.Updtime = model.Updtime;
			if(_memberAddressService.Update(entity) != null)
				return true;
			return false;
		}

		public bool Delete(int id)
		{
			var entity = _memberAddressService.GetMemberAddressById(id);
			if(entity == null)
				return false;
			if(_memberAddressService.Delete(entity))
				return true;
			return false;
		}
	}
}