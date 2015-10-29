using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.Community
{
    public class ServiceModel
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Class { get; set; }
       // public virtual int ServiceId { get; set; }
        public string Link { get; set; }
        public virtual int AddUser { get; set; }
        public virtual int UpUser { get; set; }
        public virtual DateTime AddTime { get; set; }
        public virtual DateTime UpTime { get; set; }
    }
}