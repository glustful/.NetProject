using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.CMS
{
    public class AdvertisementModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Continue { get; set; }
        public string Detail { get; set; }
        public int ContentId { get; set; }
        public string ContentTitle { get; set; }
    }
}