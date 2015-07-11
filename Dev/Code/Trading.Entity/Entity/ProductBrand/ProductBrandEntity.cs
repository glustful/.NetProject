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
        /// AdTitle
        /// </summary>
        public virtual string AdTitle { get; set; }
        public virtual int? ClassId { get; set; }
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
        /// <summary>
        /// 副标题
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 父类
        /// </summary>
        public virtual ProductBrandEntity Father { get; set; }

        public virtual List<BrandParameterEntity> ParameterEntities { get; set; }
	}
}