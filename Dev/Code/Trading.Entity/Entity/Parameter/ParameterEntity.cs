using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace Trading.Entity.Model
{
	public class ParameterEntity : IBaseEntity
	{
		/// <summary>
		/// 参数ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 平台商品分类ID
		/// </summary>
		public virtual ClassifyEntity Classify { get; set; }
//
//	    public virtual ParameterValueEntity ParameterValue { get; set; }

	    /// <summary>
		/// 参数名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 参数排序
		/// </summary>
		public virtual int Sort { get; set; }
		/// <summary>
		/// AddUser
		/// </summary>
		public virtual string Adduser { get; set; }
		/// <summary>
		/// AddTime
		/// </summary>
		public virtual DateTime Addtime { get; set; }
		/// <summary>
		/// UpdUser
		/// </summary>
		public virtual string Upduser { get; set; }
		/// <summary>
		/// UpdTime
		/// </summary>
		public virtual DateTime Updtime { get; set; }

        public virtual IList<ParameterValueEntity> Values { get; set; }
	}
}