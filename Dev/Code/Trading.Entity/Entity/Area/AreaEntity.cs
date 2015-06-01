using YooPoon.Core.Data;

namespace Trading.Entity.Entity.Area
{
    public class AreaEntity : IBaseEntity
    {
        public int Id { get; set; }
        public string AreaName { get; set; }
        public int ParentId { get; set; }
        public int Level { get; set; }
    }
}
