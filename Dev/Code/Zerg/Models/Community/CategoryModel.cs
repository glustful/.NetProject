using System;

namespace Zerg.Models.Community
{
	public class CategoryModel
	{

		/// <summary>
        /// Id
        /// </summary>
		public int Id {get;set;}


		/// <summary>
        /// 父分类
        /// </summary>
//		public Category Father {get;set;}


		/// <summary>
        /// 分类名称
        /// </summary>
		public string Name {get;set;}


		/// <summary>
        /// 分类排序
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