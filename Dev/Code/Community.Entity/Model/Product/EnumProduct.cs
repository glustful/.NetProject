
namespace Community.Entity.Model.Product
{

	public enum EnumProductType
	{
		/// <summary>
		/// 实物商品
		/// </summary>
		Product,
		/// <summary>
		/// 服务商品
		/// </summary>
		Service,
	}

    public enum EnumProductStatus
    {   
        /// <summary>
        /// 正常状态
        /// </summary>
        Normal,
        /// <summary>
        /// 删除状态
        /// </summary>
        Delete
    }
}
