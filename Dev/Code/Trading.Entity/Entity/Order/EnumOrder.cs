
namespace Trading.Entity.Model
{

	public enum EnumOrderType
	{
		/// <summary>
		/// RecoType
		/// </summary>
		推荐订单,
		/// <summary>
		/// DealType
		/// </summary>
		成交订单,
	}
}

namespace Trading.Entity.Model
{

	public enum EnumShipStatus
	{
		/// <summary>
		/// WaitForView
		/// </summary>
		未看房,
		/// <summary>
		/// MakeAppointment
		/// </summary>
		已预约,
		/// <summary>
		/// HasViewedHouse
		/// </summary>
		已看房,
		/// <summary>
		/// HasBuyedHouse
		/// </summary>
		已买房,
	}
}
