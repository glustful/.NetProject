using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Entity.Model;

namespace Zerg.Models.CRM
{
    /// <summary>
    /// 经纪人推送至平台的数据模型
    /// </summary>
    public class BrokerRECClientModel
    {
        /// <summary>
        /// 推荐客户ID
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 经纪人ID
        /// </summary>
        public virtual int Broker { get; set; }
        /// <summary>
        /// 经纪人昵称
        /// </summary>
        public virtual string NickName { get; set; }
        /// <summary>
        /// 经纪人电话
        /// </summary>
        public virtual string BrokerPhone { get; set; }
        /// <summary>
        /// 经纪人性别
        /// </summary>
        public virtual string Sex { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public virtual string RegTime { get; set; }
        /// <summary>
        /// 客户信息ID
        /// </summary>
        public virtual int ClientInfo { get; set; }
        /// <summary>
        /// 客户名
        /// </summary>
        public virtual string Clientname { get; set; }
        /// <summary>
        /// 客户电话
        /// </summary>
        public virtual string Phone { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public virtual string Qq { get; set; }
        /// <summary>
        /// 客户意向户型
        /// </summary>
        public virtual string HouseType { get; set; }
        /// <summary>
        /// 客户意楼盘信息
        /// </summary>
        public virtual string Houses { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public virtual string Note { get; set; }

        /// <summary>
        /// AddUser
        /// </summary>
        public virtual int Adduser { get; set; }
        /// <summary>
        /// AddTime
        /// </summary>
        public virtual DateTime Addtime { get; set; }
        /// <summary>
        /// UpUser
        /// </summary>
        public virtual int Upuser { get; set; }
        /// <summary>
        /// UpTime
        /// </summary>
        public virtual DateTime Uptime { get; set; }
        /// <summary>
        /// 经纪人名
        /// </summary>
        public virtual string Brokername { get; set; }
        /// <summary>
        /// 经纪人等级
        /// </summary>
        public virtual string Brokerlevel { get; set; }
        /// <summary>
        /// 项目名
        /// </summary>
        public virtual string Projectname { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public virtual int Projectid { get; set; }
        /// <summary>
        /// 进度状态
        /// </summary>
        public virtual EnumBRECCType Status { get; set; }
        /// <summary>
        /// 添加/修改类型
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 删除标识
        /// </summary>
        public virtual EnumDelFlag DelFlag { get; set; }
        /// <summary>
        /// 预约时间
        /// </summary>
        public virtual string Appointmenttime { get; set; }
    }
}