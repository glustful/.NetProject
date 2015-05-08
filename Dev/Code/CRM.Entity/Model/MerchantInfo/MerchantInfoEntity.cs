using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class MerchantInfoEntity : IBaseEntity
	{
		/// <summary>
		/// 商家ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 用户ID
		/// </summary>
		public virtual int UserId { get; set; }
		/// <summary>
		/// 商家名字
		/// </summary>
		public virtual string Merchantname { get; set; }
		/// <summary>
		/// 邮箱
		/// </summary>
		public virtual string Mail { get; set; }
		/// <summary>
		/// 商家地址
		/// </summary>
		public virtual string Address { get; set; }
		/// <summary>
		/// 联系电话
		/// </summary>
		public virtual string Phone { get; set; }
		/// <summary>
		/// 商家描述
		/// </summary>
		public virtual string Describe { get; set; }
		/// <summary>
		/// 营业执照
		/// </summary>
		public virtual string License { get; set; }
		/// <summary>
		/// 法人代表
		/// </summary>
		public virtual string Legalhuman { get; set; }
		/// <summary>
		/// 法人身份证
		/// </summary>
		public virtual string Legalsfz { get; set; }
		/// <summary>
		/// 组织机构代码
		/// </summary>
		public virtual string Orgnum { get; set; }
		/// <summary>
		/// 税务登记证
		/// </summary>
		public virtual string Taxnum { get; set; }
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