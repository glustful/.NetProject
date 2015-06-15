using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Event.Entity.Model;
using YooPoon.WebFramework.User.Entity;


namespace Event.Models
{
	public class PartImageModel
	{

		/// <summary>
        /// ID
        /// </summary>
		public int Id {get;set;}


		/// <summary>
        /// 项目ID
        /// </summary>
		public CrowdEntity Crowd {get;set;}


		/// <summary>
        /// 排序权重
        /// </summary>
		public int Orderby {get;set;}


		/// <summary>
        /// 图片地址
        /// </summary>
		public string Imgurl {get;set;}


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



	}
}