using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace Event.Entity.Model
{
	public class ParticipationEntity : IBaseEntity
	{
		/// <summary>
		/// ID
		/// </summary>
		public virtual int Id { get; set; }
		/// <summary>
		/// 项目ID
		/// </summary>
		public virtual CrowdEntity Crowd { get; set; }
		/// <summary>
		/// UserName
		/// </summary>
		public virtual string Username { get; set; }
		/// <summary>
		/// UseId
		/// </summary>
		public virtual int UseId { get; set; }
		/// <summary>
		/// Phone
		/// </summary>
		public virtual string Phone { get; set; }
		/// <summary>
		/// AddUser
		/// </summary>
		public virtual int Adduser { get; set; }
		/// <summary>
		/// AddTime
		/// </summary>
		public virtual DateTime Addtime { get; set; }
	}
}