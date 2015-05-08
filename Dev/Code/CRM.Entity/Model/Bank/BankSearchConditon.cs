using System;

namespace CRM.Entity.Model
{
	public class BankSearchCondition
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
		public bool isDescending { get; set; }

		public int[] Ids { get; set; }


		public string Codeid { get; set; }

		public string address { get; set; }

		public EnumBankSearchOrderBy? OrderBy { get; set; }
	}

	public enum EnumBankSearchOrderBy
	{
		OrderById,
	}
}