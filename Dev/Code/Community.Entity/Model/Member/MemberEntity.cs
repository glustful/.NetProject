using System;
using System.Collections.Generic;
using Community.Entity.Model.MemberAddress;
using YooPoon.Core.Data;

namespace Community.Entity.Model.Member
{
	public class MemberEntity : IBaseEntity
	{
		/// <summary>
		/// ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 真实姓名
		/// </summary>
		public virtual string  RealName { get; set; }
		/// <summary>
		/// 身份证号
		/// </summary>
		public virtual string  IdentityNo { get; set; }
		/// <summary>
		/// 性别
		/// </summary>
		public virtual EnumGender Gender { get; set; }
		/// <summary>
		/// 电话
		/// </summary>
		public virtual string  Phone { get; set; }
		/// <summary>
		/// QQ
		/// </summary>
		public virtual string  Icq { get; set; }
		/// <summary>
		/// 邮编
		/// </summary>
		public virtual string  PostNo { get; set; }
		/// <summary>
		/// 头像
		/// </summary>
		public virtual string  Thumbnail { get; set; }
		/// <summary>
		/// 账户余额
		/// </summary>
		public virtual decimal AccountNumber { get; set; }
		/// <summary>
		/// 积分
		/// </summary>
		public virtual decimal Points { get; set; }
		/// <summary>
		/// 等级
		/// </summary>
		public virtual int Level { get; set; }
		/// <summary>
		/// 注册时间
		/// </summary>
		public virtual DateTime AddTime { get; set; }
		/// <summary>
		/// 修改人
		/// </summary>
		public virtual int UpdUser { get; set; }
		/// <summary>
		/// 修改时间
		/// </summary>
		public virtual DateTime UpdTime { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public virtual int UserId { get; set; }
        /// <summary>
        /// 收获地址
        /// </summary>
        public virtual List<MemberAddressEntity> Address { get; set; } 
	}
}