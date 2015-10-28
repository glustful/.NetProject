using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.Entity.Model.Service
{
    public class ServiceSearchCondition
    {
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
        public bool IsDescending { get; set; }
        public DateTime? AddtimeBegin { get; set; }

        public DateTime? AddtimeEnd { get; set; }
        public int[] Ids { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        //public int? ServiceId { get; set; }
        public string Link { get; set; }
        public int[] AddUsers { get; set; }
        public int[] UpUsers { get; set; }
        public EnumServiceSearchOrderBy? OrderBy { get; set; }

    }


    public enum EnumServiceSearchOrderBy
    {

        OrderById,    

    }
}
