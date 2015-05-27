using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace Trading.Entity.Model
{
	public class ProductBrandEntity : IBaseEntity
	{
		/// <summary>
		/// 品牌ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 品牌名称
		/// </summary>
		public virtual string Bname { get; set; }
		/// <summary>
		/// 品牌图片
		/// </summary>
		public virtual string Bimg { get; set; }
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

        public string SubTitle { get; set; }

        public string Content { get; set; }

        public virtual ProductBrandEntity Father { get; set; }
	}
}