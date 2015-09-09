using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Community.Entity.Model.ProductComment;

namespace Zerg.Models.Community
{
    public class ProductComment
    {
        public ProductModel ProductModel { get; set; }
        public List<ProductCommentModel> Comments { get; set; }
    }
}