using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class BrokerLeadClientEntity : IBaseEntity
	{
		/// <summary>
		/// 带客ID
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
        /// <summary>
        /// 经纪人名
        /// </summary>
	    public virtual string Brokername { get; set; }
        /// <summary>
        /// 经纪人等级
        /// </summary>
	    public virtual string BrokerLevel { get; set; }
        /// <summary>
        /// 项目名
        /// </summary>
	    public virtual string Projectname { get; set; }
        /// <summary>
        /// 项目Id
        /// </summary>
	    public virtual int ProjectId { get; set; }
        /// <summary>
        /// 进度状态
        /// </summary>
        public virtual EnumBLeadType Status { get; set; }
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
        /// <summary>
        /// 客户名
        /// </summary>
	    public virtual string ClientName { get; set; }
        /// <summary>
        /// 客户电话
        /// </summary>
	    public virtual string Phone { get; set; }
        /// <summary>
        /// 带客订单
        /// </summary>
	    public virtual int ComOrder { get; set; }
        /// <summary>
        /// 成交订单
        /// </summary>
	    public virtual int DealOrder { get; set; }
        /// <summary>
        /// 删除标识
        /// </summary>
	    public virtual EnumDelFlag DelFlag { get; set; }
	}
}