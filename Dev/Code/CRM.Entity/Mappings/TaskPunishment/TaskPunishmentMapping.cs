using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.TaskPunishment
{
	public class TaskPunishmentMapping : EntityTypeConfiguration<TaskPunishmentEntity>, IMapping
	{
		public TaskPunishmentMapping()
		{
			ToTable("TaskPunishment");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int");
			Property(c => c.Name).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.Describe).HasColumnType("varchar").HasMaxLength(256).IsOptional();
			Property(c => c.Value).HasColumnType("varchar").HasMaxLength(50);
		}
	}
}