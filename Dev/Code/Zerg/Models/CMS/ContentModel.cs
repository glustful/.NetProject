using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Entity.Model;

namespace Zerg.Models
{
    public class ContentModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AddUser { get; set; }
        public EnumContentStatus Status { get; set; }
        public string Channel { get; set; }
        public int ChannelId { get; set; }
        public string StatusString
        {
            get
            {
                switch (Status)
                {
                    case EnumContentStatus.Created:
                        return "刚新建";
                    case EnumContentStatus.Deleted:
                        return "已删除";
                    case EnumContentStatus.Published:
                        return "已发布";
                    default:
                        return "";
                }
            }
        }
    }
}