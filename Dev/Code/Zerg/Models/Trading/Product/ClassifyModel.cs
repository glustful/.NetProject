using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.Trading.Product
{
    public class ClassifyModel
    {

        /// <summary>
        /// 上级商品分类ID（若无则为-1）
        /// </summary>
        public  int ClassifyId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public  string Name { get; set; }
        /// <summary>
        /// 分类排序
        /// </summary>
        public  int Sort { get; set; }
        /// <summary>
        /// AddUser
        /// </summary>
        public  string Adduser { get; set; }
        /// <summary>
        /// UpdUser
        /// </summary>
        public  string Upduser { get; set; }

    }
}