using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Entity.Entity.Coupon
{
    public class CouponSearchCondition
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

        public int? CouponCategoryId { get; set; }


        public int? Ids { get; set; }
        public string Number { get; set; }
        public int? Status { get; set; }

        public EnumCouponSearchOrderBy? OrderBy { get; set; }
    }
    public enum EnumCouponSearchOrderBy
    {
        OrderById,
    }
}
