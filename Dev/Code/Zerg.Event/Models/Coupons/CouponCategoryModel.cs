using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trading.Entity.Model;

namespace Zerg.Event.Models.Coupons
{
    //优惠券页面显示所用模型
    public class CouponCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ReMark { get; set; }
        public string Number { get; set; }
        public int Count { get; set; }
        public string Price { get; set; }
        public string SubTitle { get; set; }
        public string BrandImg { get; set; }
        public string BrandName { get; set; }
        public Dictionary<string, string> ProductParamater { get; set; }
        
    }
}