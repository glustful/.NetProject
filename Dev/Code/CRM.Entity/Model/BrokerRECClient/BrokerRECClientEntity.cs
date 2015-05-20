using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class BrokerRECClientEntity : IBaseEntity
	{
		/// <summary>
		/// 推荐客户ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 经纪人ID
		/// </summary>
		public virtual BrokerEntity Broker { get; set; }
		/// <summary>
		/// 客户信息ID
		/// </summary>
		public virtual ClientInfoEntity ClientInfo { get; set; }
		/// <summary>
		/// 客户名
		/// </summary>
		public virtual string Clientname { get; set; }
		/// <summary>
		/// 电话
		/// </summary>
		public virtual int Phone { get; set; }
		/// <summary>
		/// QQ
		/// </summary>
		public virtual int Qq { get; set; }
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
		/// <summary>
		/// 经纪人名
		/// </summary>
		public virtual string Brokername { get; set; }
		/// <summary>
		/// 经纪人等级
		/// </summary>
		public virtual string Brokerlevel { get; set; }
		/// <summary>
		/// 项目名
		/// </summary>
		public virtual string Projectname { get; set; }
		/// <summary>
		/// 项目ID
		/// </summary>
		public virtual int Projectid { get; set; }
		/// <summary>
		/// 进度状态
		/// </summary>
		public virtual EnumBRECCType Status { get; set; }
		/// <summary>
		/// 场秘ID
		/// </summary>
		public virtual BrokerEntity SecretaryId { get; set; }
		/// <summary>
		/// 场秘电话
		/// </summary>
		public virtual string SecretaryPhone { get; set; }
		/// <summary>
		/// 带客人员ID
		/// </summary>
		public virtual BrokerEntity WriterId { get; set; }
		/// <summary>
		/// 带客人员电话
		/// </summary>
		public virtual string WriterPhone { get; set; }
	}
}