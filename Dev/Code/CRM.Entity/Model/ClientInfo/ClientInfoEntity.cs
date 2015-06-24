using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class ClientInfoEntity : IBaseEntity
	{
		/// <summary>
		/// 客户信息ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 客户姓名
		/// </summary>
		public virtual string Clientname { get; set; }
		/// <summary>
		/// 电话
		/// </summary>
		public virtual string Phone { get; set; }
		/// <summary>
		/// 意向户型
		/// </summary>
		public virtual string Housetype { get; set; }
		/// <summary>
		/// 楼盘信息
		/// </summary>
		public virtual string Houses { get; set; }
		/// <summary>
		/// 备注信息
		/// </summary>
		public virtual string Note { get; set; }
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