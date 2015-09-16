using System;

namespace Zerg.Models.Community
{
	public class ProductParameterModel
	{

		/// <summary>
        /// Id
        /// </summary>
		public int Id {get;set;}


		/// <summary>
        /// 参数值Id
        /// </summary>
//		public ParameterValueEntity ParameterValue {get;set;}


		/// <summary>
        /// 参数Id
        /// </summary>
//		public ParameterEntity Parameter {get;set;}


		/// <summary>
        /// 商品Id
        /// </summary>
//		public ProductEntity Product {get;set;}


		/// <summary>
        /// 商品参数排序
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

        public int ProductId { get; set; }
        public int ParameterId { get; set; }
        public int ParameterValueId { get; set; }

        public int[] ValueIds { get; set; }

	}
}