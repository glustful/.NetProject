using System;

namespace Zerg.Models.Community
{
	public class ParameterModel
	{

		/// <summary>
        /// 参数ID
        /// </summary>
		public int Id {get;set;}


		/// <summary>
        /// 平台商品分类ID
        /// </summary>
//		public CategoryEntity Category {get;set;}


		/// <summary>
        /// 参数名称
        /// </summary>
		public string Name {get;set;}


		/// <summary>
        /// 参数排序
        /// </summary>
		public int Sort {get;set;}


		/// <summary>
        /// AddUser
        /// </summary>
		public int AddUser {get;set;}


		/// <summary>
        /// AddTime
        /// </summary>
		public DateTime AddTime {get;set;}


		/// <summary>
        /// UpdUser
        /// </summary>
		public int UpdUser {get;set;}


		/// <summary>
        /// UpdTime
        /// </summary>
		public DateTime UpdTime {get;set;}



	}
}