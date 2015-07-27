using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Event.Models.Coupons
{
    //新建CouponCategory所用模型
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ReMark { get; set; }
        public int Count { get; set; }
        public int BrandId { get; set; }
        public string Price { get; set; }
        public int ClassId { get; set; }
        public string Intro { get; set; }
        public string Content { get; set; }
    }
}