using Community.Entity.Model.Area;
using System;
using System.ComponentModel.DataAnnotations;

namespace Zerg.Models.Community
{
    public class AreaModel
    {

        /// <summary>
        /// 行政区划ID
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// 区位码ID
        /// </summary>
        public string Codeid { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Adddate { get; set; }

        public string AdddateString
        {
            get
            {
                return Adddate.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }


        /// <summary>
        /// 父级ID
        /// </summary>
        public AreaModel Parent { get; set; }

        /// <summary>
        /// 父级名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 区位名
        /// </summary>
        public string Name { get; set; }



    }
}