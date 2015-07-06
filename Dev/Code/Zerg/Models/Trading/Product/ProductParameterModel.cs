using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.Trading.Product
{
    public class ProductParameterModel
    {
        public int ParameterId { get; set; }

        public string ParameterString { get; set; }

        public int ValueId { get; set; }

        public string Value { get; set; }
    }
}