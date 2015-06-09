
namespace Trading.Entity.Model
{

	public enum EnumOrderType
	{
		/// <summary>
		/// RecoType
		/// </summary>
		推荐订单=1,
		/// <summary>
		/// DealType
		/// </summary>
		成交订单=2,
        带客订单=3,
	}
}

namespace Trading.Entity.Model
{

	public enum EnumOrderStatus
	{
		/// <summary>
		/// 默认未处理
		/// </summary>
		默认=0,
		/// <summary>
        /// 审核通过，等待流程结转
		/// </summary>
		审核通过=1,
		/// <summary>
		/// HasViewedHouse
		/// </summary>
		审核失败=-1,
		/// <summary>
		/// HasBuyedHouse
		/// </summary>
		已结转=2,
	}
}
