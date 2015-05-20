using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Zerg.Models.Trading.Product
{
    public class ProductDetailModel
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public  int Id { get; set; }
        /// <summary>
        /// 商品_商品ID
        /// </summary>
        public  int ProductId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public  string Productname { get; set; }
        /// <summary>
        /// 商品明细
        /// </summary>
        public  string Productdetail { get; set; }
        /// <summary>
        /// 商品图片（主图）
        /// </summary>
        public  string Productimg { get; set; }
        /// <summary>
        /// 商品附图1
        /// </summary>
        public  string Productimg1 { get; set; }
        /// <summary>
        /// 商品附图2
        /// </summary>
        public  string Productimg2 { get; set; }
        /// <summary>
        /// 商品附图3
        /// </summary>
        public  string Productimg3 { get; set; }
        /// <summary>
        /// 商品附图4
        /// </summary>
        public  string Productimg4 { get; set; }
        /// <summary>
        /// 售后说明
        /// </summary>
        public  string Sericeinstruction { get; set; }
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