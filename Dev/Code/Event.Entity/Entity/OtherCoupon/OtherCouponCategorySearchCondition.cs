using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Entity.Entity.OtherCoupon
{
    public class OtherCouponCategorySearchCondition
    {
        public int? Page { set; get; }

        public int? PageSize { get; set; }

        public int? BrandId { get; set; }
        public bool IsDescending { get; set; }
        public string Name { get; set; }
        public EnumOtherCouponCategorySearchOrderBy? OrderBy { get; set; }
    }
    public enum EnumOtherCouponCategorySearchOrderBy
    {
        OrderById,
    }
}
