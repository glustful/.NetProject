using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace Trading.Entity.Model
{
	public class BrandParameterEntity : IBaseEntity
	{
		/// <summary>
		/// 商品参数ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 品牌ID
		/// </summary>
		public virtual ProductBrandEntity ProductBrand { get; set; }
		/// <summary>
		/// 商品参数排序
		/// </summary>
		public virtual int Sort { get; set; }
		/// <summary>
		/// 参数值名称
		/// </summary>
		public virtual string Parametername { get; set; }
		/// <summary>
		/// 参数值
		/// </summary>
		public virtual string Parametervaule { get; set; }
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