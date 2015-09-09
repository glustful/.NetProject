
namespace Community.Entity.Model.Order
{

	public enum EnumOrderStatus
	{
		/// <summary>
		/// 新建
		/// </summary>
		Created,
		/// <summary>
		/// 已付款
		/// </summary>
		Payed,
		/// <summary>
		/// 配送中
		/// </summary>
		Delivering,
		/// <summary>
		/// 订单完成
		/// </summary>
		Successed,
		/// <summary>
		/// 订单关闭
		/// </summary>
		Canceled,
	}
}
