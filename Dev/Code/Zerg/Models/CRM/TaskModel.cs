using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM.Entity.Model;

namespace Zerg.Models.CRM
{
    public class TaskModel
    {
        /// <summary>
        /// 任务属性ID
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// 失败惩罚ID
        /// </summary>
        public int TaskPunishment { get; set; }
        /// <summary>
        /// 发放奖励ID
        /// </summary>
        public int TaskAward { get; set; }
        /// <summary>
        /// 任务目标ID
        /// </summary>
        public int TaskTag { get; set; }
        /// <summary>
        /// 任务类型ID
        /// </summary>
        public int TaskType { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Taskname { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime Endtime { get; set; }
        /// <summary>
        /// AddUser
        /// </summary>
        public int Adduser { get; set; }
        /// <summary>
        /// AddTime
        /// </summary>
        public DateTime Addtime { get; set; }
        /// <summary>
        /// UpUser
        /// </summary>
        public virtual int Upuser { get; set; }
        /// <summary>
        /// UpTime
        /// </summary>
        public DateTime Uptime { get; set; }
    }
}