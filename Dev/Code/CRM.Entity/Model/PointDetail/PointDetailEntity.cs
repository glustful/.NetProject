using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class PointDetailEntity : IBaseEntity
	{
		/// <summary>
		/// 积分ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 经纪人ID
		/// </summary>
		public virtual BrokerEntity Broker { get; set; }
		/// <summary>
		/// 积分动作描述
		/// </summary>
		public virtual string Pointsds { get; set; }
		/// <summary>
		/// 增加积分
		/// </summary>
		public virtual int Addpoints { get; set; }
		/// <summary>
		/// 积分总额
		/// </summary>
		public virtual int Totalpoints { get; set; }
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