using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.Trading.Product
{
    public class ProductBrandParameterModel
    {
        /// <summary>
        /// 品牌ID
        /// </summary>
        public  int ProductBrandId { get; set; }
        /// <summary>
        /// 商品参数排序
        /// </summary>
        public  int Sort { get; set; }
        /// <summary>
        /// 参数值名称
        /// </summary>
        public  string Parametername { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public  string Parametervaule { get; set; }
        /// <summary>
        /// AddUser
        /// </summary>
        public  string Adduser { get; set; }
        /// <summary>
        /// AddTime
        /// </summary>
        public  DateTime Addtime { get; set; }
        /// <summary>
        /// UpdUser
        /// </summary>
        public  string Upduser { get; set; }
        /// <summary>
        /// UpdTime
        /// </summary>
        public  DateTime Updtime { get; set; }
    }
}