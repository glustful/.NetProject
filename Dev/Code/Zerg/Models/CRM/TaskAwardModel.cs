using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.CRM
{
    public class TaskAwardModel
    {
        /// <summary>
        /// 发放奖励ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 奖励名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 奖励描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 目标值
        /// </summary>
        public string Value { get; set; }
    }
}