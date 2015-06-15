using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace Event.Entity.Model
{
	public class RedConfigEntity : IBaseEntity
	{
		/// <summary>
		/// ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 商家关联Id
		/// </summary>
		public virtual int 商家关联id { get; set; }
		/// <summary>
		/// 活动标题
		/// </summary>
		public virtual string Ttitle { get; set; }
		/// <summary>
		/// 活动简介
		/// </summary>
		public virtual string Intro { get; set; }
		/// <summary>
		/// 开始时间
		/// </summary>
		public virtual DateTime Starttime { get; set; }
		/// <summary>
		/// 结束时间
		/// </summary>
		public virtual DateTime Endtime { get; set; }
		/// <summary>
		/// 当前状态
		/// </summary>
		public virtual int Status { get; set; }
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