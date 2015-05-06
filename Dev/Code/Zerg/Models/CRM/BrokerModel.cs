using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.CRM
{
    public class BrokerModel
    {
        /// <summary>
        /// 经纪人ID
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// 等级ID
        /// </summary>
        public virtual int LevelId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual int UserId { get; set; }
        /// <summary>
        /// 经纪人名
        /// </summary>
        public virtual string Brokername { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string Nickname { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public virtual string Realname { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public virtual string Sfz { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public virtual string Sexy { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public virtual int Phone { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public virtual int Qq { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public virtual int Zip { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public virtual string Headphoto { get; set; }
        /// <summary>
        /// 积分总额
        /// </summary>
        public virtual int Totalpoints { get; set; }
        /// <summary>
        /// 账户金额
        /// </summary>
        public virtual decimal Amount { get; set; }
        /// <summary>
        /// 经纪人等级
        /// </summary>
        public virtual string Agentlevel { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public virtual string Usertype { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public virtual string Address { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public virtual DateTime Regtime { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>
        public virtual bool Delflag { get; set; }
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
        /// 添加/修改类型
        /// </summary>
        public virtual string Type { get; set; }
    }

    /// <summary>
    /// Broker经纪人分页、搜索条件
    /// </summary>
    public class BrokerSearchModel
    {
        /// <summary>
        /// 分页数量
        /// </summary>
        public virtual string Pageindex { get; set; }
    }
}