







using System;

namespace Trading.Entity.Model
{
	public class OrderSearchCondition
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


		public OrderDetailEntity[] OrderDetails { get; set; }


		public string Ordercode { get; set; }



		public EnumOrderType? Ordertype { get; set; }



		public int[] Shipstatuses { get; set; }

        public int? Shipstatus { get; set; }



		public int? Status { get; set; }



		public int? BusId { get; set; }



		public string Busname { get; set; }



		public int? AgentId { get; set; }



		public string Agentname { get; set; }



		public string Agenttel { get; set; }



		public string Customname { get; set; }



		public string Remark { get; set; }



		public DateTime? AdddateBegin { get; set; }

		public DateTime? AdddateEnd { get; set; }



		public string Adduser { get; set; }



		public string Upduser { get; set; }



		public DateTime? UpddateBegin { get; set; }

		public DateTime? UpddateEnd { get; set; }




		public EnumOrderSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumOrderSearchOrderBy
	{

        OrderById,
        OrderByAddTime
	}

}