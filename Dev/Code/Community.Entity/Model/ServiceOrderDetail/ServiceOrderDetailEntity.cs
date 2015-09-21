using Community.Entity.Model.Product;
using Community.Entity.Model.ServiceOrder;
using YooPoon.Core.Data;

namespace Community.Entity.Model.ServiceOrderDetail
{
	public class ServiceOrderDetailEntity : IBaseEntity
	{
		/// <summary>
		/// Id
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// Order_Id
		/// </summary>
		public virtual ServiceOrderEntity ServiceOrder { get; set; }
		/// <summary>
		/// Product_Id
		/// </summary>
		public virtual ProductEntity Product { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public virtual string ProductName { get; set; }
        /// <summary>
        /// 商品主图
        /// </summary>
        public virtual string MainImg { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string OrderNo { get; set; }
		/// <summary>
		/// Count
		/// </summary>
		public virtual decimal Count { get; set; }
		/// <summary>
		/// Price
		/// </summary>
		public virtual decimal Price { get; set; }
	}
}