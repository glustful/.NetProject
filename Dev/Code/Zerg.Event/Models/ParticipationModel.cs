using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Event.Entity.Model;
using YooPoon.WebFramework.User.Entity;


namespace Event.Models
{
	public class ParticipationModel
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
        /// UserName
        /// </summary>
		public string Username {get;set;}


		/// <summary>
        /// UserId
        /// </summary>
		public int Userid {get;set;}


		/// <summary>
        /// Phone
        /// </summary>
		public string Phone {get;set;}


		/// <summary>
        /// AddUser
        /// </summary>
		public int Adduser {get;set;}


		/// <summary>
        /// AddTime
        /// </summary>
		public DateTime Addtime {get;set;}



	}
}