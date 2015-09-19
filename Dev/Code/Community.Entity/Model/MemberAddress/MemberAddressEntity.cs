using System;
using Community.Entity.Model.Member;
using YooPoon.Core.Data;
using Community.Entity.Model.Area;

namespace Community.Entity.Model.MemberAddress
{
	public class MemberAddressEntity : IBaseEntity
	{
		/// <summary>
		/// 行政区划ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 地址ID
		/// </summary>
		public virtual MemberEntity Member { get; set; }


        /// <summary>
        /// 行政区域
        /// </summary>
        public virtual AreaEntity Area { get; set; }
		/// <summary>
		/// 地址
		/// </summary>
		public virtual string Address { get; set; }
		/// <summary>
		/// 邮编
		/// </summary>
		public virtual string Zip { get; set; }
		/// <summary>
		/// 联系人
		/// </summary>
		public virtual string Linkman { get; set; }
		/// <summary>
		/// 联系电话
		/// </summary>
		public virtual string Tel { get; set; }
		/// <summary>
		/// AddUser
		/// </summary>
		public virtual int Adduser { get; set; }
		/// <summary>
		/// AddTime
		/// </summary>
		public virtual DateTime Addtime { get; set; }
		/// <summary>
		/// UpdUser
		/// </summary>
		public virtual int Upduser { get; set; }
		/// <summary>
		/// UpdTime
		/// </summary>
		public virtual DateTime Updtime { get; set; }
	}
}