using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.CRM
{
    public class MerchantInfoModel
    {
        /// <summary>
        /// 商家ID
        /// </summary>
        public  string Id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public  int UserId { get; set; }
        /// <summary>
        /// 商家名字
        /// </summary>
        public  string Merchantname { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public  string Mail { get; set; }
        /// <summary>
        /// 商家地址
        /// </summary>
        public   string Address { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public   string Phone { get; set; }
        /// <summary>
        /// 商家描述
        /// </summary>
        public  string Describe { get; set; }
        /// <summary>
        /// 营业执照
        /// </summary>
        public  string License { get; set; }
        /// <summary>
        /// 法人代表
        /// </summary>
        public   string Legalhuman { get; set; }
        /// <summary>
        /// 法人身份证
        /// </summary>
        public   string Legalsfz { get; set; }
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public   string Orgnum { get; set; }
        /// <summary>
        /// 税务登记证
        /// </summary>
        public  string Taxnum { get; set; }
        /// <summary>
        /// AddUser
        /// </summary>
        public   int Adduser { get; set; }
        /// <summary>
        /// AddTime
        /// </summary>
        public  DateTime Addtime { get; set; }
        /// <summary>
        /// UpUser
        /// </summary>
        public  int Upuser { get; set; }
        /// <summary>
        /// UpTime
        /// </summary>
        public  DateTime Uptime { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int? PageCount { get; set; }

        /// <summary>
        /// 是否降序
        /// </summary>
        public bool isDescending { get; set; }
    }
}