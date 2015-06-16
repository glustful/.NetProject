using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.Trading.Product
{
    public class ProductModel
    {

        public int Id { get; set; }

        /// <summary>
        /// 平台商品分类ID
        /// </summary>
        public  int ClassifyId { get; set; }
        /// <summary>
        /// 品牌项目ID
        /// </summary>
        public  int ProductBrandId { get; set; }

        /// <summary>
        /// 带客佣金
        /// </summary>
        public  decimal Commission { get; set; }
        /// <summary>
        /// 成交佣金
        /// </summary>
        public decimal Dealcommission { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public  decimal Price { get; set; }
        /// <summary>
        /// 商家ID
        /// </summary>
        public  int Bussnessid { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public  string Productname { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 子标题
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// 商品状态
        /// </summary>
        public  bool Status { get; set; }
        /// <summary>
        /// 商品主图
        /// </summary>
        public  string Productimg { get; set; }
        /// <summary>
        /// 商家推荐标识
        /// </summary>
        public  bool Recommend { get; set; }
        /// <summary>
        /// 分类排序
        /// </summary>
        public  int Sort { get; set; }
        /// <summary>
        /// 库存计数（拍下、付款）
        /// </summary>
        public  int Stockrule { get; set; }
        /// <summary>
        /// 副标题
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone { get; set; }

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