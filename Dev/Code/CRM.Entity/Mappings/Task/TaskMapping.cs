using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.Task
{
	public class TaskMapping : EntityTypeConfiguration<TaskEntity>, Zerg.Common.Data.IZergMapping
	{
		public TaskMapping()
		{
			ToTable("Task");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int");
			HasOptional(c =>c.TaskPunishment);
			HasOptional(c =>c.TaskAward);
			HasOptional(c =>c.TaskTag);
			HasOptional(c =>c.TaskType);
			Property(c => c.Taskname).HasColumnType("varchar").HasMaxLength(30);
			Property(c => c.Describe).HasColumnType("varchar").HasMaxLength(256).IsOptional();
			Property(c => c.Endtime).HasColumnType("datetime");
			Property(c => c.Adduser).HasColumnType("int");
			Property(c => c.Addtime).HasColumnType("datetime");
			Property(c => c.Upuser).HasColumnType("int");
			Property(c => c.Uptime).HasColumnType("datetime");
		}
	}
}