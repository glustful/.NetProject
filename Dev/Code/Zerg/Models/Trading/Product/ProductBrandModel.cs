using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.Trading.Product
{
    public class ProductBrandModel
    {
        /// <summary>
        /// 品牌名称
        /// </summary>
        /// 
         public  string Bname { get; set; }
        /// <summary>
        /// 品牌图片(存图片URL)
        /// </summary>
        public  string Bimg { get; set; }
        public string SubTitle { get; set; }

        /// <summary>
        /// AdTitle
        /// </summary>
        public string AdTitle { get; set; }
        /// <summary>
        /// AddUser
        /// </summary>
        public  string Adduser { get; set; }
        /// <summary>
        /// AddTime
        /// </summary>
        public  DateTime Addtime { get; set; }
        /// <summary>
        /// Content
        /// </summary>
        public string Content    { get; set;  }
        /// <summary>
        /// UpdUser
        /// </summary>
        public  string Upduser { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// UpdTime
        /// </summary>
        public  DateTime Updtime { get; set; }
        public Dictionary<string,string> Parameters { get; set; }

        public List<ProductModel> Products { get; set; }
       
    }
}