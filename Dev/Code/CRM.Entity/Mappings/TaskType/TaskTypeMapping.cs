using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.TaskType
{
	public class TaskTypeMapping : EntityTypeConfiguration<TaskTypeEntity>, Zerg.Common.Data.IZergMapping
	{
		public TaskTypeMapping()
		{
			ToTable("TaskType");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int");
			Property(c => c.Name).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.Describe).HasColumnType("varchar").HasMaxLength(256).IsOptional();
		}
	}
}