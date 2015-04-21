using YooPoon.Core.Data;
using System.Data.Entity.ModelConfiguration;
using CRM.Entity.Model;

namespace CRM.Entity.Mappings.TaskList
{
	public class TaskListMapping : EntityTypeConfiguration<TaskListEntity>, IMapping
	{
		public TaskListMapping()
		{
			ToTable("TaskList");
			HasKey(c => c.Id);
			Property(c => c.Id).HasColumnType("int");
			HasOptional(c =>c.Task);
			HasOptional(c =>c.Broker);
			Property(c => c.Taskschedule).HasColumnType("varcahr").HasMaxLength(50);
		}
	}
}