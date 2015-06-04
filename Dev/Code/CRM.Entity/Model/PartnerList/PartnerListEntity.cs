using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class PartnerListEntity : IBaseEntity
	{
		/// <summary>
		/// 合伙人ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 经纪人ID
		/// </summary>
		public virtual BrokerEntity Broker { get; set; }
		/// <summary>
		/// 合伙经纪人ID
		/// </summary>
		public virtual int PartnerId { get; set; }
		/// <summary>
		/// 合伙经纪人名
		/// </summary>
		public virtual string Brokername { get; set; }
		/// <summary>
		/// 电话
		/// </summary>
		public virtual string Phone { get; set; }
		/// <summary>
		/// 经纪人等级
		/// </summary>
		public virtual string Agentlevel { get; set; }
		/// <summary>
		/// 注册时间
		/// </summary>
		public virtual DateTime Regtime { get; set; }
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

        public virtual EnumPartnerType Status { get; set; }
	}
}