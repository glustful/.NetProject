using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YooPoon.Core.Data;

namespace Event.Entity.Entity.OtherCoupon
{
    public class OtherCouponCategory : IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ReMark { get; set; }
        public int Count { get; set; }
        public int BrandId { get; set; }
        public string Price { get; set; }
        public int ClassId { get; set; }
    }
}
