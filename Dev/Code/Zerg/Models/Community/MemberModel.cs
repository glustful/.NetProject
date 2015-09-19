using System;
using Community.Entity.Model.Member;

namespace Zerg.Models.Community
{
	public class MemberModel
	{

		/// <summary>
        /// ID
        /// </summary>
		public int Id {get;set;}


		/// <summary>
        /// 真实姓名
        /// </summary>
		public string  RealName {get;set;}


		/// <summary>
        /// 身份证号
        /// </summary>
		public string  IdentityNo {get;set;}


		/// <summary>
        /// 性别
        /// </summary>
		public EnumGender Gender {get;set;}

		public string GenderString
		{
			get
			{
				switch(Gender)
				{

					case EnumGender.Male:
						return "男";

					case EnumGender.Female:
						return "女";

					default:
						return "";
				}
			}
		}


		/// <summary>
        /// 电话
        /// </summary>
		public string  Phone {get;set;}


		/// <summary>
        /// QQ
        /// </summary>
		public string  Icq {get;set;}


		/// <summary>
        /// 邮编
        /// </summary>
		public string  PostNo {get;set;}


		/// <summary>
        /// 头像
        /// </summary>
		public string  Thumbnail {get;set;}


		/// <summary>
        /// 账户余额
        /// </summary>
		public decimal AccountNumber {get;set;}


		/// <summary>
        /// 积分
        /// </summary>
		public decimal Points {get;set;}


		/// <summary>
        /// 等级
        /// </summary>
		public int Level {get;set;}


		/// <summary>
        /// 注册时间
        /// </summary>
		public DateTime AddTime {get;set;}
        public string AddTimeString
        {
            get { return AddTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

		/// <summary>
        /// 修改人
        /// </summary>
		public int UpdUser {get;set;}


		/// <summary>
        /// 修改时间
        /// </summary>
		public DateTime UpdTime {get;set;}

<<<<<<< HEAD

=======
        public string UserId { get; set; }
        /// <summary>
        /// UC用户
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// UC密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string SecondPassword { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public virtual DateTime Regtime { get; set; }

        public bool ValidateModel(out string msg)
        {
            msg = "";
            if (string.IsNullOrEmpty(UserName)) return false;
            if (string.IsNullOrEmpty(Password)) return false;
            if (string.IsNullOrEmpty(SecondPassword)) return false;
            if (string.IsNullOrEmpty(Phone)) return false;

            return true;
        }
>>>>>>> d3477e4c9d87a895939c01ec0d32def6e5699bb3

	}
}