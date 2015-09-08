using System;
using System.Collections.Generic;
using Community.Entity.Model.Product;
using Community.Entity.Model.ProductComment;

namespace Zerg.Models.Community
{
	public class ProductModel
	{

		/// <summary>
        /// Id
        /// </summary>
		public int Id {get;set;}


		/// <summary>
        /// 分类排序
        /// </summary>
//		public CategoryEntity Category {get;set;}


		/// <summary>
        /// 商家Id
        /// </summary>
		public int Bussnessid {get;set;}


		/// <summary>
        /// 商家名称
        /// </summary>
		public string Bussnessname {get;set;}


		/// <summary>
        /// 价格
        /// </summary>
		public decimal Price {get;set;}


		/// <summary>
        /// 商品名称
        /// </summary>
		public string Name {get;set;}


		/// <summary>
        /// 商品状态
        /// </summary>
		public bool Status {get;set;}


		/// <summary>
        /// 商品主图
        /// </summary>
		public string MainImg {get;set;}


		/// <summary>
        /// 商家推荐标识
        /// </summary>
		public bool IsRecommend {get;set;}


		/// <summary>
        /// 分类排序
        /// </summary>
		public int Sort {get;set;}


		/// <summary>
        /// 库存计数
        /// </summary>
		public int Stock {get;set;}


		/// <summary>
        /// AddUser
        /// </summary>
		public int Adduser {get;set;}


		/// <summary>
        /// AddTime
        /// </summary>
		public DateTime Addtime {get;set;}


		/// <summary>
        /// UpdUser
        /// </summary>
		public int UpdUser {get;set;}


		/// <summary>
        /// UpdTime
        /// </summary>
		public DateTime UpdTime {get;set;}


		/// <summary>
        /// 副标题
        /// </summary>
		public string Subtitte {get;set;}


		/// <summary>
        /// 联系电话
        /// </summary>
		public string Contactphone {get;set;}


		/// <summary>
        /// 商品类型
        /// </summary>
		public EnumProductType Type {get;set;}

		public string TypeString
		{
			get
			{
				switch(Type)
				{

					case EnumProductType.Product:
						return "实物商品";

					case EnumProductType.Service:
						return "服务商品";

					default:
						return "";
				}
			}
		}


		/// <summary>
        /// 商品明细
        /// </summary>
//		public ProductDetailEntity Detail {get;set;}


		/// <summary>
        /// 商品评论
        /// </summary>
		public List<ProductCommentEntity> Comments {get;set;}


		/// <summary>
        /// 商品属性
        /// </summary>
//		public List<ProductParameterEntity> Parameters {get;set;}



	}
}