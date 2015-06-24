using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models
{
    public class TagDetailModel
    {
        public TagModel Tag { get; set; }

        public List<ContentModel> Contents { get; set; }
    }
}