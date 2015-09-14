using System;
using Community.Entity.Model.Product;

namespace Zerg.Models.Community
{
	public class ProductCommentModel
	{

		/// <summary>
        /// ID
        /// </summary>
		public int Id {get;set;}


		/// <summary>
        /// 商品实体
        /// </summary>
		public ProductEntity Product {get;set;}
        /// <summary>
        /// 商品ID
        /// </summary>
        public int ProductId { get; set; }
        public string ProductName { get; set; }

	    /// <summary>
        /// AddUser
        /// </summary>
		public int AddUser {get;set;}


		/// <summary>
        /// AddTime
        /// </summary>
		public DateTime AddTime {get;set;}


		/// <summary>
        /// 评论内容
        /// </summary>
		public string Content {get;set;}


		/// <summary>
        /// 评论星级
        /// </summary>
		public int Stars {get;set;}



	}
}