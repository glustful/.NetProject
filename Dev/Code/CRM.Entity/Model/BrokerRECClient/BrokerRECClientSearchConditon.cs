







using System;

namespace CRM.Entity.Model
{
	public class BrokerRECClientSearchCondition
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





		public BrokerEntity[] Brokers { get; set; }



		public ClientInfoEntity[] ClientInfos { get; set; }



		public string Clientname { get; set; }



		public int? Phone { get; set; }



		public int? Qq { get; set; }



		public int[] Addusers { get; set; }



		public DateTime? AddtimeBegin { get; set; }

		public DateTime? AddtimeEnd { get; set; }



		public int[] Upusers { get; set; }



		public DateTime? UptimeBegin { get; set; }

		public DateTime? UptimeEnd { get; set; }



		public string Brokername { get; set; }



		public string Brokerlevel { get; set; }



		public string Projectname { get; set; }



		public int[] Projectids { get; set; }



		public BrokerEntity[] SecretaryIDs { get; set; }



		public string SecretaryPhone { get; set; }



		public BrokerEntity[] WriterIDs { get; set; }



		public string WriterPhone { get; set; }



		public BrokerEntity[] AccountantIDs { get; set; }



		public bool? Payment { get; set; }



		public Decimal Amount { get; set; }






		public EnumBrokerRECClientSearchOrderBy? OrderBy { get; set; }

	}


	public enum EnumBrokerRECClientSearchOrderBy
	{

		OrderById,

		OrderByAmount,

	}

}