using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.UC
{
    public class ControllerModel
    {
        public string ControllerName { get; set; }


        public string ControllerFullName { get; set; }

        public string Description { get; set; }

        public List<ActionPermissionModel> Actions { get; set; }
    }

    public class ActionPermissionModel
    {
        public string ActionName { get; set; }

        public string Description { get; set; }

        public bool IsAllowed { get; set; }
    }
}