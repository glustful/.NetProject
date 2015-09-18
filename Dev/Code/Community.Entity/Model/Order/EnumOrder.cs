
namespace Community.Entity.Model.Order
{

	public enum EnumOrderStatus
	{
		/// <summary>
		/// 新建
		/// </summary>
		Created=0,
		/// <summary>
		/// 已付款
		/// </summary>
		Payed=1,
		/// <summary>
		/// 配送中
		/// </summary>
		Delivering=2,
		/// <summary>
		/// 订单完成
		/// </summary>
		Successed=3,
		/// <summary>
		/// 订单关闭
		/// </summary>
		Canceled=4,
	}
}
