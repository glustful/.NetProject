using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Event.Entity.Model;
using YooPoon.WebFramework.User.Entity;


namespace Event.Models
{
	public class FollowerModel
	{

		/// <summary>
        /// ID
        /// </summary>
		public int Id {get;set;}


		/// <summary>
        /// OpenId
        /// </summary>
		public string Openid {get;set;}


		/// <summary>
        /// 昵称
        /// </summary>
		public string Nickname {get;set;}


		/// <summary>
        /// 性别
        /// </summary>
		public string Sex {get;set;}


		/// <summary>
        /// 城市
        /// </summary>
		public string City {get;set;}


		/// <summary>
        /// 国家
        /// </summary>
		public string Country {get;set;}


		/// <summary>
        /// 省份
        /// </summary>
		public string Private {get;set;}


		/// <summary>
        /// 用户语言
        /// </summary>
		public string Language {get;set;}


		/// <summary>
        /// 用户头像
        /// </summary>
		public string Headimgurl {get;set;}


		/// <summary>
        /// 用户关注时间
        /// </summary>
		public string Subscribe_time {get;set;}


		/// <summary>
        /// Unioid机制
        /// </summary>
		public string Unioid {get;set;}


		/// <summary>
        /// 备注信息
        /// </summary>
		public string Remark {get;set;}


		/// <summary>
        /// 用户分组Id
        /// </summary>
		public string Groupid {get;set;}


		/// <summary>
        /// AddUser
        /// </summary>
		public int Adduser {get;set;}


		/// <summary>
        /// AddTime
        /// </summary>
		public DateTime Addtime {get;set;}


		/// <summary>
        /// UpUser
        /// </summary>
		public int Upuser {get;set;}


		/// <summary>
        /// UpTime
        /// </summary>
		public DateTime Uptime {get;set;}
        /// <summary>
        /// 参加抽奖用户
        /// </summary>
        //public int FollowerId { get; set; }
        /// <summary>
        /// 参加抽奖用户手机
        /// </summary>
        public string Phone { get; set; }



	}
}