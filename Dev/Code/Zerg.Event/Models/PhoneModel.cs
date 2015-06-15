using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Event.Entity.Model;
using YooPoon.WebFramework.User.Entity;


namespace Event.Models
{
	public class PhoneModel
	{

		/// <summary>
        /// ID
        /// </summary>
		public int Id {get;set;}


		/// <summary>
        /// FD
        /// </summary>
		public FollowerEntity Follower {get;set;}


		/// <summary>
        /// OpenId
        /// </summary>
		public string Openid {get;set;}


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