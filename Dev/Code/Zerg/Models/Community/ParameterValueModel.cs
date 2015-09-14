using Community.Entity.Model.Parameter;
using System;


namespace Zerg.Models.Community
{
	public class ParameterValueModel
	{

		/// <summary>
        /// 参数值Id
        /// </summary>
		public int Id {get;set;}


		/// <summary>
        /// 参数Id
        /// </summary>
		public ParameterEntity Parameter {get;set;}


		/// <summary>
        /// 参数值
        /// </summary>
		public string ParameterValue {get;set;}


		/// <summary>
        /// 参数值排序
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