using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zerg.Models.Trading.Product
{
    public class ParameterModel
    {
		/// <summary>
		/// 平台商品分类ID
		/// </summary>
		public  int ClassifyId { get; set; }
		/// <summary>
		/// 参数名称
		/// </summary>
		public  string Name { get; set; }
		/// <summary>
		/// 参数排序
		/// </summary>
		public  int Sort { get; set; }
		/// <summary>
		/// AddUser
		/// </summary>
		public  string Adduser { get; set; }
		/// <summary>
		/// AddTime
		/// </summary>
		public  DateTime Addtime { get; set; }
		/// <summary>
		/// UpdUser
		/// </summary>
		public  string Upduser { get; set; }
		/// <summary>
		/// UpdTime
		/// </summary>
		public  DateTime Updtime { get; set; }
    }
}