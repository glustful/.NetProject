using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace Event.Entity.Model
{
	public class DiscountEntity : IBaseEntity
	{
		/// <summary>
		/// ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// Crowd_Id
		/// </summary>
		public virtual CrowdEntity Crowd { get; set; }
		/// <summary>
		/// 参与人数
		/// </summary>
		public virtual int Number { get; set; }
		/// <summary>
		/// 折扣优惠
		/// </summary>
		public virtual int Discount { get; set; }
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