using System;
using YooPoon.Core.Data;

namespace Community.Entity.Model.Category
{
	public class CategoryEntity : IBaseEntity
	{
		/// <summary>
		/// Id
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 父分类
		/// </summary>
		public virtual CategoryEntity Father { get; set; }
        /// <summary>
        /// 父分类ID 
        /// </summary>
      //  public virtual int FatherId { get; set; }
		/// <summary>
		/// 分类名称
		/// </summary>
		public virtual string Name { get; set; }
		/// <summary>
		/// 分类排序
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