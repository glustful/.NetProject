using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace Event.Entity.Model
{
	public class FollowerEntity : IBaseEntity
	{
		/// <summary>
		/// ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// OpenId
		/// </summary>
		public virtual string Openid { get; set; }
		/// <summary>
		/// 昵称
		/// </summary>
		public virtual string Nickname { get; set; }
		/// <summary>
		/// 性别
		/// </summary>
		public virtual string Sex { get; set; }
		/// <summary>
		/// 城市
		/// </summary>
		public virtual string City { get; set; }
		/// <summary>
		/// 国家
		/// </summary>
		public virtual string Country { get; set; }
		/// <summary>
		/// 省份
		/// </summary>
		public virtual string Private { get; set; }
		/// <summary>
		/// 用户语言
		/// </summary>
		public virtual string Language { get; set; }
		/// <summary>
		/// 用户头像
		/// </summary>
		public virtual string Headimgurl { get; set; }
		/// <summary>
		/// 用户关注时间
		/// </summary>
		public virtual string Subscribetime { get; set; }
		/// <summary>
		/// Unioid机制
		/// </summary>
		public virtual string Unioid { get; set; }
		/// <summary>
		/// 备注信息
		/// </summary>
		public virtual string Remark { get; set; }
		/// <summary>
		/// 用户分组Id
		/// </summary>
		public virtual string Groupid { get; set; }
		/// <summary>
		/// AddUser
		/// </summary>
		public virtual int Adduser { get; set; }
		/// <summary>
		/// AddTime
		/// </summary>
		public virtual DateTime Addtime { get; set; }
		/// <summary>
		/// UpUser
		/// </summary>
		public virtual int Upuser { get; set; }
		/// <summary>
		/// UpTime
		/// </summary>
		public virtual DateTime Uptime { get; set; }
	}
}