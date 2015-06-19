using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Entity.Model;

namespace Zerg.Models.CRM
{
    public class BrokerModel
    {
        /// <summary>
        /// UCId
        /// </summary>
        public int UcId { get; set; }
        /// <summary>
        /// UC用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// UC密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 是否记住密码
        /// </summary>
        public bool Remember { get; set; }
        /// <summary>
        /// UC状态
        /// </summary>
        public int Status { get; set; }

        public virtual int Id { get; set; }

        /// <summary>
        /// 用户基础信息ID
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
        /// 身份证正面照
        /// </summary>
        public virtual string SfzPhoto { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual string Sexy { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Phone { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public virtual int Qq { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public virtual int Zip { get; set; }
        /// <summary>
        /// EmAIL
        /// </summary>
        public virtual string Email { get; set; }
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
        /// 等级ID
        /// </summary>
        public virtual int LevelId { get; set; }

        /// <summary>
        /// 经纪人等级名称
        /// </summary>
        public virtual string Agentlevel { get; set; }
        /// <summary>
        /// 用户类型（会员 经纪人 财务 小秘书）
        /// </summary>
        //public virtual string Usertype { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public virtual string Address { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public virtual DateTime Regtime { get; set; }
        public virtual string rgtime { get; set; }
        /// <summary>
        /// 用户状态（删除0 注销-1 正常1）
        /// </summary>
        public virtual int State { get; set; }


        /// <summary>
        /// 所属的合伙人ID（同ID）
        /// </summary>
        public virtual int PartnersId { get; set; }


        /// <summary>
        /// 所属的合伙人姓名（同经纪人名）
        /// </summary>
        public virtual string PartnersName { get; set; }




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
        /// 添加/删除类型
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public virtual string MobileYzm { get; set; }
        /// <summary>
        /// 验证码密文
        /// </summary>
        public virtual string Hidm { get; set; }

        /// <summary>
        /// 邀请码
        /// </summary>
        public virtual string inviteCode { get; set; }
      



        /// <summary>
        /// 确认密码
        /// </summary>
        public virtual string SecondPassword { get; set; }

        /// <summary>
        /// 用户类型枚举 （经纪人 broker  ，  管理员 manager）
        /// </summary>
        public virtual EnumUserType UserType { get; set; }

        /// <summary>
        /// 用户状态 （删除0 注销-1 正常1）
        /// </summary>    
        public enum EnumUserState
        {
            Delete = 0,
            Cancel = -1,
            OK = 1
        }

        public bool ValidateModel(out string msg)
        {
            msg = "";
            if (string.IsNullOrEmpty(UserName))
            {
                msg = "用户名不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(Password)) return false;
            if (string.IsNullOrEmpty(SecondPassword)) return false;
            if (string.IsNullOrEmpty(Phone)) return false;
            if (string.IsNullOrEmpty(MobileYzm)) return false;
            if (string.IsNullOrEmpty(Hidm)) return false;
            return true;
        }
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