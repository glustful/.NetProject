using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class MerchantOrderEntity : IBaseEntity
	{
		/// <summary>
		/// 订单关联表ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 商家ID
		/// </summary>
		public virtual MerchantInfoEntity MerchantInfo { get; set; }
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