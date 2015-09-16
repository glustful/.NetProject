using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Event.Entity.Entity.Coupon;
using Event.Service.Coupon;

namespace Zerg.Event.Controllers
{
    public class TestTimeController : Controller
    {
        private readonly ICouponService _couponService;

        public TestTimeController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        /// <summary>
        /// 批量插入测试
        /// 方法1耗时16秒以上
        /// 方法2耗时22秒以上
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var sw = new Stopwatch();
            var sw2 = new Stopwatch();

            #region 构造数据
            var random = new Random();
            var list = new List<Coupon>();
            for (var i = 0; i < 1000; i++)
            {
                list.Add(new Coupon
                {
                    Number = DateTime.Now.ToString("HHmmss") + random.Next(10000000, 100000000),
                    Price = 10,
                    Status = EnumCouponStatus.Actived,
                    CouponCategoryId = 1
                });
            }
            #endregion
            sw.Start();
            _couponService.BulkCreate(list);
            sw.Stop();

            sw2.Start();
            foreach (var p in list)
            {
                _couponService.Create(p);
            }
            sw2.Stop();

            @ViewBag.t1 = sw.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture);
            @ViewBag.t2 = sw2.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture);
            return View();
        }
	}
}