using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Entity.Model;

namespace Zerg.Models.CMS
{
    public class ChannelModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EnumChannelStatus Status { get; set; }
       // public ChannelModel Parent { get; set; }
        public int ParentId { get; set; }
        public string StatusString
        {
            get
            {
                switch (Status)
                {
                    case EnumChannelStatus.Normal:
                        return "正常";
                    case EnumChannelStatus.Deleted:
                        return "已删除";
                    default:
                        return "";
                }
            }
        }
    }
}