using System;
using YooPoon.Core.Data;

namespace Community.Entity.Model.Area
{
	public class AreaEntity : IBaseEntity
	{
		/// <summary>
		/// 行政区划ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 区位码ID
		/// </summary>
		public virtual string CodeId { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime AddDate { get; set; }
		/// <summary>
		/// 父级ID
		/// </summary>
		public virtual AreaEntity Parent { get; set; }
		/// <summary>
		/// 区位名
		/// </summary>
		public virtual string Name { get; set; }
	}
}