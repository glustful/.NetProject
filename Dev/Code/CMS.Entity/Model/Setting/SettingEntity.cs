using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace CMS.Entity.Model
{
	public class SettingEntity : IBaseEntity
	{
		/// <summary>
		/// Id
		/// </summary>
		public  int Id { get; set; }
		/// <summary>
		/// 键
		/// </summary>
		public  string Key { get; set; }
		/// <summary>
		/// 值
		/// </summary>
		public  string Value { get; set; }
	}
}