
namespace CRM.Entity.Model
{

	public enum EnumBRECCType
	{
		/// <summary>
		/// 审核中
		/// </summary>
		Normal,
		/// <summary>
		/// 等待上访
		/// </summary>
		Write,
		/// <summary>
		/// 洽谈中
		/// </summary>
		Discuss,
		/// <summary>
		/// 洽谈失败
		/// </summary>
		Defeated,
		/// <summary>
		/// 洽谈成功
		/// </summary>
		Succeed,
	}
}
