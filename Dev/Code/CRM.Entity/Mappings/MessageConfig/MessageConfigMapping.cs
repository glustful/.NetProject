using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.MessageConfig
{
	public class MessageConfigMapping : EntityTypeConfiguration<MessageConfigEntity>, Zerg.Common.Data.IZergMapping
	{
		public MessageConfigMapping()
		{
			ToTable("MessageConfig");
			HasKey(c => c.Id);
			 
			Property(c => c.Name).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Template).HasColumnType("varchar").HasMaxLength(800);
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
		}
	}
}