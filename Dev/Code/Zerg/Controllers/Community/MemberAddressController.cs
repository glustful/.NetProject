using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Community.Entity.Model.MemberAddress;
using Community.Service.MemberAddress;
using Zerg.Models.Community;
using Community.Entity.Model.Member;
using System.Web.Http.Cors;
using System.Net.Http;
using Zerg.Common;

namespace Zerg.Controllers.Community
{
    [AllowAnonymous]
    [EnableCors("*", "*", "*", SupportsCredentials = true)]
	public class MemberAddressController : ApiController
	{
		private readonly IMemberAddressService _memberAddressService;

		public MemberAddressController(IMemberAddressService memberAddressService)
		{
			_memberAddressService = memberAddressService;
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
			var model = _memberAddressService.GetMemberAddresssByCondition(condition).Select(c=>new MemberAddressModel
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
			}).ToList();
            var totalCount = _memberAddressService.GetMemberAddressCount(condition);
            return PageHelper.toJson(new { List = model, TotalCount = totalCount });
		}

        public HttpResponseMessage Post([FromBody]MemberAddressModel model)
		{
			var entity = new MemberAddressEntity
			{
				//Member = model.Member,
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
                return PageHelper.toJson(PageHelper.ReturnValue(true, "post ³É¹¦"));
			}
            return PageHelper.toJson(PageHelper.ReturnValue(false, "post  Ê§°Ü")); 
		}

        public HttpResponseMessage Put([FromBody]MemberAddressModel model)
		{
			var entity = _memberAddressService.GetMemberAddressById(model.Id);
			if(entity == null)
                return PageHelper.toJson(PageHelper.ReturnValue(false, "ÐÞ¸ÄÊ§°Ü"));
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
                return PageHelper.toJson(PageHelper.ReturnValue(true, "ÐÞ¸Ä³É¹¦"));
            return PageHelper.toJson(PageHelper.ReturnValue(false, "ÐÞ¸ÄÊ§°Ü"));
		}

        public HttpResponseMessage Delete(int id)
		{
			var entity = _memberAddressService.GetMemberAddressById(id);
			if(entity == null)
                return PageHelper.toJson(PageHelper.ReturnValue(false, "É¾³ýÊ§°Ü"));
			if(_memberAddressService.Delete(entity))
                return PageHelper.toJson(PageHelper.ReturnValue(true, "É¾³ý³É¹¦"));
            return PageHelper.toJson(PageHelper.ReturnValue(false, "É¾³ýÊ§°Ü"));
		}
	}
}