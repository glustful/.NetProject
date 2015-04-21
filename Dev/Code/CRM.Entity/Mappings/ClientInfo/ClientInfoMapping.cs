using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.ClientInfo
{
	public class ClientInfoMapping : EntityTypeConfiguration<ClientInfoEntity>, IMapping
	{
		public ClientInfoMapping()
		{
			ToTable("ClientInfo");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int").IsOptional();
			Property(c => c.Clientname).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Phone).HasColumnType("varchar").HasMaxLength(11);
			Property(c => c.Housetype).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Houses).HasColumnType("varchar").HasMaxLength(50);
			Property(c => c.Note).HasColumnType("varchar").HasMaxLength(256);
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}