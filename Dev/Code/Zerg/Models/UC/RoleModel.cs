using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YooPoon.WebFramework.Authentication.Entity;

namespace Zerg.Models.UC
{
    public class RoleModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public RoleStatus Status { get; set; }
        /// <summary>
        /// 状态中文描述
        /// </summary>
        public string StatusString
        {
            get
            {
                switch (Status)
                {
                    case RoleStatus.Normal:
                        return "正常";
                    case RoleStatus.Disabled:
                        return "不可用";
                    default:
                        return "正常";
                }
            }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}