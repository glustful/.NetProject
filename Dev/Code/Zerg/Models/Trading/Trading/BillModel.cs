using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.Trading.Trading
{
    public class BillModel
    {
        public int orderId { get; set; }
        public decimal Actualamount { get; set; }
        public string beneficiarynumber { get; set; }
        public string remark { get; set; }
    }
}