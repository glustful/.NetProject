using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Event.Entity.Model;
using YooPoon.WebFramework.User.Entity;


namespace Event.Models
{
	public class CrowdModel
	{

		/// <summary>
        /// ID
        /// </summary>
		public int Id {get;set;}
        /// <summary>
        /// 图片栏目序号
        /// </summary>
        public int i { get; set; }

		/// <summary>
        /// 项目标题
        /// </summary>
		public string Ttitle {get;set;}


		/// <summary>
        /// 项目简介
        /// </summary>
		public string Intro {get;set;}


		/// <summary>
        /// 开始时间
        /// </summary>
		public DateTime Starttime {get;set;}


		/// <summary>
        /// 结束时间
        /// </summary>
		public DateTime Endtime {get;set;}


		/// <summary>
        /// 项目状态
        /// </summary>
		public int Status {get;set;}


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
        /// 图片集合
        /// </summary>
	    public List<PartImageEntity> ImgList { get; set; }
        /// <summary>
        /// 项目参与人数
        /// </summary>
        public List<DiscountEntity> Dislist { get; set; }
        /// <summary>
        /// 众筹人数
        /// </summary>
        public int crowdNum { get; set; }
        /// <summary>
        /// 众筹设置最多人数
        /// </summary>
        public int crowdMaxNum { get; set; }
        /// <summary>
        /// 众筹url
        /// </summary>
        public string crowdUrl { get; set; }
	}
}