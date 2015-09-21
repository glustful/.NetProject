using Community.Entity.Model.Product;
using Community.Entity.Model.ServiceOrder;

namespace Community.Entity.Model.ServiceOrderDetail
{
	public class ServiceOrderDetailSearchCondition
	{
		/// <summary>
		/// 页码
		/// </summary>
		public int? Page { get; set; }

		/// <summary>
		/// 每页大小
		/// </summary>
		public int? PageCount { get; set; }

		/// <summary>
		/// 是否降序
		/// </summary>
		public bool IsDescending { get; set; }


		public int[] Ids { get; set; }
        public EnumServiceOrderStatus? Status { get; set; }
        public string Addusers { get; set; }


		public ServiceOrderEntity[] ServiceOrders { get; set; }



		public ProductEntity[] Products { get; set; }



		public decimal? CountBegin { get; set; }

		public decimal? CountEnd { get; set; }



		public decimal? PriceBegin { get; set; }

		public decimal? PriceEnd { get; set; }




		public EnumServiceOrderDetailSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumServiceOrderDetailSearchOrderBy
	{

		OrderById,

	}

}