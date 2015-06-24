using CRM.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.CRM
{
    public class MessageDetailModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public   int Id { get; set; }
        /// <summary>
        /// 发送主题
        /// </summary>
        public  string Title { get; set; }
        /// <summary>
        /// 发送内容
        /// </summary>
        public   string Content { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public  string Sender { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public   string Mobile { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime Addtime { get; set; }
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
        public DateTime? AddtimeBegin { get; set; }

        public DateTime? AddtimeEnd { get; set; }

        public EnumMessageDetailSearchOrderBy? OrderBy { get; set; }
    }
}