using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Zerg.Controllers.CRM
{
    public class testController : ApiController
    {
        public string GetName(string name)
        {
            return string.Format("name:{0}", name);
        }

        public string PostName([FromBody] human human)
        {
            return string.Format("name:{0},age:{1}", human.name, human.age);
        }
    }

    public class human
    {
        public string name { get; set; }
        public int age { get; set; }
    }
}
