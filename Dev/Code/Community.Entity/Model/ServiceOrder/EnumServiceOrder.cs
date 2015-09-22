
namespace Community.Entity.Model.ServiceOrder
{

    public enum EnumServiceOrderStatus
    {
        /// <summary>
        /// 新建
        /// </summary>
        Created = 0,
        /// <summary>
        /// 已付款
        /// </summary>
        Payed = 1,
        /// <summary>
        /// 订单完成
        /// </summary>
        Successed = 2,
        /// <summary>
        /// 订单关闭
        /// </summary>
        Canceled = 3,
    }
}
