using System;
using Community.Entity.Model.Parameter;
using Community.Entity.Model.ParameterValue;
using Community.Entity.Model.Product;
using YooPoon.Core.Data;

namespace Community.Entity.Model.ProductParameter
{
	public class ProductParameterEntity : IBaseEntity
	{
		/// <summary>
		/// Id
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 参数值Id
		/// </summary>
		public virtual ParameterValueEntity ParameterValue { get; set; }
		/// <summary>
		/// 参数Id
		/// </summary>
		public virtual ParameterEntity Parameter { get; set; }
		/// <summary>
		/// 商品Id
		/// </summary>
		public virtual ProductEntity Product { get; set; }
		/// <summary>
		/// 商品参数排序
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