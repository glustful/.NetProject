using System;
using Community.Entity.Model.Member;
using Community.Entity.Model.Product;
using YooPoon.Core.Data;

namespace Community.Entity.Model.ProductComment
{
	public class ProductCommentEntity : IBaseEntity
	{
		/// <summary>
		/// ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 商品ID
		/// </summary>
		public virtual ProductEntity Product { get; set; }
        public virtual MemberEntity Member { get; set; }
		/// <summary>
		/// AddUser
		/// </summary>
		//public virtual int AddUser { get; set; }
		/// <summary>
		/// AddTime
		/// </summary>
		public virtual DateTime AddTime { get; set; }
		/// <summary>
		/// 评论内容
		/// </summary>
		public virtual string Content { get; set; }
		/// <summary>
		/// 评论星级
		/// </summary>
		public virtual int Stars { get; set; }
	}
}