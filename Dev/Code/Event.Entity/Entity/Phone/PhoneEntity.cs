using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace Event.Entity.Model
{
	public class PhoneEntity : IBaseEntity
	{
		/// <summary>
		/// ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// Follower_Id
		/// </summary>
		public virtual FollowerEntity Follower { get; set; }
		/// <summary>
		/// OpenId
		/// </summary>
		public virtual string Openid { get; set; }
		/// <summary>
		/// Phone
		/// </summary>
		public virtual string Phone { get; set; }
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