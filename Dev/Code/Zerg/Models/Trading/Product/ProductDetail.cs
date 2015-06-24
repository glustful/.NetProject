using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trading.Entity.Model;

namespace Zerg.Models.Trading.Product
{
    public class ProductDetail
    {
        public int Id { get; set; }
        public string Productimg { get; set; }
        public string Productname { get; set; }
        public decimal Price { get; set; }
        public string SubTitle { get; set; }
        public string Acreage { get; set; }
        public string Type { get; set; }
        public string Phone { get; set; }
        public int StockRule { get; set; }
        public string Content { get; set; }
        public string Productimg1 { get; set; }
        public string Productimg2 { get; set; }
        public string Productimg3 { get; set; }
        public string Productimg4 { get; set; }
        public string ProductDetailed { get; set; }
        public decimal RecCommission { get; set; }
        public decimal Dealcommission { get; set; }
        public string Advertisement { get; set; }      
        public string BrandImg { get; set; }
        public string Bname { get; set; }
        public decimal Commission { get; set; }
        public string ClassifyName { get; set; }
        public int Stockrule { get; set; }
        public DateTime Addtime { get; set; }
        public string Sericeinstruction { get; set; }
        public int Status { get; set; }
        public int Recommend { get; set; }
        public virtual ClassifyEntity Classify { get; set; }
        public int BrandId { get; set; }
        public int ClassId { get; set; }
    }
}