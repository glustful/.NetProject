using System;

namespace Zerg.Models.Community
{
	public class MemberAddressModel
	{

		/// <summary>
        /// 行政区划ID
        /// </summary>
		public int Id {get;set;}


		/// <summary>
        /// 地址ID
        /// </summary>
//		public Member Member {get;set;}


		/// <summary>
        /// 地址
        /// </summary>
		public string Address {get;set;}


		/// <summary>
        /// 邮编
        /// </summary>
		public string Zip {get;set;}


		/// <summary>
        /// 联系人
        /// </summary>
		public string Linkman {get;set;}


		/// <summary>
        /// 联系电话
        /// </summary>
		public string Tel {get;set;}


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
		public int Upduser {get;set;}


		/// <summary>
        /// UpdTime
        /// </summary>
		public DateTime Updtime {get;set;}



	}
}