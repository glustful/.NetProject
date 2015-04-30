using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CRM.Entity.Model
{
	public class BankEntity : IBaseEntity
	{
		/// <summary>
		/// 银行ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 银行名字
		/// </summary>
		public virtual string Codeid { get; set; }
		/// <summary>
		/// 开户行地址
		/// </summary>
		public virtual string address { get; set; }
	}
}