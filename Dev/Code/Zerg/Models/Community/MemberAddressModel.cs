using System;

namespace Zerg.Models.Community
{
	public class MemberAddressModel
	{

		/// <summary>
        /// ID
        /// </summary>
		public int Id {get;set;}

        /// <summary>
        /// 行政区划ID
        /// </summary>
        public int AreaId { get; set; }

		/// <summary>
        /// 地址ID
        /// </summary>
        public int Member { get; set; }


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
        public string AddtimeString
        {
            get
            {
                return Addtime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }


		/// <summary>
        /// UpdUser
        /// </summary>
		public int Upduser {get;set;}


		/// <summary>
        /// UpdTime
        /// </summary>
		public DateTime Updtime {get;set;}
        public string UpdtimeString
        {
            get { return Updtime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        public int UserId { get; set; }

        public bool? IsDefault { get; set; }
	}
}