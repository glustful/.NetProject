using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace Event.Entity.Model
{
	public class CrowdEntity : IBaseEntity
	{
		/// <summary>
		/// ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 项目标题
		/// </summary>
		public virtual string Ttitle { get; set; }
		/// <summary>
		/// 项目简介
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
		/// 项目状态
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
        /// <summary>
        /// 众筹链接地址
        /// </summary>
        public virtual string crowdUrl { get; set; }
	}
}