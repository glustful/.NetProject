using System;
using Community.Entity.Model.Parameter;
using YooPoon.Core.Data;

namespace Community.Entity.Model.ParameterValue
{
	public class ParameterValueEntity : IBaseEntity
	{
		/// <summary>
		/// 参数值Id
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 参数Id
		/// </summary>
		public virtual ParameterEntity Parameter { get; set; }
		/// <summary>
		/// 参数值
		/// </summary>
		public virtual string Value { get; set; }
		/// <summary>
		/// 参数值排序
		/// </summary>
		public virtual int Sort { get; set; }
		/// <summary>
		/// AddUser
		/// </summary>
		public virtual int AddUser { get; set; }
		/// <summary>
		/// AddTime
		/// </summary>
		public virtual DateTime AddTime { get; set; }
		/// <summary>
		/// UpdUser
		/// </summary>
		public virtual int UpdUser { get; set; }
		/// <summary>
		/// UpdTime
		/// </summary>
		public virtual DateTime UpdTime { get; set; }
	}
}