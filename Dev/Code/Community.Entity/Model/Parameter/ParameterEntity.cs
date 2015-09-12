using System;
using System.Collections.Generic;
using Community.Entity.Model.Category;
using Community.Entity.Model.ParameterValue;
using YooPoon.Core.Data;

namespace Community.Entity.Model.Parameter
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
		public virtual CategoryEntity Category { get; set; }
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

        public virtual List<ParameterValueEntity> Values { get; set; }
	}
}