
namespace CRM.Entity.Model
{

	public enum EnumBRECCType
	{
		/// <summary>
		/// 审核中
		/// </summary>
        审核中=0,
        /// <summary>
        /// 审核中
        /// </summary>
        审核不通过 = -1,
		/// <summary>
		/// 等待上访
		/// </summary>
        等待上访=1,
        /// <summary>
        /// 等待上访
        /// </summary>
        客人未到 = -2,
		/// <summary>
		/// 洽谈中
		/// </summary>
        洽谈中=2,
		/// <summary>
		/// 洽谈失败
		/// </summary>
        洽谈失败=-3,
		/// <summary>
		/// 洽谈成功
		/// </summary>
        洽谈成功=3,
	}

    public enum EnumDelFlag
    {
        默认=1,
        删除=0
    }
}
