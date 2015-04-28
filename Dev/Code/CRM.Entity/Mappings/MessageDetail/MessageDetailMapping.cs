using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.MessageDetail
{
	public class MessageDetailMapping : EntityTypeConfiguration<MessageDetailEntity>, IMapping
	{
		public MessageDetailMapping()
		{
			ToTable("MessageDetail");
			HasKey(c => c.Id);
			 
			Property(c => c.Title).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Content).HasColumnType("varchar").HasMaxLength(500);
			Property(c => c.Sender).HasColumnType("varchar");
            Property(c => c.Mobile).HasColumnType("varchar");
			Property(c => c.Addtime).HasColumnType("datetime");
		}
	}
}