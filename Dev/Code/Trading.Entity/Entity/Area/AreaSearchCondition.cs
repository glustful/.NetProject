namespace Trading.Entity.Entity.Area
{
    public class AreaSearchCondition
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>       
        public int? PageCount { get; set; }
        /// <summary>
        /// 是否降序
        /// </summary>
        public bool IsDescending { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public int[] Ids { get; set; }
        public int? ParentId { get; set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        public string AreaName { get; set; }

        public EnumAreaSearchOrderBy? OrderBy { get; set; }
    }
    public enum EnumAreaSearchOrderBy
    {
        OrderById,
    }
}
