using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class MerchantClientEntity : IBaseEntity
	{
		/// <summary>
		/// 商家带客ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 商家ID
		/// </summary>
		public virtual MerchantInfoEntity MerchantInfo { get; set; }
		/// <summary>
		/// 客户信息ID
		/// </summary>
		public virtual ClientInfoEntity ClientInfo { get; set; }
		/// <summary>
		/// 预约时间
		/// </summary>
		public virtual DateTime Appointmenttime { get; set; }
		/// <summary>
		/// 预约状态
		/// </summary>
		public virtual string Appointmentstatus { get; set; }
		/// <summary>
		/// 状态详情
		/// </summary>
		public virtual string Details { get; set; }
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