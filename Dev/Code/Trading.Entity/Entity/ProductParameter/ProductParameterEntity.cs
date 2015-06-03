using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace Trading.Entity.Model
{
	public class ProductParameterEntity : IBaseEntity
	{
		/// <summary>
		/// 商品参数ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 参数值ID
		/// </summary>
		public virtual ParameterValueEntity ParameterValue { get; set; }
		/// <summary>
		/// 参数ID
		/// </summary>
		public virtual ParameterEntity Parameter { get; set; }
		/// <summary>
		/// 商品ID
		/// </summary>
		public virtual ProductEntity Product { get; set; }
		/// <summary>
		/// 商品参数排序
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
	}
}