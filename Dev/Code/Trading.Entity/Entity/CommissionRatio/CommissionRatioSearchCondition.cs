using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Entity.Entity.CommissionRatio
{
    public class CommissionRatioSearchCondition
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int[] Id { get; set; }
        public bool IsDescending { get; set; }
        public EnumCommissionSearchOrderBy? OrderBy { get; set; }
    }
    public enum EnumCommissionSearchOrderBy
    {

        OrderById,

    }
}
